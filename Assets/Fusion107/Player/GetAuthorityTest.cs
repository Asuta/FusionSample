using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GetAuthorityTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30),("GetAuthority")))
        {
            var obj = transform.GetComponent<NetworkBehaviour>();
            obj.Object.RequestStateAuthority();
        }
    }
}
