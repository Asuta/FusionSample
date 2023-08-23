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
        public ConfigurableJoint leftHandJoint;
        //public NetworkObject leftHandNetworkObject;
        public Transform leftHandGrabT;

        public Rigidbody rightHandRb;
        public ConfigurableJoint rightHandJoint;
        //public NetworkObject rightHandNetworkObject;
        public Transform rightHandGrabT;

        public GameObject[] unGrabableObjects;




        [Header("hand Grab  Sync")]
        private float hahah;

        [Networked(OnChanged = nameof(OnHandGrabChanged))]
        public NetworkObject LeftHandGrabbedObject { get; set; }
        [Networked(OnChanged = nameof(OnHandGrabChanged))]
        public NetworkObject RightHandGrabbedObject { get; set; }

        [Header("test")]
        public string testString;
        public int testInt;
        public NetworkObject testNetObj;




        private static void OnHandGrabChanged(Changed<PlayerMovement> obj)
        {
            Debug.LogError("OnHandGrabChanged  name = " + obj.Behaviour.LeftHandGrabbedObject.name);
        }




        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public override void Spawned()
        {

            // 找到名为“Floor”的游戏对象，并获取其碰撞器以赋值给planeCollider
            planeCollider = GameObject.Find("Floor").GetComponent<Collider>();
            XRrig.SetActive(HasStateAuthority);
            // 如果没有状态权限，则忽略bodyCollider和planeCollider之间的碰撞
            if (HasStateAuthority == false)
            {
                Physics.IgnoreCollision(bodyCollider, planeCollider);
                body.useGravity = false;
                // 将allBodys中的所有刚体的useGravity属性设置为false
                foreach (Rigidbody rb in allBodys)
                {
                    rb.useGravity = false;
                }
            }

            if (HasStateAuthority)
            {
                // 将allBodys中的所有刚体的isKinematic属性设置为true，1秒后设置为false
                foreach (Rigidbody rb in allBodys)
                {
                    rb.isKinematic = true;
                }
                StartCoroutine(SetKinematic());

                //foreach (GameObject obj in unGrabableObjects),and set all of this and there children's tag to "unGrabable"
                foreach (GameObject obj in unGrabableObjects)
                {
                    obj.tag = "unGrabable";
                    foreach (Transform child in obj.transform)
                    {
                        child.tag = "unGrabable";
                    }
                }
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
                    GrabSomething(leftHandGrabT, leftHandRb,1);
                }
                if (rightGrab.action.triggered)
                {
                    GrabSomething(rightHandGrabT, rightHandRb,2);
                }

                if (leftGrab.action.WasReleasedThisFrame())
                {
                    Debug.LogError("leftGrab released");
                    //remove the fixedjoint
                    TakeOutSomething(leftHandRb,1);
                }
                if (rightGrab.action.WasReleasedThisFrame())
                {
                    Debug.LogError("rightGrab released");
                    TakeOutSomething(rightHandRb,2);
                }
            }


            //string rpc test
            if (Input.GetKeyDown(KeyCode.T))
            {
                RPC_SendMessage(testString);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RPC_SendMessage2(testInt);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                RPC_SendMessage3(testNetObj);
            }




        }


        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SendMessage(string message, RpcInfo info = default)
        {
            Debug.LogError("RPC_SendMessage  message = " + message);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SendMessage2(int message, RpcInfo info = default)
        {
            Debug.LogError("RPC_SendMessage  message = " + message);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_SendMessage3(NetworkObject haha, RpcInfo info = default)
        {
            //log haha gameobject name
            Debug.LogError("hahhahahah  name = " + haha.transform.name);
        }



        //[ProButton]
        // [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        // private void RPC_TakeOutWeaponhaha(Rigidbody hahaef)
        // {
        //     Debug.LogError("haha name ====" + hahaef.gameObject.name);
        //     Debug.LogError("haha  hash name ====" + hahaef.gameObject.GetHashCode());
        // }


        private void GrabSomething(Transform HandGrabT, Rigidbody HandRb,int whichHand)
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

            // when collider's rigidbody is not null,add a fixedjoint to the collider in the array,use loop
            foreach (Collider collider in colliders)
            {
                if (collider != null && collider.GetComponent<Rigidbody>() != null && collider.tag != "unGrabable")
                {
                    Debug.LogError(collider.gameObject.name);
                    ConfigurableJoint fixedJoint = HandRb.gameObject.AddComponent<ConfigurableJoint>();
                    fixedJoint.connectedBody = collider.GetComponent<Rigidbody>();
                    //set the joint 's all move and rotation to lock
                    fixedJoint.xMotion = ConfigurableJointMotion.Locked;
                    fixedJoint.yMotion = ConfigurableJointMotion.Locked;
                    fixedJoint.zMotion = ConfigurableJointMotion.Locked;
                    fixedJoint.angularXMotion = ConfigurableJointMotion.Locked;
                    fixedJoint.angularYMotion = ConfigurableJointMotion.Locked;
                    fixedJoint.angularZMotion = ConfigurableJointMotion.Locked;
                    //RPC_Grab(collider.GetComponent<NetworkObject>(), HandRb.GetComponent<NetworkObject>(), fixedJoint.anchor, fixedJoint.connectedAnchor);
                    RPC_Grab2(collider.GetComponent<NetworkObject>(), whichHand, fixedJoint.anchor, fixedJoint.connectedAnchor);
                    //结束这个循环
                    break;
                }
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Grab(NetworkObject grabThing,int WhickHand, Vector3 anchorPosition, Vector3 connectedAnchorPosition, RpcInfo info = default)
        {
            if (WhickHand == 1)
            {
                Debug.LogError("leftHandNetworkObject");
            }

            if (WhickHand == 2)
            {
                Debug.LogError("rightHandNetworkObject");
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Grab2(NetworkObject grabThing,int WhickHand, Vector3 anchorPosition, Vector3 connectedAnchorPosition, RpcInfo info = default)
        {
            if (HasStateAuthority)
                return;

            if (WhickHand == 1)
            {
                Debug.LogError("leftHandNetworkObject");
                leftHandJoint.connectedBody = grabThing.GetComponent<Rigidbody>();
                leftHandJoint.anchor = anchorPosition;
                leftHandJoint.connectedAnchor = connectedAnchorPosition;
                // set move and rotation to lock
                leftHandJoint.xMotion = ConfigurableJointMotion.Locked;
                leftHandJoint.yMotion = ConfigurableJointMotion.Locked;
                leftHandJoint.zMotion = ConfigurableJointMotion.Locked;
                leftHandJoint.angularXMotion = ConfigurableJointMotion.Locked;
                leftHandJoint.angularYMotion = ConfigurableJointMotion.Locked;
                leftHandJoint.angularZMotion = ConfigurableJointMotion.Locked;
            }

            if (WhickHand == 2)
            {
                Debug.LogError("rightHandNetworkObject");
                rightHandJoint.connectedBody = grabThing.GetComponent<Rigidbody>();
                rightHandJoint.anchor = anchorPosition;
                rightHandJoint.connectedAnchor = connectedAnchorPosition;
                // set move and rotation to lock
                rightHandJoint.xMotion = ConfigurableJointMotion.Locked;
                rightHandJoint.yMotion = ConfigurableJointMotion.Locked;
                rightHandJoint.zMotion = ConfigurableJointMotion.Locked;
                rightHandJoint.angularXMotion = ConfigurableJointMotion.Locked;
                rightHandJoint.angularYMotion = ConfigurableJointMotion.Locked;
                rightHandJoint.angularZMotion = ConfigurableJointMotion.Locked;
            }
        }




        private void TakeOutSomething(Rigidbody HandRb,int WhickHand)
        {
            ConfigurableJoint[] fixedJoints = HandRb.GetComponents<ConfigurableJoint>();
            foreach (ConfigurableJoint fixedJoint in fixedJoints)
            {
                fixedJoint.connectedBody = null;
                Destroy(fixedJoint);
            }
            RPC_TakeOutSometing2(WhickHand);
        }

        // [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        // public void RPC_TakeOutSometing(NetworkObject hand, RpcInfo info = default)
        // {
        //     if (!HasStateAuthority)
        //     {
        //         ConfigurableJoint[] fixedJoints = hand.GetComponents<ConfigurableJoint>();
        //         foreach (ConfigurableJoint fixedJoint in fixedJoints)
        //         {
        //             fixedJoint.connectedBody = null;
        //             Destroy(fixedJoint);
        //         }
        //     }
        // }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_TakeOutSometing2(int whichHand, RpcInfo info = default)
        {
            if (HasStateAuthority)
                return;

            if (whichHand == 1)
            {
                Debug.LogError("leftHandNetworkObject");
                leftHandJoint.connectedBody = null;
                //set move and rotation to free
                leftHandJoint.xMotion = ConfigurableJointMotion.Free;
                leftHandJoint.yMotion = ConfigurableJointMotion.Free;
                leftHandJoint.zMotion = ConfigurableJointMotion.Free;
                leftHandJoint.angularXMotion = ConfigurableJointMotion.Free;
                leftHandJoint.angularYMotion = ConfigurableJointMotion.Free;
                leftHandJoint.angularZMotion = ConfigurableJointMotion.Free;
            }

            if (whichHand == 2)
            {
                Debug.LogError("rightHandNetworkObject");
                rightHandJoint.connectedBody = null;
                //set move and rotation to free
                rightHandJoint.xMotion = ConfigurableJointMotion.Free;
                rightHandJoint.yMotion = ConfigurableJointMotion.Free;
                rightHandJoint.zMotion = ConfigurableJointMotion.Free;
                rightHandJoint.angularXMotion = ConfigurableJointMotion.Free;
                rightHandJoint.angularYMotion = ConfigurableJointMotion.Free;
                rightHandJoint.angularZMotion = ConfigurableJointMotion.Free;
            }
        }





        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(leftHandGrabT.position, grabRadius);
            Gizmos.DrawWireSphere(rightHandGrabT.position, grabRadius);
        }



        public override void FixedUpdateNetwork()
        {

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