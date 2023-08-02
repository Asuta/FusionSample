using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ColliderHurt : MonoBehaviour
{
    public CollisionHurtEvent GetHurt;

    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    private void OnCollisionEnter(Collision other)
    {
        //检测碰撞到的物体是图层是不是6
        if (other.gameObject.layer == 6)
        {
            // get the impact of this collision 
            float mag = other.impulse.magnitude;
            Debug.LogError("碰撞力度" + mag);
            GetHurt.Invoke(mag);
        }

        
    }
}
