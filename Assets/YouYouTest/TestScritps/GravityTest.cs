using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTest : MonoBehaviour
{
    public Transform targetT;
    private Rigidbody selfRb;
    private Vector3 originPositon;
    private Quaternion originRotation;
    // Start is called before the first frame update
    void Start()
    {
        originPositon = transform.position;
        originRotation = transform.rotation;
        selfRb = GetComponent<Rigidbody>();
        //set gravity center to targetT
        selfRb.centerOfMass = targetT.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //selfRb.centerOfMass = targetT.localPosition;
        if(Input.GetKey(KeyCode.A))
        {
            Debug.Log("A");
            selfRb.centerOfMass = targetT.localPosition;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("S");
            //reset position and rotation
            transform.position = originPositon;
            transform.rotation = originRotation;
        }
    }
}
