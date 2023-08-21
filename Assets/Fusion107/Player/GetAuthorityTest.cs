using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GetAuthorityTest : MonoBehaviour
{
    // Start is called before the first frame update
    private NetworkBehaviour ballObj;
        
    void Start()
    {
        ballObj = transform.GetComponent<NetworkBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30),("GetAuthority")))
        {
            ballObj.Object.RequestStateAuthority();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 7)
        {
            //Debug.LogError(other.transform.root.name);
            Transform root = other.transform.root;
            var player = root.GetComponent<NetworkObject>();
            ballObj.Object.RequestStateAuthority();
            
            Debug.LogError(player.InputAuthority);
            ballObj.Object.AssignInputAuthority(player.InputAuthority);
        }
    }
}
