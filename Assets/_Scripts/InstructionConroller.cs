using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class InstructionConroller : MonoBehaviour
{

	public static InstructionConroller instance;

	public GameObject[] LevelInsList;
	public Transform RateItScreen;
	private int NoOfIns = 4;
	private bool InsBtnBool = true;
	public Transform Instructions;
	// Check if the instruction are visible if so then make it true so it will help to hide it later
	[HideInInspector]
	public bool InsControl = false;
	public static bool LevelInsControl = false, RateitControl = false;
	private RaycastHit2D rayHit;

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
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		rayHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, 10f);
		if (Input.GetMouseButtonDown (0) && rayHit.collider != null) {
			if (rayHit.collider.name == transform.name) {
				ChangeIns ();
				GetComponent<Collider2D> ().enabled = false; 
				Invoke ("MovingComplete", 0.5f);
				UIManagement.instance.PlayButtonClickSnd ();
			}
		} else if (Input.GetMouseButtonDown (0) && InsControl) {
			InsControl = false;
			iTween.MoveTo (Instructions.gameObject, iTween.Hash ("x", 10, "time", 0.5f));
			iTween.MoveTo (Instructions.parent.GetChild (0).gameObject, iTween.Hash ("x", 10, "time", 0.5f));			
		}
		if (LevelInsControl && Input.GetMouseButtonDown (0)) {
			LevelInsControl = false;
			for (int i = 0; i < LevelInsList.Length; i++) {
				LevelInsList [i].SetActive (false);
			}
		}

		if (RateitControl && Input.GetMouseButtonDown (0)) {
			RateitControl = false;
			InstructionConroller.instance.HideRateitScreen ();
		}

	}

	public void ChangeIns ()
	{
		float f = transform.localScale.x;
		if (InsBtnBool) {
			NoOfIns--;
			iTween.MoveTo (Instructions.gameObject, iTween.Hash ("x", Instructions.position.x - 8, "time", 0.5f));
			if (NoOfIns == 0) {
				InsBtnBool = false;

				f = -transform.localScale.x;
				transform.localScale = new Vector2 (f, transform.localScale.y);
			}
		} else if (!InsBtnBool) {
			NoOfIns++;
			iTween.MoveTo (Instructions.gameObject, iTween.Hash ("x", Instructions.position.x + 8, "time", 0.5f));

			if (NoOfIns == 4) {
				InsBtnBool = true;
				f = -transform.localScale.x;
				transform.localScale = new Vector2 (f, transform.localScale.y);
			}
		}
	}

	void MovingComplete ()
	{
		GetComponent<Collider2D> ().enabled = true;
	}

	public void ResetThings ()
	{
		float a = transform.localScale.x, b = transform.localScale.y;
		if (a > 0) {
			transform.localScale = new Vector2 (-a, b);
		}
		NoOfIns = 4;
		InsBtnBool = true;
		InsControl = false;
	}

	// This function executes everytime new level is loaded
	public void ShowInstruction ()
	{
		int w = GameController.instance.WorldNum, l = GameController.instance.LevelNum;

		if (!LevelInsControl) {
			if (w == 1 && l == 1 && PlayerPrefController.instance.GetIns1 () == 0) {
				LevelInsList [0].SetActive (true);
				LevelInsControl = true;
				PlayerPrefController.instance.SetIns1 (1);
			} else if (w == 2 && l == 2 && PlayerPrefController.instance.GetIns2 () == 0) {
				LevelInsList [1].SetActive (true);
				LevelInsControl = true;
				PlayerPrefController.instance.SetIns2 (1);
			} else if (w == 2 && l == 6 && PlayerPrefController.instance.GetIns3 () == 0) {
				LevelInsList [2].SetActive (true);
				LevelInsControl = true;
				PlayerPrefController.instance.SetIns3 (1);
			} else if (w == 5 && l == 1 && PlayerPrefController.instance.GetIns4 () == 0) {
				LevelInsList [3].SetActive (true);
				LevelInsControl = true;
				PlayerPrefController.instance.SetIns4 (1);
			} else if (w == 6 && l == 1 && PlayerPrefController.instance.GetIns5 () == 0) {
				LevelInsList [4].SetActive (true);
				LevelInsControl = true;
				PlayerPrefController.instance.SetIns5 (1);
			}
			GameController.instance.FadeScreen.gameObject.SetActive (false);
		}

		GameController.CanShowPlayerEff = true;
		Invoke ("GetPlayerEff", 2f);

// Count the number of AdSteps needed to show the interestitial ad
		UIManagement.instance.IncrementAdStepCount ();
// Count the number of steps needed to show the RateIt screen
		UIManagement.instance.IncrementRateItCount ();
// Count how long player playing to finish the level to show the hints animation
		StartCoroutine ("WaitAndShowHintsAnim");

	}

	void GetPlayerEff ()
	{
		GameController.instance.ActivatePlayerEff ();
	}

	public void ShowRateitScreen ()
	{
		if (PlayerPrefController.instance.GetRateIt () == 0 || PlayerPrefController.instance.GetRateIt () == 2) {
			RateItScreen.GetComponent<Animator> ().SetBool ("rateit", true);
			RateitControl = true;
		}
	}

	public void HideRateitScreen ()
	{
		RateItScreen.GetComponent<Animator> ().SetBool ("rateit", false);
	}

	public void StopHintsAnimCoroutine ()
	{
		StopCoroutine ("WaitAndShowHintsAnim");
	}

	IEnumerator WaitAndShowHintsAnim ()
	{
		yield return new WaitForSeconds (20f);
		GameController.instance.HintsTxtAnim.SetBool ("hintsanim", true);
	}
}