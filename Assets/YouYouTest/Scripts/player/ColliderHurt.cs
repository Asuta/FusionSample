using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ColliderHurt : MonoBehaviour
{
    public CollisionHurtEvent GetHurt = new CollisionHurtEvent();

    private void Awake()
    {

    }
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
            float mag = other.impulse.magnitude;            
            Vector3 pos = other.contacts[0].point;
            GetHurt.Invoke(pos, mag);
        }

        // float magg = other.impulse.magnitude;
        // Vector3 poss = other.contacts[0].point;
        // //Debug.LogError("碰撞力度" + magg);
        // GetHurt.Invoke(poss,magg);

    }
}
