using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWayBehavior : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FixedUpdate()
    {
        
        if (ReticuleBehavior.isMoving)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("hit! " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Field")
        {
            Destroy(this.gameObject,0.5f);
        }
        
    }
}
