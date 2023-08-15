using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BodyNet : NetworkBehaviour
{
    public Transform target;
    private Transform selfT;
    public ConfigurableJoint confiJoint;
    // Start is called before the first frame update
    void Start()
    {
        selfT = transform;


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







}
