using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion106;
using com.cyborgAssets.inspectorButtonPro;

public class NewBehaviourScript : MonoBehaviour
{
    //public PhysxBall  targetScripts;
    public Transform[] targetTs;
    public Rigidbody[] rigidbodies;
    public float slowDownAngularVelocity = 0.9f;
    public float maxRotationChange = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            
            Quaternion difference = targetTs[i].rotation * Quaternion.Inverse(rigidbodies[i].rotation);
            difference.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180)
            {
                angle -= 360;
            }

            Vector3 targetAngleVelocity = (axis * angle * Mathf.Deg2Rad) / Time.fixedDeltaTime;
            //array[i].Value = targetAngleVelocity;
            rigidbodies[i].angularVelocity *= slowDownAngularVelocity;
            float maxChange = maxRotationChange * Time.fixedDeltaTime;
            rigidbodies[i].angularVelocity = Vector3.MoveTowards(rigidbodies[i].angularVelocity, targetAngleVelocity, maxChange);
        }
    }










    [ProButton]
    public void test()
    {
        // //遍历targetScripts中的targetTs[]，并赋值给自己的targetTs[]
        // targetTs = targetScripts.targetTs;

        // //遍历targetScripts中的rigidbodies[]，并赋值给自己的rigidbodies[]
        // rigidbodies = targetScripts.rigidbodies;
    }
}
