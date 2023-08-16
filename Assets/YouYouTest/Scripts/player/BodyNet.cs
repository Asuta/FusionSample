using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class BodyNet : NetworkBehaviour
{
    public Transform target;
    private Transform selfT;
    public ConfigurableJoint confiJoint;

    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotfrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;
    //[SerializeField] Rigidbody playerRigidbody;
    public Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        selfT = transform;
        _rigidbody = GetComponent<Rigidbody>();


        if (HasStateAuthority == false)
        {
            //lock the xyz move of the joint
            confiJoint.xMotion = ConfigurableJointMotion.Locked;
            confiJoint.yMotion = ConfigurableJointMotion.Locked;
            confiJoint.zMotion = ConfigurableJointMotion.Locked;
        }

    }





    // Update is called once per frame
    void Update()
    {
        if (HasStateAuthority)
        {
            selfT.position = target.position;
            selfT.rotation = target.rotation;
        }

    }


    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (HasStateAuthority == false)
        {
            PIDMovement();
        }
        
    }



    void PIDMovement()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + ( - _rigidbody.velocity) * kdg;
        _rigidbody.AddForce(force, ForceMode.Acceleration);
    }


}
