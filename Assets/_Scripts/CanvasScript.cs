using UnityEngine;
using System.Collections;

public class CanvasScript : MonoBehaviour
{
	private static CanvasScript CanvasInstance;

	void Awake ()
	{
		
	}

	// Use this for initialization
	void Start ()
	{
		if (CanvasInstance != null) {
			Destroy (gameObject);
		} else {
			CanvasInstance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnLevelWasLoaded (int l)
	{
		//        Assign the world's canvas to the canvas
		GetComponent<Canvas> ().worldCamera = Camera.main;
	}
}
