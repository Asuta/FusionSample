using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelvisMove : MonoBehaviour
{
    public Transform headT;
    public float distance;
    private Transform thisT;

    private void Awake()
    {
        thisT = transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        thisT.position = headT.position - Vector3.down * distance;
        thisT.rotation = Quaternion.Euler(0, headT.eulerAngles.y, 0);
        
    }
}
