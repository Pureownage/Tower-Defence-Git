using UnityEngine;
using System.Collections;

//------------------------.
/// @class EnemyMovement
/// @brief Component added to an enemy object to allow movement from it's spawn point to the attack point via the grid.
///
/// 
//------------------------
public class EnemyMovement : MonoBehaviour {

	[Header("Movement and Turning")]
	public Vector3 velocity;
	public Quaternion curRotation;
	public float moveSpeed;
	public float rotSpeed;
	public bool facing;

	[Header("Target")]
	public Vector3 targetPos;
	public int currentPathIndex;
	public float radius;

	[Header("Needed Objects")]
	public Grid grid;

	//------------------------
	/// Gets the grid object in the scene and gets the first optimal path from it and makes it the target position. \n
	/// @todo Add A* optimal path work out when done.
	//------------------------
	void Start () {
		grid = FindObjectOfType<Grid>();
		currentPathIndex = 0;
		targetPos = grid.GetOptimalPath(currentPathIndex);
		facing = false;
		radius = 0.01f;
	}

	//------------------------
	/// Enemy turns to face the target position. If it is turned to face it head on, will then move towards it. \n
	/// If the enemy is close enough to the target position, it will request the next position of the grid's optimal path. \n
	/// If it has ran out of positions (sentinel value given from GetOptimalPath), will move to the core.
	///
	/// @todo Add core move to case.
	//------------------------
	void Update ()
	{
		if(facing == false)
			facing = TurnToFace ();
		
		if (Vector3.Distance (targetPos, transform.position) <= radius) {
			facing = false;
			currentPathIndex++;
			targetPos = grid.GetOptimalPath (currentPathIndex); //may return Vector3(-999, -999, -999) - if so, end of path and go towards core - todo
		}
		if (targetPos == new Vector3 (-999, -999, -999)) {
			/// @issue Will rotate towards this, but will not move to it. Adding core move to case should solve this.
		}
		else if (facing == true) {
			velocity = (targetPos - transform.position).normalized * moveSpeed * Time.deltaTime;
			transform.position = transform.position + velocity;
		}


	}

	//------------------------
	/// Enemy turns to face the target position. Informs Update when it has reached the target rotation.
	/// 
	/// @return A boolean which determines if enemy is facing the target fully.
	/// @retval	true	Enemy is facing target rotation.
	/// @retval	false	Enemy is not facing target rotation. More rotation will need to be done to face target.
	//------------------------
	private bool TurnToFace ()
	{
		Quaternion targetRot = Quaternion.LookRotation ((targetPos - transform.position).normalized, Vector3.up);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRot, rotSpeed * Time.deltaTime);

		if (transform.rotation == targetRot) {
			return true;
		} else {
			return false;
		}
	}
}
