using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[Header("Enemy Info")]
	public string name;
	public int hp;
	public int attackDmg;
	public int armorLvl;
	/// @todo enum for resistances/damage negators?

	[Header("Money")]
	public int moneyVal;
	//public int spawnCost; //needed as wave system uses the text file rather than picking via a cost?


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
