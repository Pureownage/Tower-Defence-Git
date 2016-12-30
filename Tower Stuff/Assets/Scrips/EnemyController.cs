using UnityEngine;
using System.Collections;

public class EnemyController: MonoBehaviour {

    public float Health = 2f;
    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        
    
	}

    public void BeHit(float damage)
    {
        Health = Health - damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //audio.Play();
       Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {

	}
}
