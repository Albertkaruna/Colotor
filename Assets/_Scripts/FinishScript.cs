using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Security.Policy;

public class FinishScript : MonoBehaviour
{


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			// If player hits the end object then call StopMove()
			StopMove ();
			if (GameController.instance.CurrentPlayer.GetIsItRight () == true) {
				//	Code goes here to decide the win or loss
				GameController.instance.StoreBestValue ();
				StartCoroutine (GetNextLevel ());
				GameController.instance.UnlockNextLevel ();
				StartCoroutine (WaitAndDo (1));
				UIManagement.instance.PlayVictorySnd ();
				col.gameObject.SetActive (false);

				InstructionConroller.instance.StopHintsAnimCoroutine ();
			} else {
				StartCoroutine (WaitAndDo (1));
// Reset the game without restarting
				UIManagement.instance.FailureResetGame ();
				UIManagement.instance.PlayFailureSnd ();
				// Count the number of AdSteps needed to show the interestitial ad
				UIManagement.instance.IncrementAdStepCount ();
			}
		}

		GameController.instance.DeactivatePlayerEff ();
	}

	IEnumerator WaitAndDo (int v)
	{
		yield return new WaitForSeconds (1.5f);
		GameController.PlayerPositionToggle = true;
//		GameController.instance.CurrentPlayer.CanMove = true;
	}

	// Stop the player movement
	void StopMove ()
	{
		GameController.instance.CurrentPlayer.CanMove = false;
		GameController.instance.Ghost.enabled = false;
	}

	IEnumerator GetNextLevel ()
	{
		yield return new WaitForSeconds (0.3f);
		if (!CheckWorldNLevel ()) {
			GameController.instance.LevelNum++;
			yield return new WaitForSeconds (0.1f);
			iTween.ScaleTo (GameController.instance.LevelsObj.transform.GetChild (GameController.instance.LevelNum - 1).gameObject, iTween.Hash ("scale", new Vector3 (0.8f, 0.8f, 0.8f), "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic));      
			yield return new WaitForSeconds (0.3f);
			float NewPos = GameController.instance.LevelsObj.transform.position.x - 10;
			iTween.MoveTo (GameController.instance.LevelsObj, iTween.Hash ("x", NewPos, "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic));
			yield return new WaitForSeconds (0.3f);
			GameController.instance.SetCanValue (GameController.instance.WorldNum);
			iTween.ScaleTo (GameController.instance.LevelsObj.transform.GetChild (GameController.instance.LevelNum - 1).gameObject, iTween.Hash ("scale", Vector3.one, "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic));      
			yield return new WaitForSeconds (0.3f);
			InstructionConroller.instance.ShowInstruction ();
//			GameCont roller.instance.CurrentPlayer.CanMove = true;
		} else {
			GameController.instance.InLevelUI.gameObject.SetActive ((false));
            
			GameController.instance.Worlds [GameController.instance.WorldNum - 1].gameObject.SetActive (false);
                        
			GameController.instance.Worlds [GameController.instance.WorldNum].gameObject.SetActive (true);
			StartCoroutine (LoadNewWorldUI ());
		}
	}

	IEnumerator LoadNewWorldUI ()
	{
		yield return new WaitForSeconds (0.5f);

		try {
			Transform t = GameController.instance.Worlds [GameController.instance.WorldNum].GetChild (GameController.instance.LevelNum).transform;
			t.GetComponent<Button> ().interactable = true;
			t.GetChild (0).gameObject.SetActive (true);
			t.GetChild (1).gameObject.SetActive (false);
			int a = GameController.instance.WorldNum + 1, b = GameController.instance.LevelNum - 1;
			PlayerPrefController.instance.SetEntryWorldKey (GameController.instance.WorldNum);
			GameController.instance.Slider = PlayerPrefController.instance.GetEntryWorldKey ();
			PlayerPrefController.instance.SetLocktKey (a + "" + b + 1, 1);
		} catch (System.Exception ex) {
			Debug.Log (ex);
		}

		iTween.ScaleTo (GameController.instance.LevelsObj.transform.GetChild (GameController.instance.LevelNum).gameObject, iTween.Hash ("scale", new Vector3 (0.8f, 0.8f, 0.8f), "time", 0.5f, "easetype", iTween.EaseType.easeOutElastic));  
		yield return new WaitForSeconds (0.5f);
		iTween.MoveTo (GameController.instance.LevelsObj, iTween.Hash ("x", GameController.instance.LevelsObj.transform.position.x - 10, "time", 0.5f, "easetype", iTween.EaseType.easeOutElastic));
		yield return new WaitForSeconds (1f);
		GameController.instance.Worlds [GameController.instance.WorldNum].GetComponent<Animator> ().SetBool ("world", true);
		yield return new WaitForSeconds (0.2f);
		GameController.instance.SlideButtons.gameObject.SetActive (true);
		GameController.instance.LevelsObj.SetActive (false);
	}

	// Checks all the levels are played if so then let to load new world
	bool CheckWorldNLevel ()
	{
		if (GameController.instance.WorldNum == 1 && GameController.instance.LevelNum == 5) { // 5
			GameController.instance.LevelNum = 1;
			return true;
		} else if (GameController.instance.WorldNum == 2 && GameController.instance.LevelNum == 6) { // 6
			GameController.instance.LevelNum = 1;
			return true;
		} else if (GameController.instance.WorldNum == 3 && GameController.instance.LevelNum == 8) { // 8
			GameController.instance.LevelNum = 1;
			return true;
		} else if (GameController.instance.WorldNum == 4 && GameController.instance.LevelNum == 8) { // 8
			GameController.instance.LevelNum = 1;
			return true;
		} else if (GameController.instance.WorldNum == 5 && GameController.instance.LevelNum == 9) { // 9
			GameController.instance.LevelNum = 1;
			return true;
		} else if (GameController.instance.WorldNum == 6 && GameController.instance.LevelNum == 10) {// 10
			GameController.instance.LevelNum = 1;
			return true;
		} else {
			return false;
		}
	}
}

