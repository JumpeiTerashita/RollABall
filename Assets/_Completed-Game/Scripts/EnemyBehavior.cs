using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyBehavior : MonoBehaviour {

    public GameObject target;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Tag: " + collision.gameObject.tag);
            Debug.Log("Name: " + collision.gameObject.name);
            Instantiate(explosion, transform.position, Quaternion.identity);
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
