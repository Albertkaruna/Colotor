using UnityEngine;
using System.Collections;

public class RateIt : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnMouseDown ()
	{
		UIManagement.instance.PlayButtonClickSnd ();
		if (gameObject.name == "Rate") {
			Application.OpenURL ("https://play.google.com/store/apps/details?id=com.hyperzeta.colotor");
			InstructionConroller.instance.HideRateitScreen ();
			PlayerPrefController.instance.SetRateIt (1);
		} else if (gameObject.name == "Later") {
			InstructionConroller.instance.HideRateitScreen ();
			PlayerPrefController.instance.SetRateIt (2);
		} else if (gameObject.name == "No") {
			InstructionConroller.instance.HideRateitScreen ();
			PlayerPrefController.instance.SetRateIt (3);
		}
	}
}
