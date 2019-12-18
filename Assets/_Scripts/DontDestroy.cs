using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour
{
	public static DontDestroy instanceDestroy;

	// Use this for initialization
	void Start ()
	{
		if (instanceDestroy != null) {
			Destroy (gameObject);
		} else {
			instanceDestroy = this;
			DontDestroyOnLoad (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
