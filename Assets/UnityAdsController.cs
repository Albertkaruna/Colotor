using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsController : MonoBehaviour
{
	public static UnityAdsController instance;
	private string GameID = "1197531";

	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		Advertisement.Initialize (GameID);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public void ShowUniytinterstitialAd ()
	{
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
	}

	public void ShowUnityRewardedAd ()
	{
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}

	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			//				Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			HeyZapAdsController.instance.RewardToPlayer ();
			UIManagement.instance.GetInRewardCompleteScreen ();
			break;
		}
	}

}
