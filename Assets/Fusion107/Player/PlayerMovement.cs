using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.EventSystems;

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
        public Rigidbody leftHandRb;
        public Transform leftHandGrabT;
        public Rigidbody rightHandRb;
        public Transform rightHandGrabT;



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

            Debug.LogError("leftStick: " + leftStick.action.ReadValue<Vector2>());
            Debug.LogError("rightStick: " + rightStick.action.ReadValue<Vector2>());


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
            }


            //在leftHandGrabT的位置画一个球形线框，只需要在scene里看到，不需要在build里看到
            Debug.DrawLine(leftHandGrabT.position, leftHandGrabT.position + leftHandGrabT.forward * 0.1f, Color.red);

            GenerateSphere();


        }

        private void GenerateSphere()
        {
            int resolution = 8; // 球体的分辨率，可以根据需要调整

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    float u = (float)i / (resolution - 1);
                    float v = (float)j / (resolution - 1);

                    float theta = u * Mathf.PI * 2;
                    float phi = v * Mathf.PI;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 point = new Vector3(x, y, z);
                    Vector3 nextPointU = new Vector3(Mathf.Sin(phi) * Mathf.Cos(theta + Mathf.PI / resolution), Mathf.Cos(phi), Mathf.Sin(phi) * Mathf.Sin(theta + Mathf.PI / resolution));
                    Vector3 nextPointV = new Vector3(Mathf.Sin(phi + Mathf.PI / resolution) * Mathf.Cos(theta), Mathf.Cos(phi + Mathf.PI / resolution), Mathf.Sin(phi + Mathf.PI / resolution) * Mathf.Sin(theta));

                    Debug.DrawLine(point, nextPointU, Color.white);
                    Debug.DrawLine(point, nextPointV, Color.white);
                }
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