using UnityEngine;
using System.Collections;
using Heyzap;
using UnityEngine.UI;
using System.IO;

public class HeyZapAdsController : MonoBehaviour
{
	public static HeyZapAdsController instance;
	private string HeyZapPublisherID = "c0d047674442aff18af093539748d8ae";

	public static bool AdChecker;


	void Awake ()
	{
		AdChecker = false;
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		HeyzapAds.Start (HeyZapPublisherID, HeyzapAds.FLAG_NO_OPTIONS);
	}

	// Use this for initialization
	void Start ()
	{
		HZIncentivizedAd.Fetch ();

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void ShowInterstitialAd ()
	{
		if (HZInterstitialAd.IsAvailable () && AdChecker) {
			HZInterstitialAd.Show ();
			AdChecker = false;
		} else if (AdChecker) {
			AdChecker = false;
			UnityAdsController.instance.ShowUniytinterstitialAd ();
		}
	}

	public void ShowIncentivezedAds ()
	{
		if (HZIncentivizedAd.IsAvailable ()) {
			HZIncentivizedAd.Show ();
			ShowIncentiveAds ();
		} else {
			HZIncentivizedAd.Fetch ();
			UnityAdsController.instance.ShowUnityRewardedAd ();
		}
	}

	private void ShowIncentiveAds ()
	{		
		HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag) {


			if (adState.Equals ("fetch_failed")) {
				//TELL USER TO CHECK CONNECTION
				HZIncentivizedAd.Fetch ();
			}
			if (adState.Equals ("incentivized_result_complete")) {
				// Give reward to the player
				RewardToPlayer ();
				UIManagement.instance.GetInRewardCompleteScreen ();
				HZIncentivizedAd.Fetch ();
			}
			if (adState.Equals ("incentivized_result_incomplete")) {
				// The user did not watch the entire video and should not be given a reward.
				HZIncentivizedAd.Fetch ();
			}
		};

		HZIncentivizedAd.SetDisplayListener (listener);
	}

	public void RewardToPlayer ()
	{
		GameController.instance.Hints += 3;
		GameController.instance.SetHintsTxt (GameController.instance.Hints);
		GameController.instance.SetRemainingHintsTxt (GameController.instance.Hints);
		PlayerPrefController.instance.SetHintsKey (GameController.instance.Hints);
	}
}
