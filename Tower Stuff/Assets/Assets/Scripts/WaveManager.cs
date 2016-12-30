using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

//------------------------.
/// @class SpawnInfo
/// @brief A struct to hold both id and time information for spawning purposes when retrieved from a file.
//------------------------
public struct SpawnInfo
{
	public int id;
	public float time;

	public SpawnInfo (int _id, float _time)
	{
		id = _id;
		time = _time;
	}
}

//------------------------.
/// @class WaveManager
/// @brief Given a file, reads and creates wave data to send to the spawner in game. Also controls wave delays.
///
/// @todo tidy up
/// @todo test
/// @todo add comments
//------------------------
public class WaveManager : MonoBehaviour {

	[Header("Data")]
	public string waveFilepath;
	public List<string[]> waveData;
	public List<SpawnInfo[]> waveTimeData;

	[Header("Game Variables")]
	public int currentWaveNumber;
	public int maxWaves;
	public float waveDelay; //seconds in between waves - allow player to upgrade, get bearings etc - was 20.0f

	[Header("Needed Objects")]
	public Spawner spawnerObject;
	public Text waveText;

	//------------------------
	/// Load data from a file. Currently, each line is a wave. Finds the spawner and sends it the first wave data. 
	///
	/// @todo Allow the use of the 1 int and 1 float for better control of spawning times
	//------------------------
	void Start ()
	{
		waveDelay = 0.0f;
		waveFilepath = Path.Combine(Application.dataPath,"WaveData.txt"); // print (waveFilepath); - debug purposes
		waveData = new List<string[]> (); // if using 1 int per enemy
		//waveTimeData = new List<SpawnInfo[]>(); // if using 1 int and 1 float per enemy - eg 1:2.0 

		string line;
		StreamReader reader = new StreamReader (waveFilepath);
		using (reader) {
			do {
				line = reader.ReadLine ();

				if (line != null) {
					string[] lineData = line.Split ('\t');
					waveData.Add (lineData);
				}
			} while (line != null);
			reader.Close ();
		/*	
		@todo Better spawning times data use

		do {
				line = reader.ReadLine ();

				if (line != null) {
					string[] lineData = line.Split ('\t');
					string[] final = lineData[0].Split(':');
					Debug.Log(final.ToString());
					Debug.Log(final[0]);
					SpawnInfo info = new SpawnInfo(int.Parse(final[0]), float.Parse(final[0].Substring(1)));
					waveTimeData.Add(info);
					Debug.Log(waveTimeData[0].id + " : " + waveTimeData[0].time );
					//waveData.Add (lineData);
				}
			} while (line != null);*/
			reader.Close ();
		}

		currentWaveNumber = 0;
		maxWaves = waveData.Count;
		spawnerObject = FindObjectOfType<Spawner>();
		StartCoroutine(IncreaseWave());
	}

	void Update () {
	
	}

	//------------------------
	/// Send the wave data for a certain wave to the spawner to use.
	/// @param[in]	index	Index position for the waveData. Corresponds to wave number.
	//------------------------
	void SendWave (int index)
	{
		spawnerObject.GetWaveData(waveData[index]);
	}

	//------------------------
	/// Increases the wave number, updates text on screen and sends the wave data to the spawner. \n
	/// Has a delay time for sending the next wave to the spawner.
	///
	/// @todo All waves done case needed.
	//------------------------
	public IEnumerator IncreaseWave ()
	{
		Debug.Log("About to send wave!");
		if (currentWaveNumber < maxWaves) 
		{
			currentWaveNumber++;
			Debug.Log("Waiting..." + " Current Time: " + Time.time);
			yield return new WaitForSeconds(waveDelay); // was (waveDelay)
			Debug.Log("Sent wave " + currentWaveNumber + "!" + "Current Time: " + Time.time);
			waveText.text = "Wave " + currentWaveNumber + " of " + maxWaves;
			SendWave(currentWaveNumber-1);
		}
		else
		{
			//Player has won! - sort out end here
		}


	}
}
