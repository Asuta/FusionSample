using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public Transform target;
    private Transform thisT;


    // Start is called before the first frame update
    void Awake()
    {
        thisT = transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        thisT.position = target.position;
        thisT.rotation = target.rotation;
    }
}
