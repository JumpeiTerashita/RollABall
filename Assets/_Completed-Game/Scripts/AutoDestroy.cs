using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// アタッチされたGameObjectは自壊する。
/// </summary>

namespace KTB
{
    public class AutoDestroy : MonoBehaviour
    {

        [SerializeField]
        float DestroyLimit = 1.0f;
        // Use this for initialization
        void Start()
        {
            StartCoroutine(Destroy());
        }

        public void SetDestroyLimit(float _DestroyLimit)
        {
            DestroyLimit = _DestroyLimit;
        }

        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(DestroyLimit);
            Destroy(this.gameObject);
        }
    }
}