using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void PlayButton ()
	{
		StartCoroutine (WaitAndDo ());
	}

	IEnumerator WaitAndDo ()
	{
		int entry = PlayerPrefController.instance.GetEntryWorldKey ();
		yield return new WaitForSeconds (0.1f);
		GameController.instance.Worlds [entry].gameObject.SetActive (true);
		yield return new WaitForSeconds (0.2f);
	
		GameController.instance.Worlds [entry].GetComponent<Animator> ().SetBool ("world", true);
		GameController.instance.SlideButtons.gameObject.SetActive (true);
	}


	public void GetBackPlayButton ()
	{
		GameController.instance.PlayBtn.gameObject.SetActive (true);
	}

	void FadeInEnd ()
	{
		GameController.instance.FadeScreen.GetComponent<Animator> ().SetBool ("fade", false);
	}

	void FadeOutEnd ()
	{
		StartCoroutine (NewWorld ());
	}

	IEnumerator NewWorld ()
	{
//		yield return new WaitForSeconds (0.3f);
//		GameController.instance.LevelsObj = GameObject.FindGameObjectWithTag ("Levels");
		yield return new WaitForSeconds (0.3f);
		try {
			iTween.MoveTo (GameController.instance.LevelsObj, iTween.Hash ("x", GameController.instance.LevelPos, "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic));
		} catch (System.Exception ex) {
			Debug.Log (ex);
		}
		yield return new WaitForSeconds (0.3f);
		try {
			iTween.ScaleTo (GameController.instance.LevelsObj.transform.GetChild (GameController.instance.LevelNum - 1).gameObject, iTween.Hash ("scale", Vector3.one, "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic));
		} catch (System.Exception ex) {
			Debug.Log (ex);
		}      
		yield return new WaitForSeconds (0.2f);
		GameController.instance.InLevelUI.gameObject.SetActive (true);
		GameController.instance.SetCanValue (GameController.instance.WorldNum);
		GameController.instance.InLevelUI.GetComponent<Animator> ().SetBool ("levelui", true);
		yield return new WaitForSeconds (0.3f);
		InstructionConroller.instance.ShowInstruction ();
	}

	public void IAPScreenOut ()
	{		
		GameController.instance.IAPScreen.gameObject.SetActive (false);
	}

	public void HintsTxtAnimEnd ()
	{
		GameController.instance.HintsTxtAnim.SetBool ("hintsanim", false);
	}

}
