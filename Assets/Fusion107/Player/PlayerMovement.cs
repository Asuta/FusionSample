using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Fusion107
{
    public class PlayerMovement : NetworkBehaviour
    {
        private CharacterController _controller;

        public float PlayerSpeed = 2f;

        public float JumpForce = 5f;
        public float GravityValue = -9.81f;

        private Vector3 velocity;
        private bool _jumpPressed;

        [Header("body movement")]
        public Rigidbody body;
        public float bodySpeed = 2f;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }



        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                // Debug.LogError("Jump pressed");
                _jumpPressed = true;
            }
        }

        public override void FixedUpdateNetwork()
        {

            // velocity.y += GravityValue * Runner.DeltaTime;
            // if (_jumpPressed && _controller.isGrounded)
            // {
            //     velocity.y += JumpForce;
            //     // Debug.LogError("Jump");
            // }
            // _jumpPressed = false;
            // // Only move own player and not every other player. Each player controls its own player object.
            // if (HasStateAuthority == false)
            // {
            //     return;
            // }

            // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;


            // if (move != Vector3.zero)
            // {
            //     gameObject.transform.forward = move;
            // }

            // _controller.Move(move + velocity * Runner.DeltaTime);

            // if (_controller.isGrounded)
            // {
            //     velocity = new Vector3(0, -1, 0);
            // }



            if (HasStateAuthority)
            {
                //用wasd控制body的velocity
                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * bodySpeed;
                if (move != Vector3.zero)
                {
                    body.velocity = move;
                }
                else
                {
                    body.velocity = Vector3.zero;
                }
            }




        }
    }

}