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
        public Collider bodyCollider;
        public Collider planeCollider;
        public float bodySpeed = 2f;
        //define a list
        public List<Rigidbody> allBodys = new List<Rigidbody>();
        public Transform headDirection;

        [Header("XR rig")]
        public GameObject XRrig;
        public InputActionProperty leftStick;
        public InputActionProperty rightStick;
        public InputActionProperty Abutton;
        public InputActionProperty Bbutton;
        public float upDownSpeed;

        [Header("hand Grab")]
        public float grabRadius;
        public InputActionProperty leftGrab;
        public InputActionProperty rightGrab;
        public Rigidbody leftHandRb;
        public Transform leftHandGrabT;
        public Rigidbody rightHandRb;
        public Transform rightHandGrabT;

        [Header("test")]
        public NetworkObject thisNetworkObject;



        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public override void Spawned()
        {
            Debug.LogError("Spawned");
            //find a gameobject the name of floor,and get the collider to planeCollider
            planeCollider = GameObject.Find("Floor").GetComponent<Collider>();
            XRrig.SetActive(HasStateAuthority);
            //if (HasStateAuthority == false) ,ignore collition between body and plane
            if (HasStateAuthority == false)
            {
                Physics.IgnoreCollision(bodyCollider, planeCollider);
                body.useGravity = false;
                // set allbodys to unuse gravity
                foreach (Rigidbody rb in allBodys)
                {
                    rb.useGravity = false;
                }
            }

            if (HasStateAuthority)
            {
                //set allbodys to kinematic ,and 1 second later set to unkinematic
                foreach (Rigidbody rb in allBodys)
                {
                    rb.isKinematic = true;
                }
                StartCoroutine(SetKinematic());
            }


        }

        IEnumerator SetKinematic()
        {
            yield return new WaitForSeconds(1f);
            foreach (Rigidbody rb in allBodys)
            {
                rb.isKinematic = false;
            }
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




            if (Input.GetKeyDown(KeyCode.F))
            {
                DealDamageRpc(thisNetworkObject.Id,thisNetworkObject);
            }




            if (HasStateAuthority)
            {

                //buttonA,让xrrig往上移动，buttonB，让xrrig往下移动
                if (Abutton.action.triggered)
                {
                    XRrig.transform.position += new Vector3(0, upDownSpeed, 0);
                }
                if (Bbutton.action.triggered)
                {
                    XRrig.transform.position += new Vector3(0, -upDownSpeed, 0);
                }

                if (leftGrab.action.triggered)
                {
                    //make a spherecast ,and all the collider in the spherecast will be in the array,then add a fixedjoint to the first collider in the array
                    GrabSomething(leftHandGrabT, leftHandRb);
                }
                if (rightGrab.action.triggered)
                {
                    GrabSomething(rightHandGrabT, rightHandRb);
                }

                if (leftGrab.action.WasReleasedThisFrame())
                {
                    Debug.LogError("leftGrab released");
                    //remove the fixedjoint
                    TakeOutSomething(leftHandRb);
                }
                if (rightGrab.action.WasReleasedThisFrame())
                {
                    Debug.LogError("rightGrab released");
                    TakeOutSomething(rightHandRb);
                }
            }

        }



        //[ProButton]
        // [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        // private void RPC_TakeOutWeaponhaha(Rigidbody hahaef)
        // {
        //     Debug.LogError("haha name ====" + hahaef.gameObject.name);
        //     Debug.LogError("haha  hash name ====" + hahaef.gameObject.GetHashCode());
        // }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void DealDamageRpc(NetworkId damageID,NetworkObject damageObj)
        {
            // The code inside here will run on the client which owns this object (has state and input authority).
            Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
            //PlayerSpeed -= damage;
            // log the name of the object
            Debug.LogError("damage name ====" + damageID);
            Debug.LogError("damage  hash name ====" + damageObj.GetHashCode());
        }

        private void GrabSomething(Transform HandGrabT, Rigidbody HandRb)
        {
            Collider[] colliders = Physics.OverlapSphere(HandGrabT.position, grabRadius);

            //把rigidboy =  HandRb的collider从数组中去掉
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Rigidbody>() == HandRb)
                {
                    colliders[i] = null;
                }
            }



            // if (colliders.Length > 0)
            // {
            //     Debug.LogError(colliders[0].gameObject.name);
            //     FixedJoint fixedJoint = HandRb.gameObject.AddComponent<FixedJoint>();
            //     fixedJoint.connectedBody = colliders[0].GetComponent<Rigidbody>();
            // }


            // when collider's rigidbody is not null,add a fixedjoint to the collider in the array,use loop
            foreach (Collider collider in colliders)
            {
                if (collider != null && collider.GetComponent<Rigidbody>() != null)
                {
                    Debug.LogError(collider.gameObject.name);
                    FixedJoint fixedJoint = HandRb.gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = collider.GetComponent<Rigidbody>();
                }
            }
        }

        private void TakeOutSomething(Rigidbody HandRb)
        {
            FixedJoint[] fixedJoints = HandRb.GetComponents<FixedJoint>();
            foreach (FixedJoint fixedJoint in fixedJoints)
            {
                fixedJoint.connectedBody = null;
                Destroy(fixedJoint);
            }
        }





        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(leftHandGrabT.position, grabRadius);
            Gizmos.DrawWireSphere(rightHandGrabT.position, grabRadius);
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
                //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * bodySpeed;
                Vector3 move = new Vector3(leftStick.action.ReadValue<Vector2>().x, 0, leftStick.action.ReadValue<Vector2>().y) * Runner.DeltaTime * bodySpeed;
                move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * bodySpeed;
                // 做一个新的向量，x轴是headDirection的欧拉角的0，y轴是
                Vector3 newVector = new Vector3(0, headDirection.eulerAngles.y, 0);
                //把newVector转换成四元数
                Quaternion newQuaternion = Quaternion.Euler(newVector);
                move = newQuaternion * move;
                if (move != Vector3.zero)
                {
                    body.velocity = move;
                }
                else
                {
                    //body.velocity = Vector3.zero;
                }

                Vector3 move2 = new Vector3(rightStick.action.ReadValue<Vector2>().x, 0, rightStick.action.ReadValue<Vector2>().y) * Runner.DeltaTime * bodySpeed;
                move2 = newQuaternion * move2;
                if (move2 != Vector3.zero)
                {
                    XRrig.transform.position += move2 * 0.05f;
                }
                else
                {
                    //XRrig.transform.position += Vector3.zero;
                }


            }




        }
    }

}