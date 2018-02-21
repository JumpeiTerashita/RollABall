using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyBehavior : MonoBehaviour {

    [SerializeField]
    private GameObject target;
    private NavMeshAgent agent;

    [SerializeField]
    GameObject explosion;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // ターゲットの位置を目的地に設定する。
        agent.SetDestination(target.transform.position);
        //Debug.Log(agent.destination);
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Finish"))
        {
            Invoke("Explode", 1f);


            //Sequence seq = DOTween.Sequence();
            //// 色変更
            //seq.Append(
            //    transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1.2f)
            //    .SetEase(Ease.OutQuart)
            //    .OnComplete(() => {
            //        Debug.Log("Destroied");

            //        Destroy(this.gameObject);
            //    })
            //);


        }
    }
}
