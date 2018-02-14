using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTB;

public class Cannon : MonoBehaviour {

    [SerializeField]
    GameObject target;

    [SerializeField]
    GameObject bullet;

    float intervalTimer = 0;

    [SerializeField]
    float interval = 1f;

    [SerializeField]
    float arrivalTime = 1f;

    [SerializeField]
    float bulletLifeTime = 3f;
	
	// Update is called once per frame
	void Update () {
        Debug.Log(intervalTimer);
        intervalTimer += Time.deltaTime;
        Vector3 direction;
        Vector3 forward;

        direction = (target.transform.position - transform.position);
        forward = direction.normalized;

        Quaternion rot = Quaternion.LookRotation(forward);
        transform.rotation = rot;
        forward = transform.forward;
        if (intervalTimer > interval)
        {
            intervalTimer -= interval;

            Vector3 spd;
            

            spd = forward * arrivalTime;

            if (arrivalTime>0f)
            {
                spd = forward * (direction.magnitude/arrivalTime);
            }

            //Vector3[] v0 = MyMath.CalcTargetVec(5f, direction);

            GameObject obj = Instantiate(bullet);
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<Rigidbody>().AddForce(spd, ForceMode.VelocityChange);
            Destroy(obj, bulletLifeTime);
        }
	}
}
