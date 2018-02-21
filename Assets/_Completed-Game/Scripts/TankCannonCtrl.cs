using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCannonCtrl : MonoBehaviour
{
    [SerializeField] Transform targetTr; // 目標
    [SerializeField] Transform turretTr; // 砲台
    [SerializeField] Transform cannonTr; // 砲塔

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // ターゲットを戦車座標系に
        Vector3 dir = targetTr.position - cannonTr.position;
        Quaternion invRot = Quaternion.Inverse(transform.rotation);
        Vector3 invDir = invRot * dir;
        float turretAng = getLongitudeRad(invDir) * Mathf.Rad2Deg; // ローカルでの方位角
        float cannonAng = getLatitudeRad(invDir) * Mathf.Rad2Deg; // ローカルでの仰角
        Quaternion turretRot = Quaternion.AngleAxis(turretAng, Vector3.up);
        Quaternion cannonRot = Quaternion.AngleAxis(cannonAng, -Vector3.right);
        turretTr.localRotation = turretRot; //Quaternion.Lerp(turretTr.localRotation, turretRot, 0.2f);
        cannonTr.localRotation = cannonRot; //Quaternion.Lerp(cannonTr.localRotation, cannonRot, 0.2f);
        Debug.DrawRay(cannonTr.position, cannonTr.forward * 100f);
    }

    private float getLongitudeRad(Vector3 _dir)
    {
        return Mathf.Atan2(_dir.x, _dir.z);
    }
    private float getLatitudeRad(Vector3 _dir)
    {
        float lxz = Mathf.Sqrt(_dir.x * _dir.x + _dir.z * _dir.z);
        return Mathf.Atan2(_dir.y, lxz);
    }
}