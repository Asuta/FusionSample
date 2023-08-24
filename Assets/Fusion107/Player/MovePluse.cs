using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.EventSystems;
using System;
using com.cyborgAssets.inspectorButtonPro;

namespace Fusion107
{
    public class MovePluse : NetworkBehaviour
    {
        public float forceNum = 10f;
        public Rigidbody[] rigidbodies;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionStay(Collision other)
        {
            if (!HasStateAuthority)
            {
                return;
            }

            //判断other的layer是不是8号（用二进制）
            if ((1 << other.gameObject.layer & 1 << 8) != 0)
            {
                //获取到碰撞产生的力
                Vector3 force = other.impulse;
                force = force * forceNum;
                
                //给所有的刚体添加力
                foreach (var item in rigidbodies)
                {
                    item.AddForce(force);
                    Debug.LogError("添加了" + force);
                }
            }
        }
    }

}