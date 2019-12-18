using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{

	public static UIManagement instance;

	//    Toggles the setting button functionality
	private bool SettingsToggle = true;
	public AudioSource MusicSrc, SoundSrc;
	private bool s, ss;
	public AudioClip[] BGClips;
	public AudioClip Victory, Failed, ButtonClick, PickTacktick;
	public Sprite Sound1, Sound2, Music1, Music2;
	public Image MusicImage, SoundImage;
	[SerializeField]
	public static int AdStepCount = 0, RateItStepCount = 0;

	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		s = true;
		ss = true;

	}


	// Use this for initialization
	void Start ()
	{
		MusicSound ();

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}




	// Play button function
	public void Play (GameObject PlayButton)
	{
		PlayButtonClickSnd ();
		GameController.instance.ExitBtn.gameObject.SetActive (false);
		PlayButton.SetActive (false);
		GameController.instance.SettingBtn.gameObject.SetActive (false);
		GameController.instance.PlayBtnStuff.GetComponent<Animator> ().SetBool ("play", false);
		GameController.instance.BackKey = "WorldSelect";
		GameController.instance.LetEscapeGo ();
	}

	public void Settings ()
	{
		StartCoroutine (SettingsControl ());
	}


	public void Facebook ()
	{
		PlayButtonClickSnd ();
		Application.OpenURL ("https://www.facebook.com/HyperZeta-522463037937722");
	}

	public void Twitter ()
	{
		PlayButtonClickSnd ();
		Application.OpenURL ("https://twitter.com/hyper_zeta");
	}



	void MusicSound ()
	{
		if (PlayerPrefs.GetInt ("Music") == 1) {
			MusicSrc.enabled = false;
			s = false;
			MusicImage.sprite = Music2;
		}
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			SoundSrc.enabled = false;
			ss = false;
			SoundImage.sprite = Sound2;
		}
	}


	public void SoundBtn (Image b)
	{
		PlayButtonClickSnd ();
		SoundSrc.enabled = !SoundSrc.enabled;
		if (ss) {
			ss = false;
			b.sprite = Sound2;
			PlayerPrefs.SetInt ("Sound", 1);
		} else if (!ss) {
			ss = true;
			b.sprite = Sound1;
			MusicSrc.Play ();
			PlayerPrefs.SetInt ("Sound", 0);
		}
	}

	public void MusicBtn (Image b)
	{
		PlayButtonClickSnd ();
		MusicSrc.enabled = !MusicSrc.enabled;
		if (s) {
			s = false;
			b.sprite = Music2;
			PlayerPrefs.SetInt ("Music", 1);
		} else if (!s) {
			s = true;
			b.sprite = Music1;
			MusicSrc.Play ();
			PlayerPrefs.SetInt ("Music", 0);
		}
	}

	public void Exit ()
	{
		PlayButtonClickSnd ();
		Application.Quit ();
	}


	public void PlayButtonClickSnd ()
	{
		SoundSrc.PlayOneShot (ButtonClick);
	}

	public void PlayTacktickSnd ()
	{
		SoundSrc.PlayOneShot (PickTacktick);
		try {
			Destroy (Instantiate (GameController.instance.PickEff, GameController.instance.CurrentPlayer.transform.position, Quaternion.identity), 3f);
		} catch (System.Exception ex) {
			UnityEngine.Debug.Log (ex);
		}
	}

	public void PlayVictorySnd ()
	{
		SoundSrc.PlayOneShot (Victory);
		try {
			Destroy (Instantiate (GameController.instance.VictoryEff, GameController.instance.CurrentPlayer.transform.position, Quaternion.identity), 3f);
		} catch (System.Exception ex) {
			UnityEngine.Debug.Log (ex);
		}
	}

	public void PlayFailureSnd ()
	{
		SoundSrc.PlayOneShot (Failed);
		try {
			Destroy (Instantiate (GameController.instance.FailEff, GameController.instance.CurrentPlayer.transform.position, Quaternion.identity), 3f);
		} catch (System.Exception ex) {
			UnityEngine.Debug.Log (ex);
		}
	}






	IEnumerator SettingsControl ()
	{
		Animator anim = GameController.instance.SettingPanel.GetComponent<Animator> ();
		if (SettingsToggle) {
			SettingsToggle = false;
			yield return new WaitForSeconds (0.3f);
			PlayButtonClickSnd ();
			anim.SetInteger ("settings", 1);
			yield return new WaitForSeconds (0.4f);
			anim.SetInteger ("settings", 3);
			GameController.instance.BackKey = "Settings";
		} else {
			SettingsToggle = true;
			yield return new WaitForSeconds (0.3f);
			PlayButtonClickSnd ();
			anim.SetInteger ("settings", 4);
			yield return new WaitForSeconds (0.4f);
			anim.SetInteger ("settings", 2);
			int ActiveScene = SceneManager.GetActiveScene ().buildIndex;
			if (ActiveScene > 0) {
				GameController.instance.BackKey = "OnGameplay";	
			} else {
				GameController.instance.BackKey = "Exit";	
			}

			if (SceneManager.GetActiveScene ().buildIndex > 0) {
				ShowInterstitialAd ();
			}
		}
		GameController.instance.LetEscapeGo ();

		if (GameController.instance.ResetWindow.GetComponent<Animator> ().GetBool ("gamereset"))
			GameController.instance.ResetWindow.GetComponent<Animator> ().SetBool ("gamereset", false);
	}

	public void  WorldOneStart (GameObject LVL)
	{
		if (LVL.name == "1") {
			StartCoroutine (HideUnwantedThings (1, 1, 0));
		} else if (LVL.name == "2") {
			StartCoroutine (HideUnwantedThings (1, 2, -10));
		} else if (LVL.name == "3") {
			StartCoroutine (HideUnwantedThings (1, 3, -20));
		} else if (LVL.name == "4") {
			StartCoroutine (HideUnwantedThings (1, 4, -30));
		} else if (LVL.name == "5") {
			StartCoroutine (HideUnwantedThings (1, 5, -40));
		}
	}

	public void  WorldTwoStart (GameObject LVL)
	{
		
		if (LVL.name == "1") {
			StartCoroutine (HideUnwantedThings (2, 1, 0));
		} else if (LVL.name == "2") {
			StartCoroutine (HideUnwantedThings (2, 2, -10));
		} else if (LVL.name == "3") {
			StartCoroutine (HideUnwantedThings (2, 3, -20));
		} else if (LVL.name == "4") {
			StartCoroutine (HideUnwantedThings (2, 4, -30));
		} else if (LVL.name == "5") {
			StartCoroutine (HideUnwantedThings (2, 5, -40));
		} else if (LVL.name == "6") {
			StartCoroutine (HideUnwantedThings (2, 6, -50));
		}
	}

	public void  WorldThreeStart (GameObject LVL)
	{
		if (LVL.name == "1") {
			StartCoroutine (HideUnwantedThings (3, 1, 0));
		} else if (LVL.name == "2") {
			StartCoroutine (HideUnwantedThings (3, 2, -10));
		} else if (LVL.name == "3") {
			StartCoroutine (HideUnwantedThings (3, 3, -20));
		} else if (LVL.name == "4") {
			StartCoroutine (HideUnwantedThings (3, 4, -30));
		} else if (LVL.name == "5") {
			StartCoroutine (HideUnwantedThings (3, 5, -40));
		} else if (LVL.name == "6") {
			StartCoroutine (HideUnwantedThings (3, 6, -50));
		} else if (LVL.name == "7") {
			StartCoroutine (HideUnwantedThings (3, 7, -60));
		} else if (LVL.name == "8") {
			StartCoroutine (HideUnwantedThings (3, 8, -70));
		}
	}

	public void  WorldFourStart (GameObject LVL)
	{
		if (LVL.name == "1") {
			StartCoroutine (HideUnwantedThings (4, 1, 0));
		} else if (LVL.name == "2") {
			StartCoroutine (HideUnwantedThings (4, 2, -10));
		} else if (LVL.name == "3") {
			StartCoroutine (HideUnwantedThings (4, 3, -20));
		} else if (LVL.name == "4") {
			StartCoroutine (HideUnwantedThings (4, 4, -30));
		} else if (LVL.name == "5") {
			StartCoroutine (HideUnwantedThings (4, 5, -40));
		} else if (LVL.name == "6") {
			StartCoroutine (HideUnwantedThings (4, 6, -50));
		} else if (LVL.name == "7") {
			StartCoroutine (HideUnwantedThings (4, 7, -60));
		} else if (LVL.name == "8") {
			StartCoroutine (HideUnwantedThings (4, 8, -70));
		}
	}

	public void  WorldFiveStart (GameObject LVL)
	{


		if (LVL.name == "1") {
			StartCoroutine (HideUnwantedThings (5, 1, 0));
		} else if (LVL.name == "2") {
			StartCoroutine (HideUnwantedThings (5, 2, -10));
		} else if (LVL.name == "3") {
			StartCoroutine (HideUnwantedThings (5, 3, -20));
		} else if (LVL.name == "4") {
			StartCoroutine (HideUnwantedThings (5, 4, -30));
		} else if (LVL.name == "5") {
			StartCoroutine (HideUnwantedThings (5, 5, -40));
		} else if (LVL.name == "6") {
			StartCoroutine (HideUnwantedThings (5, 6, -50));
		} else if (LVL.name == "7") {
			StartCoroutine (HideUnwantedThings (5, 7, -60));
		} else if (LVL.name == "8") {
			StartCoroutine (HideUnwantedThings (5, 8, -70));
		} else if (LVL.name == "9") {
			StartCoroutine (HideUnwantedThings (5, 9, -80));
		}
	}

	public void  WorldSixStart (GameObject LVL)
	{
		if (LVL.name == "1") {
			StartCoroutine (HideUnwantedThings (6, 1, 0));
		} else if (LVL.name == "2") {
			StartCoroutine (HideUnwantedThings (6, 2, -10));
		} else if (LVL.name == "3") {
			StartCoroutine (HideUnwantedThings (6, 3, -20));
		} else if (LVL.name == "4") {
			StartCoroutine (HideUnwantedThings (6, 4, -30));
		} else if (LVL.name == "5") {
			StartCoroutine (HideUnwantedThings (6, 5, -40));
		} else if (LVL.name == "6") {
			StartCoroutine (HideUnwantedThings (6, 6, -50));
		} else if (LVL.name == "7") {
			StartCoroutine (HideUnwantedThings (6, 7, -60));
		} else if (LVL.name == "8") {
			StartCoroutine (HideUnwantedThings (6, 8, -70));
		} else if (LVL.name == "9") {
			StartCoroutine (HideUnwantedThings (6, 9, -80));
		} else if (LVL.name == "10") {
			StartCoroutine (HideUnwantedThings (6, 10, -90));
		}
	}

	// Change it to enumerator to make animations
	IEnumerator HideUnwantedThings (int WorldNo, int LevelNo, int LevelPos)
	{
		PlayButtonClickSnd ();
		if (GameController.instance.Worlds [WorldNo - 1] != null) {
			GameController.instance.Worlds [WorldNo - 1].GetComponent<Animator> ().SetBool ("world", false);
		}

		GameController.instance.SlideButtons.gameObject.SetActive (false);
		yield return new WaitForSeconds (0.3f);
		GameController.instance.FadeScreen.gameObject.SetActive (true);
		GameController.instance.FadeScreen.GetComponent<Animator> ().SetBool ("fade", true);
		yield return new WaitForSeconds (0.8f);
		SceneManager.LoadSceneAsync (WorldNo);
		GameController.instance.SetWorldNLevelNum (WorldNo, LevelNo, LevelPos);
		GameController.Rots = 0;
	}

	public void ResetGame ()
	{
		PlayButtonClickSnd ();
		if (!GameController.PlayerPositionToggle) {
			Transform plr = GameObject.Find ("Player" + GameController.instance.LevelNum).transform;
			GameController.PlayerPositionToggle = true;
			GameController.Rots = 0;
			GameController.instance.SetRotsTxt (0);

			GameController.instance.Ghost.enabled = false;
			PlayerController ppp = plr.GetComponent<PlayerController> ();
			ppp.ResetAfterReset ();
			ppp.CallRemoveBlocksOneByOne (0.03f);

			GameController.instance.DeactivatePlayerEff ();
			// Count the number of AdSteps needed to show the interestitial ad
			UIManagement.instance.IncrementAdStepCount ();
		}
	}

	public void FailureResetGame ()
	{
		if (!GameController.PlayerPositionToggle) {
			Transform plr = GameObject.Find ("Player" + GameController.instance.LevelNum).transform;
			GameController.PlayerPositionToggle = true;
			GameController.Rots = 0;
			GameController.instance.SetRotsTxt (0);

			GameController.instance.Ghost.enabled = false;
			PlayerController ppp = plr.GetComponent<PlayerController> ();
			ppp.ResetAfterReset ();
			ppp.CallRemoveBlocksOneByOne (0.03f);
			GameController.instance.DeactivatePlayerEff ();
		}
	}

	public void GetTacticsBack (PlayerController ppp)
	{
		for (int i = ppp.Tactics.Count - 1; i >= 0; i--) {
			GameObject g = ppp.Tactics [i];
			g.SetActive (true);
		}
	}

	

	public void Hints ()
	{
		PlayButtonClickSnd ();
		int t = GameController.instance.GetHints ();
		Transform g = GameObject.Find ("Hints" + GameController.instance.LevelNum).transform;
           

		if (t > 0 && g.position.y == 15) {
            
			g.position = Vector3.zero;
			t--;
		} else if (t > 0 && g.position.y == 0) {
			g.position = new Vector3 (0f, 15f, 0f);
		} else {
			ShowHintsPurchaseOption ();
		}
		GameController.instance.SetHints (t);
		GameController.instance.SetHintsTxt (t);
		PlayerPrefController.instance.SetHintsKey (t);
	}

	void ShowHintsPurchaseOption ()
	{
		GameController.instance.SetRemainingHintsTxt (GameController.instance.Hints);
		GameController.instance.IAPScreen.gameObject.SetActive (true);
		GameController.instance.IAPScreen.GetComponent<Animator> ().SetBool ("purchase", true);
		GameController.instance.BackKey = "IAPScreen";
		GameController.instance.LetEscapeGo ();

	}

	public void ExitPurchase ()
	{
		PlayButtonClickSnd ();
		GameController.instance.IAPScreen.GetComponent<Animator> ().SetBool ("purchase", false);
		GameController.instance.BackKey = "OnGameplay";
	}

	// Check which world ui can go when back button pressed
	public void BackBtn ()
	{
		PlayButtonClickSnd ();
		GameController.instance.InLevelUI.GetComponent<Animator> ().SetBool ("levelui", false);
		GameController.instance.LevelsObj.SetActive ((false));
		GameController.instance.Worlds [GameController.instance.WorldNum - 1].GetComponent<Animator> ().SetBool ("world", true);
		Invoke ("GetSlider", 0.8f);
		GameController.instance.BackKey = "WorldSelect";
		GameController.instance.LetEscapeGo ();
		GameController.instance.DeactivatePlayerEff ();
	}

	void GetSlider ()
	{
		GameController.instance.SlideButtons.gameObject.SetActive (true);
	}

	public void SliderControlRight ()
	{
		int t = GameController.instance.Slider;

		if (t >= 0 && t < GameController.instance.TotalWorlds - 1) {
			PlayButtonClickSnd ();
			StartCoroutine (GetNextWorld (t, true));
			GameController.instance.Slider++;
			GameController.instance.OfflineEntryWorld++;
		}
	}

	public void SliderControlLeft ()
	{
		int t = GameController.instance.Slider;
		if (t > 0 && t < GameController.instance.TotalWorlds) {
			PlayButtonClickSnd ();
			StartCoroutine (GetNextWorld (t, false));
			GameController.instance.Slider--;
			GameController.instance.OfflineEntryWorld--;
		}
	}

	IEnumerator GetNextWorld (int t, bool b)
	{
		yield return new WaitForSeconds (0.1f);
		if (b) {
			GameController.instance.Worlds [t].GetComponent<Animator> ().SetBool ("world", false);
			yield return new WaitForSeconds (0.05f);
			GameController.instance.Worlds [t + 1].gameObject.SetActive (true);
			GameController.instance.Worlds [t + 1].GetComponent<Animator> ().SetBool ("world", true);
		
		} else {
			GameController.instance.Worlds [t].GetComponent<Animator> ().SetBool ("world", false);
			yield return new WaitForSeconds (0.05f);
			GameController.instance.Worlds [t - 1].gameObject.SetActive (true);
			GameController.instance.Worlds [t - 1].GetComponent<Animator> ().SetBool ("world", true);
		
		}


		if (GameController.instance.TotalWorlds - 2 == t) {
//			GameController.instance.SlideButtons.GetChild(1).GetComponent<Button>().interactable = false;
		} else if (GameController.instance.TotalWorlds - 6 == t + 1) {
//			GameController.instance.SlideButtons.GetChild(0).GetComponent<Button>().interactable = false;
		} else {
			GameController.instance.SlideButtons.GetChild (0).GetComponent<Button> ().interactable = true;
			GameController.instance.SlideButtons.GetChild (1).GetComponent<Button> ().interactable = true;
		}
	}

	void OnLevelWasLoaded (int l)
	{
		try {
			if (UIManagement.instance.BGClips [l - 1] != null) {
				UIManagement.instance.MusicSrc.clip = UIManagement.instance.BGClips [l - 1];
				UIManagement.instance.MusicSrc.Play ();
			}
		} catch (System.Exception ex) {

		}
	}


	public void ShowVideoAd ()
	{
		PlayButtonClickSnd ();
		HeyZapAdsController.AdChecker = true;
		HeyZapAdsController.instance.ShowIncentivezedAds ();
	}

	//	public void ShowBannerAdUI ()
	//	{
	//		AdmobControl.instance.ShowBannerAd ();
	//	}

	//	public void HideBannerAdUI ()
	//	{
	//		AdmobControl.instance.HideBannerAd ();
	//	}

	void ShowInterstitialAd ()
	{
		HeyZapAdsController.instance.ShowInterstitialAd ();
	}

	public void PurchaseHints_5 ()
	{
		PlayButtonClickSnd ();
		//IAPController.instance.BuyHints5 ();
	}

	public void PurchaseHints_10 ()
	{
		PlayButtonClickSnd ();
		//IAPController.instance.BuyHints10 ();
	}

	public void PurchaseHints_15 ()
	{
		PlayButtonClickSnd ();
		//IAPController.instance.BuyHints15 ();
	}

	public void GetInstructions ()
	{
		PlayButtonClickSnd ();
		try {
			iTween.MoveTo (InstructionConroller.instance.Instructions.gameObject, iTween.Hash ("x", 0, "time", 0.5f, "easetype", iTween.EaseType.easeOutElastic));
			iTween.MoveTo (InstructionConroller.instance.Instructions.parent.GetChild (0).gameObject, iTween.Hash ("x", 0, "time", 0.5f, "easetype", iTween.EaseType.easeOutElastic));
		} catch (System.Exception ex) {
			UnityEngine.Debug.Log (ex);
		}
		Invoke ("LetGo", 0.5f);
		InstructionConroller.instance.ResetThings ();
	}

	void LetGo ()
	{
		InstructionConroller.instance.InsControl = true;
	}

	public void GameResetBtn ()
	{
		PlayButtonClickSnd ();
		GameController.instance.ResetWindow.GetComponent<Animator> ().SetBool ("gamereset", true);

	}

	public void ResetYes ()
	{
		PlayButtonClickSnd ();
		PlayerPrefController.instance.DeleteAllPref ();
		GameController.instance.ResetWindow.GetComponent<Animator> ().SetBool ("gamereset", false);
		Application.Quit ();
	}

	public void ResetNo ()
	{
		PlayButtonClickSnd ();
		GameController.instance.ResetWindow.GetComponent<Animator> ().SetBool ("gamereset", false);
	}

	public void MailSupport ()
	{
		PlayButtonClickSnd ();
		SendEmail ();
	}

	public void RateIt ()
	{
		PlayButtonClickSnd ();
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.hyperzeta.colotor");
	}

	void SendEmail ()
	{
		string email = "support@hyperzeta.com";
		string subject = MyEscapeURL ("Need help for the game Colotor");
		string body = MyEscapeURL ("");
		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}

	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL (url).Replace ("+", "%20");
	}

	// Counts number of steps to shwo the interstitial ad when AdStepCount reaches its threshold
	public void IncrementAdStepCount ()
	{
		AdStepCount++;
		if (AdStepCount == 7) {
			AdStepCount = 0;
			Invoke ("ShowIntersititalAdAfter", 1f);
		}
	}

	void ShowIntersititalAdAfter ()
	{
		ShowInterstitialAd ();
	}

	// Count number of steps user played to show the Rate it screen
	public void IncrementRateItCount ()
	{
		RateItStepCount++;
		if (RateItStepCount == 10) {
			InstructionConroller.instance.ShowRateitScreen ();
			RateItStepCount = 0;
		}
	}

	public void RateLater ()
	{
		UIManagement.instance.PlayButtonClickSnd ();
		InstructionConroller.instance.HideRateitScreen ();
		PlayerPrefController.instance.SetRateIt (2);
	}

	public void RateNo ()
	{
		UIManagement.instance.PlayButtonClickSnd ();
		InstructionConroller.instance.HideRateitScreen ();
		PlayerPrefController.instance.SetRateIt (3);
	}

	public void RateNow ()
	{
		UIManagement.instance.PlayButtonClickSnd ();
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.hyperzeta.colotor");
		InstructionConroller.instance.HideRateitScreen ();
		PlayerPrefController.instance.SetRateIt (1);
	}

	public void GetInRewardCompleteScreen ()
	{
		GameController.instance.IAPScreen.GetComponent<Animator> ().SetBool ("purchase", false);
		GameController.instance.BackKey = "RCS";
		GameController.instance.RewardCompleteScreen.GetComponent<Animator> ().SetBool ("rewardcomplete", true);
	}

	public void GetBackRewardScreenBtn ()
	{
		PlayButtonClickSnd ();
		GetBackRewardCompleteScreen ();
	}

	public void GetBackRewardCompleteScreen ()
	{
		GameController.instance.BackKey = "OnGameplay";
		GameController.instance.RewardCompleteScreen.GetComponent<Animator> ().SetBool ("rewardcomplete", false);
	}
}
