using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGenerator : MonoBehaviour
{

    [SerializeField]
    GameObject pickUp;

    [SerializeField]
    int pickUpNum = 12;

    [SerializeField]
    float instLength = 6.5f;

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.targetNum = pickUpNum;

        GenerateAsCircle(pickUpNum);
    }

    void GenerateAsCircle(int _generateNum)
    {
        float thita = 2 * Mathf.PI / _generateNum;
        for (int i = 1; i <= _generateNum; i++)
        {
            GameObject instObj = Instantiate(pickUp);
            instObj.transform.position = new Vector3(
                (instLength * Mathf.Cos(thita * i)) + this.transform.position.x,
                this.transform.position.y,
                (instLength * Mathf.Sin(thita * i)) + this.transform.position.z
                );
            instObj.transform.Rotate(
                new Vector3(
                    Random.Range(-180f, 180f),
                    Random.Range(-180f, 180f),
                    Random.Range(-180f, 180f)
                    )
            );
            instObj.transform.SetParent(this.gameObject.transform);
        }
    }
}
