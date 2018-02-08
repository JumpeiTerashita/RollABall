using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickUpBehavior : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            var thisMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
           
            Sequence seq = DOTween.Sequence();
            // 色変更
            seq.Append(
                DOTween.ToAlpha(
                    () => thisMaterial.color,
                    color => {
                        thisMaterial.color = color;
                    },
                    0f,                                // 最終的なalpha値
                    1.2f
                )
                .SetEase(Ease.OutQuart)
                .OnComplete(() => { this.gameObject.SetActive(false); })
            );
            // スケーリング
            seq.Join(
                transform.DOScale(new Vector3(1.5f,1.5f,1.5f), 1.2f)
                .SetEase(Ease.OutQuart)
            );
            // めりこまないように移動
            seq.Join(
                transform.DOLocalMoveY(1,1.2f)
                .SetEase(Ease.OutQuart)
            );

            

        }
    }
}
