using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BodyNet : NetworkBehaviour
{
    public Transform target;
    private Transform selfT;
    // Start is called before the first frame update
    void Start()
    {
        selfT = transform;
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
