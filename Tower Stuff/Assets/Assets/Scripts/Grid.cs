using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------.
/// @class Grid
/// @brief Creates a grid (on Play). This consists of tiles and joined nodes for AI movement and tower placement.
///
/// @todo Needs player interaction
/// @todo Error checking via placement - aka always an optimal path
/// @todo A* algorithm for optimal path creation.
//------------------------

/// @test Allows viewing of Grid in Unity Editor - useful for debugging
/// --- [ExecuteInEditMode]
public class Grid: MonoBehaviour {

	[Header("Positioning and Size")]
	public int x;
	public int y;
	public float squareSize;
	public Vector3 startPos;

	[Header("Prefabs")]
	public Material tileFloorMatPrefab;
	public GameObject nodePrefab;

	[Header("Lists")]
	public List<GameObject> nodes;
	public List<Vector3> optimalPath;

	//------------------------
	/// Sets up the lists, creates the tiles (and nodes) and works out connections ready for in-game use. \n
	/// @todo Add A* optimal path work out when done.
	//------------------------
	public void Start ()
	{
		nodes = new List<GameObject> (x * y);
		optimalPath = new List<Vector3> ();

		Vector3 curPos;
		for (int i = 0; i < x; ++i) {
			for (int j = 0; j < y; ++j) {
				curPos = new Vector3 (startPos.x + (i * squareSize), startPos.y, startPos.z + (j * squareSize));
				CreateTile (curPos, i, j);
			}
		}

		Debug.Log ("All tiles and Nodes Created!");
		for (int i = 0; i < x; ++i) {
			for (int j = 0; j < y; ++j) {
				int index = (i * y) + j;
				CreateConnections (index);
			}

		}

		/// @test Testing the optimal path via hard coding
		optimalPath.Add(nodes[0].transform.position);
		//Debug.Log("optimalPath[0]:" + optimalPath[0]);
		optimalPath.Add(nodes[1].transform.position);
		//Debug.Log("optimalPath[1]:" + optimalPath[1]);
		optimalPath.Add((nodes[6].transform.position));
		//Debug.Log("optimalPath[2]:" + optimalPath[2]);
		optimalPath.Add((nodes[11].transform.position));
		//Debug.Log("optimalPath[3]:" + optimalPath[3]);
		optimalPath.Add((nodes[16].transform.position));
		//Debug.Log("optimalPath[4]:" + optimalPath[4]);
	}

	public void Update ()
	{
		DrawConnections();

		/// @test Prune Connection Test
		/*if(Input.GetButtonDown("Fire1"))
		{
			print("Removing some obstacles");
				ChangeObstacleAndCleanUp(0,true);
				ChangeObstacleAndCleanUp(7,true);
				ChangeObstacleAndCleanUp(17,true);
		}*/
	}

	//------------------------
	/// Returns a position of the optimal path. Used by AI for movement. \n
	/// If there are no more positions, returns a sentinel value to tell AI there are no more.
	/// @param[in] 	index 	Index in optimal Path List wanted
	///
	/// @return A Vector3 which denotes target position for the AI.
	/// @retval	Vector3(-999, -999, -999)	Sentinel value if out of range of the optimal path list.
	//------------------------
	public Vector3 GetOptimalPath (int index)
	{
		if (index < optimalPath.Count) {
			return optimalPath [index];
		} else {
			return new Vector3(-999, -999, -999); //if ran out of optimal path 
		}
	}

	//------------------------
	/// Draws the connections between nodes for testing and debug purposes.
	//------------------------
	void DrawConnections ()
	{
		for (int i = 0; i < x * y; ++i) {
			for (int j = 0; j < nodes [i].GetComponent<Node> ().connectedNodes.Count; ++j) {
				Debug.DrawLine(nodes[i].transform.position, nodes[nodes[i].GetComponent<Node>().connectedNodes[j]].transform.position);
			}
		}

	}

	//------------------------
	/// Creates tiles with nodes from scratch (a new GameObject). \n
	/// The mesh is also created from scratch and given a default material. \n
	/// A collider is also added for mouse selection purposes. \n
	/// Both tile and node are given index positions to make them easy to find. \n
	/// Node array is added to per node created via this function.
	/// @param[in]	_startPos	Position of corner to start the tile from
	/// @param[in]	x			x index of tile and node
	/// @param[in] y			y index of tile and node
	//------------------------
	void CreateTile (Vector3 _startPos, int x, int y)
	{
		// Tile creation
		GameObject tile = new GameObject() as GameObject;
		tile.transform.parent = this.transform;
		tile.gameObject.AddComponent<MeshFilter>();
		tile.gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = tile.GetComponent<MeshFilter>().mesh;
	 

		List<Vector3> verts = new List<Vector3>(4);
		List<Vector2> uvs = new List<Vector2>(4);
		List<int> tris = new List<int>(6);

		verts.Add(_startPos);
		verts.Add(new Vector3(_startPos.x + squareSize, _startPos.y, _startPos.z));
		verts.Add(new Vector3(_startPos.x, _startPos.y, _startPos.z + squareSize));
		verts.Add(new Vector3(_startPos.x + squareSize, _startPos.y, _startPos.z + squareSize));

		tris.Add(1);
		tris.Add(0);
		tris.Add(3);
		tris.Add(0);
		tris.Add(2);
		tris.Add(3);

		uvs.Add(new Vector2(0,0));
		uvs.Add(new Vector2(1,0));
		uvs.Add(new Vector2(0,1));
		uvs.Add(new Vector2(1,1));

		tile.GetComponent<MeshRenderer>().material = tileFloorMatPrefab;

		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.Optimize();
		tile.name = "Tile " + x + " " + y;

		// Collider creation
		BoxCollider col = tile.gameObject.AddComponent<BoxCollider>();
		col.center = new Vector3(_startPos.x + squareSize / 2.0f, _startPos.y, _startPos.z + squareSize / 2.0f);
		col.size = new Vector3(squareSize, squareSize / 2.0f, squareSize);

		// Node creation
		float midLength = squareSize / 2.0f;
		GameObject node = Instantiate(nodePrefab, new Vector3(_startPos.x + midLength, _startPos.y + midLength / 2.0f, _startPos.z + midLength), Quaternion.identity) as GameObject; 
		node.transform.localScale = new Vector3(squareSize / 10.0f, squareSize / 10.0f, squareSize / 10.0f);
		node.transform.parent = tile.transform;
		node.name = "Node " + x + " " + y;
		node.GetComponent<Node>().index = nodes.Count;
		nodes.Add(node);
	}

	//------------------------
	/// Goes through all nodes in the grid and connects them. \n
	/// Currently assumes no obstacles. \n
	/// Both tile and node are given index positions to make them easy to find. \n
	/// @param[in]	index	Index of the node to create connections for
	///
	//------------------------
	void CreateConnections (int index)
	{
		Node curNode = nodes[index].GetComponent<Node>();
		/* 
		Cases
			corner 	= 	2 connections
			sides 	= 	3 connections
			else 	= 	4 connections
		*/

		//top left corner - 2 connections
		if (index == 0) {
			//Debug.Log("Node Connections: " + "1" + " " + y);
			CheckToAddConnection(curNode, 1);
			CheckToAddConnection(curNode, y);
		} 
		//top right corner - 2 connections
		else if (index == (y - 1)) {
			//Debug.Log("Node Connections: " + (y - 2)  + " " + ((y - 1) + y));
			CheckToAddConnection(curNode, y - 2);
			CheckToAddConnection(curNode, (y - 1) + y);
		}
		//bottom right corner - 2 connections
		else if (index == ((x * y) - 1)) {
			//Debug.Log("Node Connections: " + (((x * y) - y - 1))  + " " + (((x * y) - 2)));
			CheckToAddConnection(curNode, ((x * y) - y - 1));
			CheckToAddConnection(curNode, ((x * y) - 2));
		} 
		//bottom left corner - 2 connections
		else if (index == ((x * y) - y)) {
			//Debug.Log("Node Connections: " + (((x * y) - y - y))  + " " + (((x * y) - y + 1)));
			CheckToAddConnection(curNode, ((x * y) - y - y));
			CheckToAddConnection(curNode, ((x * y) - y + 1));
		} 
		//top - 3 connections
		else if (index >= 1 && index < (y - 1)) {
			//Debug.Log("Node Connections: " + (index  -1)  + " " + (index + 1)  + " " + (index + y));
			CheckToAddConnection(curNode, index - 1);
			CheckToAddConnection(curNode, index + 1);
			CheckToAddConnection(curNode, index + y);
		} 
		//bottom - 3 connections
		else if (index >= ((x * y) - y) && index < ((x * y) - 1)) {
			//Debug.Log("Node Connections: " + (index - 1)  + " " + (index + 1)  + " " + (index - y));
			CheckToAddConnection(curNode, index - 1);
			CheckToAddConnection(curNode, index + 1);
			CheckToAddConnection(curNode, index - y);
		} 
		//left - 3 connections
		else if (index % y == 0) {
			//Debug.Log("Node Connections: " + (index - y)  + " " + (index + 1)  + " " + (index + y));
			CheckToAddConnection(curNode, index + y);
			CheckToAddConnection(curNode, index + 1);
			CheckToAddConnection(curNode, index - y);
		} 
		//right - 3 connections
		else if (index % y == (y - 1)) {
			//Debug.Log("Node Connections: " + (index - y)  + " " + (index - 1)  + " " + (index + y));
			CheckToAddConnection(curNode, index + y);
			CheckToAddConnection(curNode, index - 1);
			CheckToAddConnection(curNode, index - y);
		} 
		//middle - 4 connections
		else {
			//Debug.Log("Node Connections: " + (index - y)  + " " + (index + 1)  + " " + (index + y)  + " " + (index - 1));
			CheckToAddConnection(curNode, index + y);
			CheckToAddConnection(curNode, index + 1);
			CheckToAddConnection(curNode, index - y);
			CheckToAddConnection(curNode, index - 1);
		}
		Debug.Log("All Connections Created!");
	}

	//------------------------
	/// Changes a node to an obstacle or not depending on the boolean passed in. \n
	/// If it becomes an obstacle, connections from the node and connections to this node are pruned. \n
	/// If it stops being an obstacle, connections are reformed to viable neighbours and from them to this node. \n
	/// @param[in]	index	Index of the node from the grid's node list
	/// @param[in]	isObs	New boolean state of the node
	///
	//------------------------
	public void ChangeObstacleAndCleanUp (int index, bool isObs)
	{
		Node curNode = nodes [index].GetComponent<Node> ();

		//Node becomes an obstacle - for all neighbours, remove this node from the list and clear this nodes connections
		if (isObs == true && curNode.isObstacle != true) {
			foreach (int node in curNode.connectedNodes) {
				nodes [node].GetComponent<Node> ().connectedNodes.Remove (index);
			}
			curNode.connectedNodes.Clear ();
			curNode.UpdateObstacle (isObs);
		}
		//Node stops being an obstacle - give connections to the node, then go to each neighbour and if its not in there, add it back in
		else if (isObs == false && curNode.isObstacle != false) {
			curNode.UpdateObstacle (isObs);
			CreateConnections(index);
			foreach (int node in curNode.connectedNodes) {
				if (nodes [node].GetComponent<Node> ().connectedNodes.Contains (index) == false) {
					nodes[node].GetComponent<Node>().connectedNodes.Add(index);
				}
			}
		}
	}


	//------------------------
	/// Checks if a Node to connect to is an obstacle. \n
	/// If it is an obstacle, do not add a connection to it. \n
	/// If it is not an obstacle, add a connection to it. \n
	/// @param[in]	curNode	Node component of current Node
	/// @param[in]	toAdd	Index of node to connect to
	///
	//------------------------
	private void CheckToAddConnection (Node curNode, int toAdd)
	{
		if (nodes [toAdd].GetComponent<Node> ().isObstacle != true) {
			curNode.connectedNodes.Add(toAdd);
		}
	}

}