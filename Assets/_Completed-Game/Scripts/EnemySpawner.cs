using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTB
{
    [RequireComponent(typeof(SphereCollider))]

    /// <summary>
    /// 敵生成オブジェクト　指定座標
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        GameObject PlayerBody;

        [SerializeField]
        GameObject Enemy;

        [SerializeField]
        float SpawnSpan = 5.0f;

        Vector3 SpawnPoint;

        
        bool isRunning = false;
        bool isAlreadySpawn = false;

        // Use this for initialization
        void Start()
        {
            SpawnPoint = transform.position;
            StartCoroutine(SpawnStart());
        }

        void Update()
        {
            
        }

        IEnumerator SpawnStart()
        {
            yield return new WaitForSeconds(Random.Range(-5f,5f));
            StartCoroutine(SpawnLoop());
            yield break;
        }

        IEnumerator SpawnLoop()
        {
            if (isRunning) yield break;
            isRunning = true;
            //  Debug.Log("I'm Running");
            if (isAlreadySpawn)
            {
                isAlreadySpawn = false;
            }
            else
            {
                GameObject SpawnedEnemy = Instantiate(Enemy, SpawnPoint, Quaternion.identity);
                SpawnedEnemy.GetComponent<EnemyBehavior>().target = PlayerBody;
                
            }

            yield return new WaitForSeconds(SpawnSpan);


            isRunning = false;
            StartCoroutine(SpawnLoop());
        }

        void OnTriggerStay(Collider col)
        {
            if (col.tag == "Enemy") isAlreadySpawn = true;
        }

        public void SetSpawnSpan(float _sec)
        {
            SpawnSpan = _sec;
        }

    }
}
