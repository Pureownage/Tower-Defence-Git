using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//------------------------
/// @class Spawner
/// @brief Given wave data, creates enemies for the current wave.
///
//------------------------
public class Spawner : MonoBehaviour {

	[Header("Lists")] // @todo Use List objects for these instead of standard array?
	public GameObject[] enemies;
	public Transform[] spawnPoints;
	public string[] waveData;

	[Header("Variables")]
	public float spawnTime = 2.0f;
	public int currentAlive;
	public int curIndex;
	public bool spawned;

	[Header("Needed Objects")]
	public Transform enemyParentHolder;
	public Text remainingText;

	//------------------------
	/// Sets up some variables.
	//------------------------
	void Start () 
	{
		currentAlive = 0;
        curIndex = 0;
        spawned = false;
	}

	//------------------------
	/// If everything has been spawned of this wave and no more enemies to attack, ask for next wave (which will be delayed).
	//------------------------
	void Update ()
	{
		if (spawned == true && currentAlive == 0) 
		{
			/// @todo Tell WaveManager to update to next wave and await next GetWaveData 
		}
	}

	//------------------------
	/// Waits and spawns enemies given the wave data and the prefab list of enemies. \n
	/// Currently uses a random spawn point for each enemy spawn. \n
	/// Keeps track of current Alive enemies and updates text as required. \n
	/// After all are spawned, sets spawned variable to true. \n
	//------------------------
	IEnumerator Spawn ()
	{
		int amount = waveData.Length;
		int spawnAmount = spawnPoints.Length;
		for (int i = 0; i < amount; ++i) 
		{
			yield return new WaitForSeconds(spawnTime);
			Debug.Log("Spawn Time: " + Time.time);
			int spawnLocation = PickRandomSpawner(spawnAmount);
			GameObject enemy = Instantiate (enemies [int.Parse (waveData [i])], spawnPoints [spawnLocation].position, spawnPoints [spawnLocation].rotation) as GameObject;
			enemy.transform.parent = enemyParentHolder;
			currentAlive++;
			remainingText.text = "Remaining: " + currentAlive;
		}

		spawned = true;
    }

	//------------------------
	/// Returns a random number given the amount of spawn points. \n
	/// Amount is exclusive so will give a range between 0 - (amount - 1).
	///
	/// @param[in]	amount	Amount of spawn points (but generic enough to be used for any randomness).
	//------------------------
    public int PickRandomSpawner (int amount)
	{
		return Random.Range(0, amount);
	}

	//------------------------
	/// Takes the enemy data and places it into waveData. Starts spawning these. \n
	///
	/// @param[in]	data	A list of strings that dictate what enemys to spawn.
	//------------------------
    public void GetWaveData (string[] data)
	{
		Debug.Log("Wave received!");
		waveData = data;
		StartCoroutine(Spawn());
	}

	//------------------------
	/// Lowers currentAlive by 1.
	///
	/// @todo To be called by enemies when they run out of health.
	//------------------------
	void RemoveAlive()
	{
		currentAlive--;
	}
}
