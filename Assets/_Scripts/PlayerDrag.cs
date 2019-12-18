﻿using UnityEngine;
using System.Collections;

public class PlayerDrag : MonoBehaviour
{
	//    PlayerController Script from Player
	private PlayerController PlayerControl;

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		PlayerControl = GameController.instance.CurrentPlayer;
		if (gameObject.name == "1" && col.gameObject.name == "11")
		{
			PlayerControl.SetIsItRight(true);
		}
		else if (gameObject.name == "2" && col.gameObject.name == "22")
		{
			PlayerControl.SetIsItRight(true);
		}
		else if (gameObject.name == "3" && col.gameObject.name == "33")
		{
			PlayerControl.SetIsItRight(true);
		}
		else if (gameObject.name == "4" && col.gameObject.name == "44")
		{
			PlayerControl.SetIsItRight(true);
		}

	}

	void OnTriggerExit2D(Collider2D col)
	{
		PlayerControl = GameController.instance.CurrentPlayer;

		if (gameObject.name == "1" && col.gameObject.name == "11")
		{
			StartCoroutine("SetExitVal");
		}
		else if (gameObject.name == "2" && col.gameObject.name == "22")
		{
			StartCoroutine("SetExitVal");
		}
		else if (gameObject.name == "3" && col.gameObject.name == "33")
		{
			StartCoroutine("SetExitVal");
		}
		else if (gameObject.name == "4" && col.gameObject.name == "44")
		{
			StartCoroutine("SetExitVal");
		}
	}

	IEnumerator SetExitVal()
	{
		yield return new WaitForSeconds(0.1f);
		PlayerControl.SetIsItRight(false);
	}
	
}
