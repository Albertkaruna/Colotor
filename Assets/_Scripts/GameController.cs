using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using System;

/*
██╗  ██╗██╗   ██╗██████╗ ███████╗██████╗ ███████╗███████╗████████╗ █████╗ 
██║  ██║╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗╚══███╔╝██╔════╝╚══██╔══╝██╔══██╗
███████║ ╚████╔╝ ██████╔╝█████╗  ██████╔╝  ███╔╝ █████╗     ██║   ███████║
██╔══██║  ╚██╔╝  ██╔═══╝ ██╔══╝  ██╔══██╗ ███╔╝  ██╔══╝     ██║   ██╔══██║
██║  ██║   ██║   ██║     ███████╗██║  ██║███████╗███████╗   ██║   ██║  ██║
╚═╝  ╚═╝   ╚═╝   ╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝
*/




public class GameController : MonoBehaviour
{
	//    Game conroller to handle the UI data
	public static GameController instance;
	public Transform HyperZetaLogo;
	public Transform PlayBtnStuff, SettingBtn, SettingPanel, FadeScreen, InLevelUI, HintsBtn, SlideButtons, IAPScreen, PlayBtn, ResetWindow, ExitBtn;
	public Transform RewardCompleteScreen;
	public GameObject VictoryEff, FailEff, PickEff;
	// Display texts of can,rots and best
	[SerializeField]
	private Text CanTxt, RotsTxt, BestTxt, HintsTxt, RemainingHintsTxt;
	// It holds all the transform of the world
	public Transform[] Worlds;
	// Variables to store Can and Best value
	private int Can, Best;
	// Rots variable, Global Reference for current world and level
	[HideInInspector]
	public int WorldNum, LevelNum, Hints, LevelPos, Slider, TotalWorlds = 6, OfflineEntryWorld;
	public static int Rots;
	// Global Reference to the levels holder in each world
	[HideInInspector]
	public GameObject LevelsObj;
	// Register all best keys once in a life time
	private bool IsItFirst = true;
	// Arrays for each and every worlds to store the can value
	private int[] World1, World2, World3, World4, World5, World6;
	[HideInInspector]
	public SpriteRenderer Ghost;
	//	 Player transform to store it's initial transform
	[HideInInspector]
	public Vector3 PlayerPosition;
	[HideInInspector]
	public Quaternion PlayerRotation;
	[HideInInspector]
	public PlayerController CurrentPlayer;
	public static bool PlayerPositionToggle = true;
	// Back button control
	[HideInInspector]
	public string BackKey = "Exit";
	[HideInInspector]
	// Hints text animator, used to show the text animation to tell the player can use the hints to complete the level
	public Animator HintsTxtAnim;
	[HideInInspector]
	public Transform PlayerEff;
	public static bool CanShowPlayerEff = true, LetEscapeWork = true;

	void Awake ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		World1 = new int[5]{ 6, 5, 6, 4, 5 };
		World2 = new int[6]{ 6, 4, 7, 9, 6, 12 };
		World3 = new int[8]{ 10, 10, 11, 13, 11, 8, 11, 10 };
		World4 = new int[8]{ 12, 16, 10, 7, 12, 14, 14, 18 };
		World5 = new int[9]{ 28, 10, 8, 16, 16, 14, 10, 14, 18 };
		World6 = new int[10]{ 33, 20, 30, 19, 34, 31, 18, 41, 12, 28 };

	}

	// Use this for initialization
	void Start ()
	{

// Giving the initial hints only once in a lifetime
		if (PlayerPrefController.instance.GetIsItFirsttKey () == 0) {
			PlayerPrefController.instance.SetIsItFirstKey ();
			PlayerPrefController.instance.SetHintsKey (5);
		}

		InitializeBest ();
		CurrencyControl ();
		Slider = PlayerPrefController.instance.GetEntryWorldKey ();
		OfflineEntryWorld = PlayerPrefController.instance.GetEntryWorldKey ();
		HintsTxtAnim = HintsTxt.gameObject.GetComponent<Animator> ();

		Invoke ("HideLogo", 3f);
	}

	void HideLogo ()
	{
		HyperZetaLogo.gameObject.SetActive (false);
		UIManagement.instance.MusicSrc.Play ();
		GameController.instance.PlayBtnStuff.GetComponent<Animator> ().SetBool ("play", true);
	}

	public void LetEscapeGo ()
	{
		LetEscapeWork = false;
		StartCoroutine (EscapeGoNow ());
	}

	IEnumerator EscapeGoNow ()
	{
		yield return new WaitForSeconds (2.5f);
		LetEscapeWork = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && LetEscapeWork) {
			LetEscapeWork = false;
			LetEscapeGo ();
			if (BackKey == "Exit") {
				Application.Quit ();
			} else if (BackKey == "Settings") {
				UIManagement.instance.Settings ();
			} else if (BackKey == "WorldSelect") {
				StartCoroutine (BackToPlay ());
			} else if (BackKey == "OnGameplay") {
				UIManagement.instance.BackBtn ();
			} else if (BackKey == "IAPScreen") {
				UIManagement.instance.ExitPurchase ();
			} else if (BackKey == "RCS") {
				UIManagement.instance.GetBackRewardCompleteScreen ();
			}

			 
			if (InstructionConroller.instance.InsControl) {
				try {
					iTween.MoveTo (InstructionConroller.instance.Instructions.gameObject, iTween.Hash ("x", 10, "time", 0.5f));
					iTween.MoveTo (InstructionConroller.instance.Instructions.parent.GetChild (0).gameObject, iTween.Hash ("x", 10, "time", 0.5f));			
					InstructionConroller.instance.InsControl = false;
				} catch (Exception ex) {
					UnityEngine.Debug.Log (ex);
				}
			}
			if (InstructionConroller.RateitControl) {
				InstructionConroller.RateitControl = false;
				InstructionConroller.instance.HideRateitScreen ();
			}
			if (InstructionConroller.LevelInsControl) {
				InstructionConroller.LevelInsControl = false;
				for (int i = 0; i < InstructionConroller.instance.LevelInsList.Length; i++) {
					InstructionConroller.instance.LevelInsList [i].SetActive (false);
				}
			}
		}
	}

	public void BackToPlayBtnCall ()
	{
		StartCoroutine (BackToPlay ());
		if (BackKey == "Exit") {
			Application.Quit ();
		}
	}

	IEnumerator BackToPlay ()
	{
		GameController.instance.Worlds [OfflineEntryWorld].GetComponent<Animator> ().SetBool ("world", false);
		yield return new WaitForSeconds (0.1f);
		SceneManager.LoadSceneAsync (0);
		GameController.instance.SlideButtons.gameObject.SetActive (false);
		yield return new WaitForSeconds (0.3f);
		SettingBtn.gameObject.SetActive (true);
		PlayBtnStuff.GetComponent<Animator> ().SetBool ("play", true);
		BackKey = "Exit";
		ExitBtn.gameObject.SetActive (true);
	}

	public void SetCan (int Can)
	{
		this.Can = Can;
	}

	public int GetCan ()
	{
		return this.Can;
	}


	public void SetBest ()
	{
		this.Best = Best;
	}

	public int GetBest ()
	{
		return this.Best;
	}


	public void SetCanTxt (int CanTxt)
	{
		this.CanTxt.text = "Can: " + CanTxt.ToString ();
	}

	public string GetCanTxt ()
	{
		return this.CanTxt.text.ToString ();
	}

	public void SetRotsTxt (int RotsTxt)
	{
		this.RotsTxt.text = "Rots: " + RotsTxt.ToString ();
	}

	public string GetRotsTxt ()
	{
		return this.RotsTxt.text.ToString ();
	}

	public void SetBestTxt (int BestTxt)
	{
		this.BestTxt.text = "Best: " + BestTxt.ToString ();
	}

	public string GetBestTxt ()
	{
		return this.BestTxt.text.ToString ();
	}

	public void SetHints (int Hints)
	{
		this.Hints = Hints;
	}

	public int GetHints ()
	{
		return this.Hints;
	}

	public void SetHintsTxt (int Hints)
	{
		this.HintsTxt.text = Hints.ToString ();
	}

	public string GetHintsTxt ()
	{
		return this.HintsTxt.text.ToString ();
	}

	public void SetRemainingHintsTxt (int Hints)
	{
		this.RemainingHintsTxt.text = Hints + " Remaining Hints";
	}


	// Initially registers the bestkey to each level
	void InitializeBest ()
	{
		int NoOfWorlds = 6;
		int[] CurrentWorld = World1;
	
		for (int i = 1; i <= NoOfWorlds; i++) {

			if (i == 1) {
				CurrentWorld = World1;
			} else if (i == 2) {
				CurrentWorld = World2;
			} else if (i == 3) {
				CurrentWorld = World3;
			} else if (i == 4) {
				CurrentWorld = World4;
			} else if (i == 5) {
				CurrentWorld = World5;
			} else if (i == 6) {
				CurrentWorld = World6;
			}
			for (int j = 0; j <= CurrentWorld.Length - 1; j++) {
//				PlayerPrefController.instance.SetBestKey(i + "" + j, 0); // Bestkey initialization
//				PlayerPrefController.instance.SetLocktKey(i + "" + j, 0); // Lock key initialization
				UnlockAllLevels (i, j + 1);
			}
		}
	}

	void UnlockAllLevels (int w, int l)
	{
		if (w == 1 && (l > 1 && l <= 5)) {
			if (PlayerPrefController.instance.GetLockKey (w + "" + l) == 1) {  
				Transform t = Worlds [w - 1].GetChild (l).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
			}
		} else if (w == 2 && (l >= 0 && l <= 6)) {
			if (PlayerPrefController.instance.GetLockKey (w + "" + l) == 1) {
				
				Transform t = Worlds [w - 1].GetChild (l).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
			}
		} else if (w == 3 && (l >= 0 && l <= 8)) {
			if (PlayerPrefController.instance.GetLockKey (w + "" + l) == 1) {
				Transform t = Worlds [w - 1].GetChild (l).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
			}
		} else if (w == 4 && (l >= 0 && l <= 8)) {
			if (PlayerPrefController.instance.GetLockKey (w + "" + l) == 1) {
				
				Transform t = Worlds [w - 1].GetChild (l).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
			}
		} else if (w == 5 && (l >= 0 && l <= 9)) {
			if (PlayerPrefController.instance.GetLockKey (w + "" + l) == 1) {
				
				Transform t = Worlds [w - 1].GetChild (l).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
			}
		} else if (w == 6 && (l >= 1 && l <= 10)) {
			if (PlayerPrefController.instance.GetLockKey (w + "" + l) == 1) {
				Transform t = Worlds [w - 1].GetChild (l).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
			}
		}
	}

	public void OnLevelWasLoaded (int n)
	{
		if (SceneManager.GetActiveScene ().buildIndex != 0) {
			LevelsObj = GameObject.FindGameObjectWithTag ("Levels");
			Ghost = GameObject.FindGameObjectWithTag ("Ghost").GetComponent<SpriteRenderer> ();
			PlayerEff = GameObject.FindGameObjectWithTag ("PlayerEff").transform;
			SetHints (PlayerPrefController.instance.GetHintsKey ());
			SetHintsTxt (GetHints ());
			WorldNum = SceneManager.GetActiveScene ().buildIndex;
			BackKey = "OnGameplay";
			GameController.instance.LetEscapeGo ();
			Hints = PlayerPrefController.instance.GetHintsKey ();
		} else if (SceneManager.GetActiveScene ().buildIndex == 0) {
			GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Canvas> ().worldCamera = Camera.main;
		}
	}

	// Setting the can value to proper level with given array value
	public void SetCanValue (int n)
	{
		int[] CurrentWorld = World1;
		float XValue = 0;
		if (LevelsObj != null) {
			XValue = LevelsObj.transform.position.x;
		}
		int ArrCount = 0;
		float XValueCompare = 0;
		if (n == 1) {
			ArrCount = World1.Length;
			CurrentWorld = World1;
		} else if (n == 2) {
			ArrCount = World2.Length;
			CurrentWorld = World2;
		} else if (n == 3) {
			ArrCount = World3.Length;
			CurrentWorld = World3;
		} else if (n == 4) {
			ArrCount = World4.Length;
			CurrentWorld = World4;
		} else if (n == 5) {
			ArrCount = World5.Length;
			CurrentWorld = World5;
		} else if (n == 6) {
			ArrCount = World6.Length;
			CurrentWorld = World6;
		}

		for (int i = 0; i < ArrCount; i++) {
			
			if (Mathf.RoundToInt (XValue) == XValueCompare) {
				SetCanTxt (CurrentWorld [i]);
				SetBestValue ();
				XValueCompare = 0;
				break;
			} else {
				XValueCompare -= 10;
			}
		}
	}

	// Setting best value to the best text
	void SetBestValue ()
	{
		Best = PlayerPrefController.instance.GetBestKey (WorldNum + "" + LevelNum);
		SetBestTxt (Best);
	}


	// Stoting the best finish value at each level
	public void StoreBestValue ()
	{
		int t = PlayerPrefController.instance.GetBestKey (WorldNum + "" + LevelNum);
		if (t == 0) {
			PlayerPrefController.instance.SetBestKey (WorldNum + "" + LevelNum, Rots);
		} else if (Rots < t) {
			PlayerPrefController.instance.SetBestKey (WorldNum + "" + LevelNum, Rots);
		}

//		LevelNum++;
		Rots = 0;
		SetRotsTxt (Rots);
	}

	public void SetWorldNLevelNum (int Worldno, int Levelno, int LevelPos)
	{
		this.WorldNum = Worldno;
		this.LevelNum = Levelno;
		this.LevelPos = LevelPos;
	}

	public void UnlockNextLevel ()
	{
		try {
			if (WorldNum == 1 && LevelNum <= 5) {
				Transform t = Worlds [WorldNum - 1].GetChild (LevelNum).transform;
				if (LevelNum != 1) {
					t.GetComponent<Button> ().interactable = true;
					t.GetChild (0).gameObject.SetActive (true);
					t.GetChild (1).gameObject.SetActive (false);
				}
				PlayerPrefController.instance.SetLocktKey (WorldNum + "" + LevelNum, 1);
			} else if (WorldNum == 2 && LevelNum <= 6) {
				Transform t = Worlds [WorldNum - 1].GetChild (LevelNum).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
				PlayerPrefController.instance.SetLocktKey (WorldNum + "" + LevelNum, 1);
			} else if (WorldNum == 3 && LevelNum <= 8) {
				Transform t = Worlds [WorldNum - 1].GetChild (LevelNum).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
				PlayerPrefController.instance.SetLocktKey (WorldNum + "" + LevelNum, 1);
			} else if (WorldNum == 4 && LevelNum <= 8) {
				Transform t = Worlds [WorldNum - 1].GetChild (LevelNum).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
				PlayerPrefController.instance.SetLocktKey (WorldNum + "" + LevelNum, 1);
			} else if (WorldNum == 5 && LevelNum <= 9) {
				Transform t = Worlds [WorldNum - 1].GetChild (LevelNum).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
				PlayerPrefController.instance.SetLocktKey (WorldNum + "" + LevelNum, 1);
			} else if (WorldNum == 6 && LevelNum <= 10) {
				Transform t = Worlds [WorldNum - 1].GetChild (LevelNum).transform;
				t.GetComponent<Button> ().interactable = true;
				t.GetChild (0).gameObject.SetActive (true);
				t.GetChild (1).gameObject.SetActive (false);
				PlayerPrefController.instance.SetLocktKey (WorldNum + "" + LevelNum, 1);
			}
		} catch (System.Exception ex) {
			Debug.Log (ex);   
		}
	}



	public void ActivatePlayerEff ()
	{
		if (CanShowPlayerEff) {
			try {
				if (BackKey == "OnGameplay") {
					PlayerEff.position = GameObject.Find ("Player" + GameController.instance.LevelNum).transform.position;
					PlayerEff.GetComponent<ParticleSystem> ().Play (true);
				}
			} catch (Exception ex) {
				Debug.Log (ex);
			}
		}	
	}

	public void DeactivatePlayerEff ()
	{
		PlayerEff.GetComponent<ParticleSystem> ().Stop (true);	
		PlayerEff.position = new Vector3 (0f, 10f, 0f);
	}

	// Change currency type based on different country
	void CurrencyControl ()
	{
		
	}
}