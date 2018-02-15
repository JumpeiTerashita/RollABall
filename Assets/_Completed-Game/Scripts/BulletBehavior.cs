using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KTB;

[RequireComponent(typeof(Rigidbody))]
public class BulletBehavior : MonoBehaviour
{
    public Vector3 spd;

    [SerializeField]
    GameObject explosion;


    // Use this for initialization
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Alone") return;

        Instantiate(explosion,transform.position,Quaternion.identity);

        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        var thisMaterial = this.gameObject.GetComponent<MeshRenderer>().material;

        Sequence seq = DOTween.Sequence();
        // 色変更
        seq.Append(
            DOTween.ToAlpha(
                () => thisMaterial.color,
                color =>
                {
                    thisMaterial.color = color;
                },
                0f,                                // 最終的なalpha値
                1.2f
            )
            .SetEase(Ease.OutQuart)
            .OnComplete(() => {

                //this.gameObject.SetActive(false);
                Destroy(gameObject);
            })
        );
        // スケーリング
        seq.Join(
            transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1.2f)
            .SetEase(Ease.OutQuart)
        );
        //// めりこまないように移動
        //seq.Join(
        //    transform.DOLocalMoveY(1, 1.2f)
        //    .SetEase(Ease.OutQuart)
        //);
    }
}
