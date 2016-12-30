using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

	public bool isObstacle;
	public List<int> connectedNodes;
	public int index;

	void Awake() {
		connectedNodes = new List<int>(); //could be 8 if we use euclidean neighbours, 4 if manhatten
		isObstacle = false;
	}

	void Update () {
	
	}

	public void UpdateObstacle (bool val)
	{
		isObstacle = val;
		if (isObstacle == true) {
			GetComponent<MeshRenderer> ().material.color = Color.red;
		} else {
			GetComponent<MeshRenderer> ().material.color = Color.white;
		}
	}
}