using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTB;

public class ControllableCannon : MonoBehaviour
{

    [SerializeField]
    GameObject target;

    [SerializeField]
    GameObject bullet;

    float intervalTimer = 0;

    [SerializeField]
    float interval = 10f;

    [SerializeField]
    float arrivalTime = 1f;

    [SerializeField]
    float bulletLifeTime = 3f;

    [SerializeField]
    DirMode mode = DirMode.Low;
    [SerializeField]
    bool useParabolic = false;

    [SerializeField]
    float ParabolicPower = 30f;

    Vector3 direction;
    Vector3 forward;


    Vector3 targetPosOld;

    void LookTarget()
    {

        direction = (target.transform.position - transform.position);
        forward = direction.normalized;

        Quaternion rot = Quaternion.LookRotation(forward);
        transform.rotation = rot;
        forward = transform.forward;
        return;
    }

    void DirectShoot()
    {
        Vector3 spd;
        spd = forward * arrivalTime;

        if (arrivalTime > 0f)
        {
            spd = forward * (direction.magnitude / arrivalTime);
        }

        GameObject obj = Instantiate(bullet);

        obj.GetComponent<Rigidbody>().useGravity = false;
        obj.GetComponent<Rigidbody>().AddForce(spd, ForceMode.VelocityChange);
        Destroy(obj, bulletLifeTime);
        return;
    }



    private void Start()
    {
        intervalTimer = interval;
        targetPosOld = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(intervalTimer);
        intervalTimer += Time.deltaTime;

        LookTarget();

        if (intervalTimer > interval)
        {
            intervalTimer -= interval;

            if (!useParabolic) DirectShoot();
            else
            {
                Vector3[] v0Arr = MyMath.ParabolicVec(ParabolicPower, direction);
                if (v0Arr == null)
                {
                    return;
                }
                Vector3 v0 = v0Arr[(int)mode];

                // 水平方向の弾丸速度
                float v0H = Mathf.Sqrt(v0.x * v0.x + v0.z * v0.z);
                // 水平方向のターゲット距離
                float dirH = Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z);
                float timeH = dirH / v0H; // 到達予測時間
                                          // ターゲットの予測速度ベクトル
                Vector3 tv = (target.transform.position - targetPosOld) / Time.deltaTime;
                direction += tv * timeH; // 元いた場所に到達するまでの時間にターゲットが移動する先
                v0Arr = MyMath.ParabolicVec(ParabolicPower, direction);
                if (v0Arr == null)
                {
                    return;
                }
                v0 = v0Arr[(int)mode];

                GameObject obj = Instantiate(bullet);
                obj.GetComponent<Rigidbody>().useGravity = true;
                obj.GetComponent<Rigidbody>().AddForce(v0, ForceMode.VelocityChange);
         
                Destroy(obj, 7f);
            }
        }
        targetPosOld = target.transform.position;
    }
}
