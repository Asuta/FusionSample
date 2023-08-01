using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotTest : MonoBehaviour
{
    public Transform[] ignoreObjects;
    public Transform[] ignoreSelfObjects;
    
    public float numberofIgnoreRigids;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("space");
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("space");
        }
    }
}
