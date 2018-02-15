using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody))]
public class ReticuleBehavior : MonoBehaviour
{

    [SerializeField]
    GameObject thrower;

    // Create public variables for player speed, and for the Text UI game objects
    public float speed = 1.0f;

    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private Rigidbody rb;

    public static bool isMoving = false;

    // Use this for initialization
    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);


        // Set some local float variables equal to the value of our Horizontal and Vertical Inputs
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            isMoving = false;
        }
        else isMoving = true;

        Debug.Log("isMove : " + isMoving);

        // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // スピードを退避
        var spdMagnitude = movement.magnitude;

        // movement（ワールドに対して水平/垂直）を、カメラに対してして水平/垂直に変換する
        movement = Camera.main.transform.rotation * movement;

        // y はゼロで固定
        movement.y = 0;

        // 長さを戻す
        movement = movement.normalized * spdMagnitude;


        transform.position += movement;

        // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
        // multiplying it by 'speed' - our public player speed that appears in the inspector
        //rb.AddForce(movement* speed,ForceMode.VelocityChange);
    }
}
