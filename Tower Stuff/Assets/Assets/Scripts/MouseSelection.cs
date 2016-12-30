using UnityEngine;
using System.Collections;

//------------------------.
/// @class MouseSelection
/// @brief Tracks mouse position and currently allows user to update the node's obstacle value
///
/// 
//------------------------
public class MouseSelection : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	public Grid grid;

	void Start () {
		grid = FindObjectOfType<Grid>();
	}
	
	//------------------------
	/// If user left clicks, checks for any collision, gets the Node child and updates the obstacle. \n
	///
	/// @todo Check for a tile collision instead of any collision.
	//------------------------
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				print (hit.collider.name);
				Node curNode = hit.transform.gameObject.GetComponentInChildren<Node>();
				grid.ChangeObstacleAndCleanUp(curNode.index, !curNode.isObstacle);
			}
		}
	}
}
