using System;
using Fusion;
using Fusion.Sockets;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class AutoStart : MonoBehaviour
{

    public NetworkDebugStart debugStart;
    // Start is called before the first frame update
    void Start()
    {
        
        //start a coroutine to wait for the server to start
        StartCoroutine(WaitForServer());
    }

 
    IEnumerator WaitForServer()
    {
        //wait for the server to start
        yield return new WaitForSeconds(1f);
        //start the game
        debugStart.StartSharedClient();
    }

    // Update is called once per frame
    void Update()
    {

    }
}



