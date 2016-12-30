using UnityEngine;
using System.Collections;

public class TowerManager : MonoBehaviour
{

    Transform BarrelLook;
    Vector3 HoldTarget;
    Vector3 TargetPos;
    public int LockOnRange = 10;

    float Range = 10f;
    public GameObject BulletGameObject;

    float FireRate = 0.5f;
    float FireRateCooldown = 0f;

    // Use this for initialization
    void Start()
    {
        BarrelLook = transform.Find("Barrel");
        Enemy[] Enemys = GameObject.FindObjectsOfType<Enemy>();
        Enemy Target = LocateTarget(Enemys);
        TargetPos = Target.transform.position;
        HoldTarget = TargetPos;
        print("Found Starting Target");
        //print(TargetPos);
        BarrelLook.transform.LookAt(TargetPos);
        BarrelLook.transform.Rotate(new Vector3(1.0f, 0, 0), 90);
    }

    // Update is called once per frame
    void Update()
    {
            Enemy[] Enemys = GameObject.FindObjectsOfType<Enemy>();
        Enemy Target = LocateTarget(Enemys);
        print("Finding Target");
        TargetPos = Target.transform.position;
        
        
        //print(TargetPos);
        BarrelLook.transform.LookAt(TargetPos);
        BarrelLook.transform.Rotate(new Vector3(1.0f, 0, 0), 90);
        FireRateCooldown = FireRateCooldown - Time.deltaTime;
        Vector3 Direction = Target.transform.position - this.transform.position;
        if (FireRateCooldown <= 0 && Direction.magnitude<= Range)
        {
            FireRateCooldown = FireRate;
            ShootAt(Target);
            print("Firing!");
        }
    }

    void ShootAt(Enemy E)
    {
        GameObject BulletFire = (GameObject)Instantiate(BulletGameObject, this.transform.position, this.transform.rotation);

        Bullet B = BulletFire.GetComponent<Bullet>();
        B.Target = E.transform;

    }

    Enemy LocateTarget(Enemy[] Enemys)
    {
        Enemy CurrentClosest = null;
        float Min = Mathf.Infinity;
        foreach (Enemy X in Enemys)
        {
            // Work out what is the closest enemy, and set them as the target
            float Distance = Vector3.Distance(X.transform.position, this.transform.position);
            if (Distance < Min)
            {
                CurrentClosest = X;
                Min = Distance;
            }
        }
        return CurrentClosest;
    }



}