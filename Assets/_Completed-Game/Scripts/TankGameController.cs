using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGameController : MonoBehaviour {
    [SerializeField]
    float fixedTimestep = 0.03f;
    private void Awake()
    {
        Time.fixedDeltaTime = fixedTimestep;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
