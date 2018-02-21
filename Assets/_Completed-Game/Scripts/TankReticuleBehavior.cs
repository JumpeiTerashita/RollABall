using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankReticuleBehavior : MonoBehaviour {
    // 位置座標
    private Vector3 position;
    // スクリーン座標をワールド座標に変換した位置座標
    private Vector3 screenToWorldPointPosition;

    [SerializeField] Transform cannonTr; // 砲塔


    public LayerMask mask;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(cannonTr.position, cannonTr.forward * 30f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10.0f, mask))
        {
            //// Examples
            //// 衝突したオブジェクトの色を赤に変える
            //hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
            //// 衝突したオブジェクトを消す
            //Destroy(hit.collider.gameObject);
            // Rayの衝突地点に、このスクリプトがアタッチされているオブジェクトを移動させる
            //this.transform.position = hit.point;
            // Rayの原点から衝突地点までの距離を得る
            float dis = hit.distance;
            // 衝突したオブジェクトのコライダーを非アクティブにする
            //hit.collider.enabled = false;
        }

        Debug.DrawRay(cannonTr.position, cannonTr.forward * 30f);
        // Vector3でマウス位置座標を取得する
        position = Input.mousePosition;
        // Z軸修正
        position.z = 30f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        screenToWorldPointPosition = (Camera.main.ScreenToWorldPoint(position));


        // ワールド座標に変換されたマウス座標を代入
        gameObject.transform.position = screenToWorldPointPosition;
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
