using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float BulletSpeed = 1f;
    public float Damage = 1f;
    public Transform Target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 Direction = Target.position - this.transform.localPosition;

        float DistanceThisFrame = BulletSpeed * Time.deltaTime;

        if (Direction.magnitude <= DistanceThisFrame)
        {
            HitTarget();
        }
        else
        {
            transform.Translate(Direction.normalized * DistanceThisFrame, Space.World);
            Quaternion TargetRotation = Quaternion.LookRotation(Direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, TargetRotation, Time.deltaTime * 5);

        }
	}

    void HitTarget()
    {
        Target.GetComponent<EnemyController>().BeHit(Damage);
        Destroy(gameObject);
    }

}
