using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTest : MonoBehaviour
{

    public Transform targetRotation;
    private Transform thisT;
    private Rigidbody thisRb;
    public float slowDownAngularVelocity = 0.9f;
    public float maxRotationChange = 10f;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        thisT = GetComponent<Transform>();
        thisRb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {

        thisRb.angularVelocity *= slowDownAngularVelocity;

        Vector3 angularVelocity = FindNewAngularVelocity();
        float maxChange = maxRotationChange * Time.fixedDeltaTime;
        thisRb.angularVelocity = Vector3.MoveTowards(thisRb.angularVelocity, angularVelocity, maxChange);
    }

    private Vector3 FindNewAngularVelocity()
    {
        Quaternion difference = targetRotation.rotation * Quaternion.Inverse(thisT.rotation);
        difference.ToAngleAxis(out float angle, out Vector3 axis);
        
        angle = (angle > 180) ? angle -= 360 : angle;

        return(axis*angle* Mathf.Deg2Rad)/Time.fixedDeltaTime;
    }
}
