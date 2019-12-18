using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Services;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	//	Moving distance,Clamp value for X and Y and Mouse Threshhold distance
	public float Move, MinMaxX, MinMaxY, ThreshHoldDistance;
	[SerializeField]
	private float DragControl = 50f;
	// Block used to instantiate on the tail of the player on each move
	[SerializeField]
	private GameObject Block;
	// Weather block would need to generate or not
	[SerializeField]
	private bool DoBlocking = false;
	// Rotation angle needed for each rotation and rotation freezer count
	private int RotationAngle = -90, FreezeRCount = 0;
	// Single Edge rotation restrictions
	private bool Once1 = true, Once2 = true, Once3 = true, Once4 = true, CanFreezeRotation = false, IsItRight = false, RealDoBlocking;
	// Threshold for mouse distance before move the player
	private float ThresholdX, ThresholdY;
	// If true can move the player else can't move
	[HideInInspector]
	public bool CanMove = false;
	// Player object Rigidbody
	private Rigidbody2D rigid;
	private string FreezerName = "";
	private Vector2 CurrentPos;
	private List<GameObject> GenBlocks = new List<GameObject> ();
	[HideInInspector]
	public List<GameObject> Tactics = new List<GameObject> ();
	[HideInInspector]
	public GameObject Holder;

	void Awake ()
	{
		
	}

	void Start ()
	{
		rigid = GetComponent<Rigidbody2D> ();
		RealDoBlocking = DoBlocking;
	}

	void Update ()
	{
      

	}

	void OnMouseDown ()
	{
		CanMove = true;
		GameController.instance.DeactivatePlayerEff ();
	}

	void OnMouseDrag ()
	{
		GameController.CanShowPlayerEff = false;

		if (CanMove && !EventSystem.current.IsPointerOverGameObject ()) {
			if (GameController.PlayerPositionToggle) {
				GameController.PlayerPositionToggle = false;
				GameController.instance.PlayerPosition = transform.position;
				GameController.instance.PlayerRotation = transform.rotation;

				Holder = new GameObject ("Holder");
				Holder.transform.SetParent (GameObject.Find ("Level_" + GameController.instance.LevelNum).transform);
				GameController.instance.CurrentPlayer = GetComponent<PlayerController> ();
			}

			GameController.instance.Ghost.enabled = true;
			GameController.instance.Ghost.transform.position = transform.position;
			GameController.instance.Ghost.transform.rotation = transform.rotation;
			GameController.instance.Ghost.transform.localScale = transform.localScale + new Vector3 (0.16f, 0.16f, 0.16f);
//             Get the mouse position and change it from screen to world coordination
			Vector2 Mousepos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			CurrentPos = transform.position;



			ThresholdX = Mathf.Round ((Mousepos.x - CurrentPos.x) * 50f / DragControl);
			ThresholdY = Mathf.Round ((Mousepos.y - CurrentPos.y) * 50f / DragControl);

            
			try {
				if (ThresholdX >= ThreshHoldDistance) {
					transform.position = new Vector2 (Mathf.Clamp (CurrentPos.x + Move, MinMaxX, -MinMaxX), CurrentPos.y);
					//				rigid.MovePosition(new Vector2(Mathf.Clamp(CurrentPos.x + Move, MinMaxX, -MinMaxX), CurrentPos.y));
					
					if (transform.position.x > MinMaxX && transform.position.x < -MinMaxX) {
						RotateObject (RotationAngle);
						Once2 = true;
					} else if (Once1) {
						Once1 = false;
						RotateObject (RotationAngle);
					} else {
						UnityEngine.Debug.Log ("Do some");
					}
				} else if (ThresholdX <= -ThreshHoldDistance) {
					transform.position = new Vector2 (Mathf.Clamp (CurrentPos.x - Move, MinMaxX, -MinMaxX), CurrentPos.y);
					//				rigid.MovePosition(new Vector2(Mathf.Clamp(CurrentPos.x - Move, MinMaxX, -MinMaxX), CurrentPos.y));
					if (transform.position.x > MinMaxX && transform.position.x < -MinMaxX) {
						RotateObject (RotationAngle);
						Once1 = true;
					} else if (Once2) {
						Once2 = false;
						RotateObject (RotationAngle);
					}
				} else if (ThresholdY >= ThreshHoldDistance) {
					transform.position = new Vector2 (CurrentPos.x, Mathf.Clamp (CurrentPos.y + Move, MinMaxY, -MinMaxY));
					//				rigid.MovePosition(new Vector2(CurrentPos.x, Mathf.Clamp(CurrentPos.y + Move, MinMaxY, -MinMaxY)));
					if (transform.position.y > MinMaxY && transform.position.y < -MinMaxY) {
						RotateObject (RotationAngle);
						Once4 = true;
					} else if (Once3) {
						Once3 = false;
						RotateObject (RotationAngle);
					}
				} else if (ThresholdY <= -ThreshHoldDistance) {
					transform.position = new Vector2 (CurrentPos.x, Mathf.Clamp (CurrentPos.y - Move, MinMaxY, -MinMaxY));
					//				rigid.MovePosition(new Vector2(CurrentPos.x, Mathf.Clamp(CurrentPos.y - Move, MinMaxY, -MinMaxY)));
					if (transform.position.y > MinMaxY && transform.position.y < -MinMaxY) {
						RotateObject (RotationAngle);
						Once3 = true;
					} else if (Once4) {
						Once4 = false;
						RotateObject (RotationAngle);
					}
				}
			} catch (System.Exception ex) {
				UnityEngine.Debug.Log (ex);
			}
		}

	}

	// Rotate the player to given angle
	void RotateObject (int s)
	{
		transform.Rotate (0, 0, transform.rotation.z + s, Space.Self);
//		rigid.MoveRotation(transform.rotation.z + s);
		RoundUPRotation ();
		if (CanFreezeRotation) {
			FreezeRCount++;
		}
		if (FreezeRCount == 1 && FreezerName == "F1Freezer") {
			CanFreezeRotation = false;
			FreezeRCount = 0;
			RotationAngle = -90;
		} else if (FreezeRCount == 2 && FreezerName == "F2Freezer") {
			CanFreezeRotation = false;
			FreezeRCount = 0;
			RotationAngle = -90;
		} else if (FreezeRCount == 3 && FreezerName == "F3Freezer") {
			CanFreezeRotation = false;
			FreezeRCount = 0;
			RotationAngle = -90;
			FreezerName = "";
		}

		GenerateBlock ();
// Counts number of rots and sets it to rots text
		GameController.Rots += 1;
		GameController.instance.SetRotsTxt (GameController.Rots);

	}

	// If DoBlocking is true then it will instantiate Blocks behind the player and adds its to List
	void GenerateBlock ()
	{
		if (DoBlocking) {
			if (Block != null) {
				GameObject block = Instantiate (Block, CurrentPos, Quaternion.identity) as GameObject;

				if (Holder == null) {
					Holder = new GameObject ("Holder");
					Holder.transform.SetParent (GameObject.Find ("Level_" + GameController.instance.LevelNum).transform);
				}
				block.transform.SetParent (Holder.transform);
				block.transform.localScale = transform.localScale;
				GenBlocks.Add (block);
			}
		}
	}

	// Executes when mouse up or touch ends
	void OnMouseUp ()
	{
		GameController.CanShowPlayerEff = true;
		RoundUPRotation ();
		GameController.instance.Ghost.enabled = false;
		Invoke ("GetPlayerEff", 2f);
	}

	// When touch ends or mouse up then round the player rotation to nearer angle
	void RoundUPRotation ()
	{
		var vec = transform.eulerAngles;
		vec.z = Mathf.Round (vec.z / 90) * 90;
		transform.eulerAngles = vec;
	}

	// Setter for RotationAngle
	public void SetRotationAngle (int RotationAngle)
	{
		this.RotationAngle = RotationAngle;
	}

	// Getter for RotationAngle
	public int GetRotationAngle ()
	{
		return RotationAngle;
	}

	public void SetIsItRight (bool IsItRight)
	{
		this.IsItRight = IsItRight;
	}

	public bool GetIsItRight ()
	{
		return IsItRight;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "DoubleRotation") {
			UIManagement.instance.PlayTacktickSnd ();
			RotationAngle = -180;
		} else if (col.tag == "TripleRotation") {
			UIManagement.instance.PlayTacktickSnd ();
			RotationAngle = -270;
		} else if (col.tag == "F1Freezer" || col.tag == "F2Freezer" || col.tag == "F3Freezer") {
			UIManagement.instance.PlayTacktickSnd ();
			RotationAngle = 0;
			CanFreezeRotation = true;
			FreezerName = col.tag;
			DisableGameObject (col.gameObject);
		}

// Check for Blocker to end the game.
		FindBlocker (col);
// Check for BlockFree to stop block generation
		CheckBlockFree (col);
// Check for BlockRemover to remove already generated blocks
		CheckBlockRemover (col);
// Check for BlockStarter to start the block generation (If only currently no block is generating)
		CheckBlockStarter (col);
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.tag == "DoubleRotation") {
			DisableGameObject (col.gameObject);
			RotationAngle = -90;
		} else if (col.tag == "TripleRotation") {
			DisableGameObject (col.gameObject);
			RotationAngle = -90;
		}
	}


	void DisableGameObject (GameObject g)
	{
		Tactics.Add (g);
		g.SetActive (false);
	}



	void FindBlocker (Collider2D target)
	{
		if (target.tag == "Danger") {
			UIManagement.instance.PlayFailureSnd ();
			UIManagement.instance.FailureResetGame ();
			CanMove = false;   // Uncomment this, its necessary
			UIManagement.instance.IncrementAdStepCount ();
		}
	}

	// When player touches the BlockFree object it will stop the Block generation by setting DoBlocking to false
	void CheckBlockFree (Collider2D target)
	{

		if (target.tag == "BlockFree") {
			UIManagement.instance.PlayTacktickSnd ();
			DisableGameObject (target.gameObject);
			DoBlocking = false;
		}
	}

	// Check weather player touches the BlockRemover, if touches then it will removes all the generated blocks one by one form the scene
	void CheckBlockRemover (Collider2D target)
	{
		if (target.tag == "BlockRemover") {		
			UIManagement.instance.PlayTacktickSnd ();	
			DisableGameObject (target.gameObject);
			StartCoroutine (RemoveBlocksOneByOne (0.1f));
		}
	}

	public void CallRemoveBlocksOneByOne (float t)
	{
		StartCoroutine (RemoveBlocksOneByOneReset (t));
	}

	IEnumerator RemoveBlocksOneByOne (float t)
	{
		for (int i = GenBlocks.Count - 1; i >= 0; i--) {
			yield return new WaitForSeconds (t);    
			GameObject g = GenBlocks [i];
			Destroy (g);
		}
	}

	IEnumerator RemoveBlocksOneByOneReset (float t)
	{
		for (int i = GenBlocks.Count - 1; i >= 0; i--) {
			yield return new WaitForSeconds (t);    
			GameObject g = GenBlocks [i];
			GenBlocks.Remove (g);
			Destroy (g);

		}
		yield return new WaitForSeconds (0.1f); 
		transform.position = GameController.instance.PlayerPosition;
		transform.rotation = GameController.instance.PlayerRotation;
		yield return new WaitForSeconds (0.3f);   
		UIManagement.instance.GetTacticsBack (this);
	}

	// Check weather player touches the BlockStarter, if touches then start generating blocking from the next move
	void CheckBlockStarter (Collider2D col)
	{
		if (col.tag == "BlockStarter") {
			UIManagement.instance.PlayTacktickSnd ();
			DisableGameObject (col.gameObject);
			if (!DoBlocking)
				DoBlocking = true;
		}
	}

	// Resetting rotation and freezecount variables after level reset
	public void ResetAfterReset ()
	{
		RotationAngle = -90;
		FreezeRCount = 0;
		IsItRight = false;
		CanFreezeRotation = false;
		FreezerName = "";
		Once1 = true;
		Once2 = true;
		Once3 = true;
		Once4 = true;
		DoBlocking = RealDoBlocking;

		Invoke ("GetPlayerEff", 2f);
	}

	void GetPlayerEff ()
	{
		GameController.instance.ActivatePlayerEff ();
	}
	// Change Threshold distance to each level seperately to make player movement smoother
	void OnLevelWasLoaded (int l)
	{
		if (l == 1) {
			DragControl = 40;
		} else if (l == 2) {
			DragControl = 30;
		} else if (l == 3) {
			DragControl = 30;
		} else if (l == 4) {
			DragControl = 27;
		} else if (l == 5) {
			DragControl = 27;
		} else if (l == 6) {
			DragControl = 50;
		}
	}
}