using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankReticuleBehavior : MonoBehaviour {
    // 位置座標
    private Vector3 position;
    // スクリーン座標をワールド座標に変換した位置座標
    private Vector3 screenToWorldPointPosition;

    [SerializeField] Transform cannonTr; // 砲塔

    //public LayerMask mask;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        // Vector3でマウス位置座標を取得する
        position = Input.mousePosition;
        // Z軸修正
        position.z = 30f;
        // マウス位置座標をスクリーン座標からワールド座標に変換する
        screenToWorldPointPosition = (Camera.main.ScreenToWorldPoint(position));

        Ray ray = new Ray(cannonTr.position, screenToWorldPointPosition-cannonTr.position);

        RaycastHit hit;

        Debug.DrawRay(cannonTr.position, screenToWorldPointPosition - cannonTr.position);

        if (Physics.Raycast(ray, out hit,30f))
        {
            //// Examples
            //// 衝突したオブジェクトの色を赤に変える
            //hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
            //// 衝突したオブジェクトを消す
            //Destroy(hit.collider.gameObject);
            // Rayの衝突地点に、このスクリプトがアタッチされているオブジェクトを移動させる
            //this.transform.position = hit.point;
            // Rayの原点から衝突地点までの距離を得る
            //Debug.Log("Object Hit");
            gameObject.transform.position = hit.point+ (-hit.point + cannonTr.position).normalized * 2f;
             
            // 衝突したオブジェクトのコライダーを非アクティブにする
            //hit.collider.enabled = false;
        }
        else gameObject.transform.position = screenToWorldPointPosition;


        // ワールド座標に変換されたマウス座標を代入
        //gameObject.transform.position = screenToWorldPointPosition;
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
