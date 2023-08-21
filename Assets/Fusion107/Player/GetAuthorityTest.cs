using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion107;
using Unity.VisualScripting;
using UnityEngine;

public class GetAuthorityTest : NetworkBehaviour
{
    // Start is called before the first frame update
    private NetworkObject ballObj;
    public PlayerRef localPlayer;

    public override void Spawned()
    {
        ballObj = transform.GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30),("GetAuthority")))
        {
            GetAuthority();
        }
    }

    private void GetAuthority()
    {
        ballObj.AssignInputAuthority(localPlayer);
        ballObj.RequestStateAuthority();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 7)
        {
            //Debug.LogError(other.transform.root.name);
            Transform root = other.transform.root;
            var player = root.GetComponent<PlayerMovement>();
            player.OnBallCollider(ballObj);
        }
    }
}
