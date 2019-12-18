using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using System.Reflection;
using System;


public class PlayerPrefController : MonoBehaviour
{
	// All the Player Preference datas handled from here
	public static PlayerPrefController instance;
	// All the keys used through out the game to persist the values
	private const string CANKEY = "CANKEY", BESTKEY = "BESTKEY", SOUNDKEY = "SOUNDKEY", MUSICKEY = "MUSICKEY", ISITFIRSTKEY = "ISITRIGHT";
	private const string CURRENTWORLDKEY = "CURRENTWORLDKEY", HINTSKEY = "HINTSKEY", LOCKKEY = "LOCKKEY";
	private const String ENTRYWORLDKEY = "ENTRYWORLDKEY", INS1 = "INS1", INS2 = "INS2", INS3 = "INS3", INS4 = "INS4", INS5 = "INS5";
	private const string RATEIT = "RATEIT";

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
	
	}

	//	public void SetCanKey(string LevelNo, int CanValue)
	//	{
	//		PlayerPrefs.SetInt(CANKEY + LevelNo, CanValue);
	//	}
	//
	//	public int GetCanKey(int LeveoNo)
	//	{
	//		return PlayerPrefs.GetInt(CANKEY + LeveoNo);
	//	}

	public void SetBestKey (string LevelNo, int BestValue)
	{
		PlayerPrefs.SetInt (BESTKEY + LevelNo, BestValue);
	}

	public int GetBestKey (string LeveoNo)
	{
		return PlayerPrefs.GetInt (BESTKEY + LeveoNo);
	}

	public void SetSoundKey (int SoundValue)
	{
		PlayerPrefs.SetInt (SOUNDKEY, SoundValue);
	}

	public int GetSoundKey ()
	{
		return PlayerPrefs.GetInt (SOUNDKEY);
	}

	public void SetMusicKey (int MusicValue)
	{
		PlayerPrefs.SetInt (MUSICKEY, MusicValue);
	}

	public int GetMusicKey ()
	{
		return PlayerPrefs.GetInt (MUSICKEY);
	}

	public int GetIsItFirsttKey ()
	{
		return PlayerPrefs.GetInt (ISITFIRSTKEY, 0);
	}

	public void SetIsItFirstKey ()
	{
		PlayerPrefs.SetInt (ISITFIRSTKEY, 1);
	}

	public void SetCurrentWorldKey (int w)
	{
		PlayerPrefs.SetInt (CURRENTWORLDKEY, w);
	}

	public int GetCurrentWorldKey ()
	{
		return PlayerPrefs.GetInt (CURRENTWORLDKEY);
	}

	public void SetHintsKey (int hints)
	{
		PlayerPrefs.SetInt (HINTSKEY, hints);
	}

	public int GetHintsKey ()
	{
		return PlayerPrefs.GetInt (HINTSKEY, 0);
	}

	public void SetLocktKey (string LevelNo, int lockValue)
	{
		PlayerPrefs.SetInt (LOCKKEY + LevelNo, lockValue);
	}

	public int GetLockKey (string LevelNo)
	{
		return PlayerPrefs.GetInt (LOCKKEY + LevelNo, 0);
	}

	public void SetEntryWorldKey (int w)
	{
		PlayerPrefs.SetInt (ENTRYWORLDKEY, w);
	}

	public int GetEntryWorldKey ()
	{
		return PlayerPrefs.GetInt (ENTRYWORLDKEY, 0);
	}



	public void SetIns1 (int i)
	{
		PlayerPrefs.SetInt (INS1, i);
	}

	public int GetIns1 ()
	{
		return PlayerPrefs.GetInt (INS1, 0);
	}

	public void SetIns2 (int i)
	{
		PlayerPrefs.SetInt (INS2, i);
	}

	public int GetIns2 ()
	{
		return PlayerPrefs.GetInt (INS2, 0);
	}

	public void SetIns3 (int i)
	{
		PlayerPrefs.SetInt (INS3, i);
	}

	public int GetIns3 ()
	{
		return PlayerPrefs.GetInt (INS3, 0);
	}

	public void SetIns4 (int i)
	{
		PlayerPrefs.SetInt (INS4, i);
	}

	public int GetIns4 ()
	{
		return PlayerPrefs.GetInt (INS4, 0);
	}

	public void SetIns5 (int i)
	{
		PlayerPrefs.SetInt (INS5, i);
	}

	public int GetIns5 ()
	{
		return PlayerPrefs.GetInt (INS5, 0);
	}

	public void SetRateIt (int r)
	{
		PlayerPrefs.SetInt (RATEIT, r);
	}

	public int GetRateIt ()
	{
		return PlayerPrefs.GetInt (RATEIT, 0);
	}

	public void DeleteAllPref ()
	{
		PlayerPrefs.DeleteAll ();
	}
}
