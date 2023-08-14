using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion106;
using com.cyborgAssets.inspectorButtonPro;

public class NewBehaviourScript : MonoBehaviour
{
    public PhysxBall  targetScripts;
    public Transform[] targetTs;
    public Rigidbody[] rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ProButton]
    public void test()
    {
        //遍历targetScripts中的targetTs[]，并赋值给自己的targetTs[]
        targetTs = targetScripts.targetTs;

        //遍历targetScripts中的rigidbodies[]，并赋值给自己的rigidbodies[]
        rigidbodies = targetScripts.rigidbodies;

    }
}
