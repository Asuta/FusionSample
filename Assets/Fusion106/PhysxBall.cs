using Fusion;
using UnityEngine;
using System;
using Unity.Mathematics;

namespace Fusion106
{
    public static class MyStaticClass
    {
        public static Quaternion Variable1 { get; set; }

        public static Vector3 bodyTorque;
        public static Vector3 headTorque;
        public static Vector3 leftArmTorque;
        public static Vector3 rightArmTorque;
        public static Vector3 leftForearmTorque;
        public static Vector3 rightForearmTorque;
        public static Vector3 leftHandTorque;
        public static Vector3 rightHandTorque;
        public static Vector3 headDirection;

    }

    public class Vector3Proxy
    {
        private Func<Vector3> getter;
        private Action<Vector3> setter;

        public Vector3Proxy(Func<Vector3> getter, Action<Vector3> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public Vector3 Value
        {
            get { return getter(); }
            set { setter(value); }
        }
    }

    public class PhysxBall : NetworkBehaviour
    {
        [Networked]
        private TickTimer life { get; set; }

        private Rigidbody selfRb;
        public float jumpForce = 30f;

        public Transform targetT;
        public Transform[] targetTs;
        public Rigidbody[] rigidbodies;
        public Transform bodyGroup;
        public Transform XRGroup;
        public Transform pillarsGroup;
        public Transform headDirection;


        private Vector3[] torqueRecordOut = new Vector3[8];

        public float torque = 100f;
        public float slowDownAngularVelocity = 0.9f;
        public float maxRotationChange = 0.2f;
        //define a list of transform
        public Transform[] weaponList;
        private int nowWeapon;

        //------------------------test------------------------
        // public Rigidbody[] cube1;
        // public Rigidbody cube2;
        // public float torqueCube = 100f;

        private void Awake()
        {
            // each of weaponList to deactive
            for (int i = 0; i < weaponList.Length; i++)
            {
                weaponList[i].gameObject.SetActive(false);
            }
        }

        public void Init(Vector3 forward)
        {

        }


        private void Update()
        {
            pillarsGroup.gameObject.SetActive(false);

            if (!Object.HasInputAuthority)
            {
                //deactive the XRGroup.
                XRGroup.gameObject.SetActive(false);
                return;
            }


            pillarsGroup.gameObject.SetActive(true);

            //用右摇杆让XRgroup的Y轴旋转,并且是以rigidbody[0]为中心旋转
            var rightDirection = InputTest.Instance.inputActions.XRIRightHandInteraction.StickVector2.ReadValue<Vector2>();
            //XRGroup.Rotate(0, rightDirection.x * 100f * Time.deltaTime, 0);
            XRGroup.RotateAround(rigidbodies[0].transform.position, Vector3.up, rightDirection.x * 100f * Time.deltaTime);


            if (InputTest.Instance.inputActions.XRILeftHandInteraction.ButtonY.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.R))
            {
                RPC_ResetBodyGroup();
            }

            //每按一次X，就激活下一个武器，同时把其他武器都deactive
            if (InputTest.Instance.inputActions.XRILeftHandInteraction.ButtonX.WasPressedThisFrame()||Input.GetKeyDown(KeyCode.A))
            {
                RPC_TakeOutWeapon();
            }
        }


        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_ResetBodyGroup(RpcInfo info = default)
        {
            Vector3 distance = Vector3.zero - rigidbodies[0].transform.position;
            bodyGroup.position += distance;
            //take all the rigidbodies from the bodyGroup to null parent
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].transform.parent = null;
            }
            bodyGroup.position = Vector3.zero;
            //take all the rigidbodies from null parent to the bodyGroup
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].transform.parent = bodyGroup;
            }
            //reset all the rigidbodies' velocity
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].velocity = Vector3.zero;
                rigidbodies[i].angularVelocity = Vector3.zero;
            }
        }



        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_TakeOutWeapon(RpcInfo info = default)
        {
            //每按一次X，就激活下一个武器，同时把其他武器都deactive
            weaponList[nowWeapon].gameObject.SetActive(false);
            nowWeapon++;
            if (nowWeapon >= weaponList.Length)
            {
                nowWeapon = 0;
            }
            weaponList[nowWeapon].gameObject.SetActive(true);
        }


        private void FixedUpdate()
        {
            if (!Object.HasInputAuthority)
            {
                //deactive the XRGroup.
                XRGroup.gameObject.SetActive(false);
                return;
            }

            Vector3Proxy[] array = new Vector3Proxy[]
            {
                // Add torque to the body
                new Vector3Proxy(
                    () => MyStaticClass.bodyTorque,
                    v => MyStaticClass.bodyTorque = v
                ),
                // Add torque to the head
                new Vector3Proxy(
                    () => MyStaticClass.headTorque,
                    v => MyStaticClass.headTorque = v
                ),
                // Add torque to the left arm
                new Vector3Proxy(
                    () => MyStaticClass.leftArmTorque,
                    v => MyStaticClass.leftArmTorque = v
                ),
                // Add torque to the right arm
                new Vector3Proxy(
                    () => MyStaticClass.rightArmTorque,
                    v => MyStaticClass.rightArmTorque = v
                ),
                // Add torque to the left forearm
                new Vector3Proxy(
                    () => MyStaticClass.leftForearmTorque,
                    v => MyStaticClass.leftForearmTorque = v
                ),
                // Add torque to the right forearm
                new Vector3Proxy(
                    () => MyStaticClass.rightForearmTorque,
                    v => MyStaticClass.rightForearmTorque = v
                ),
                // Add torque to the left hand
                new Vector3Proxy(
                    () => MyStaticClass.leftHandTorque,
                    v => MyStaticClass.leftHandTorque = v
                ),
                // Add torque to the right hand
                new Vector3Proxy(
                    () => MyStaticClass.rightHandTorque,
                    v => MyStaticClass.rightHandTorque = v
                ),
                // Add head direction s rotation
                new Vector3Proxy(
                    () => MyStaticClass.headDirection,
                    v => MyStaticClass.headDirection = v
                )
            };



            //遍历所有的刚体对象，同时也遍历所有的目标对象，结对
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                Quaternion difference = targetTs[i].rotation * Quaternion.Inverse(rigidbodies[i].rotation);
                difference.ToAngleAxis(out float angle, out Vector3 axis);

                if (angle > 180)
                {
                    angle -= 360;
                }

                Vector3 targetAngleVelocity = (axis * angle * Mathf.Deg2Rad) / Time.fixedDeltaTime;
                array[i].Value = targetAngleVelocity;
            }

            //找到array中的headDirection，然后赋值给headDirection
            MyStaticClass.headDirection = headDirection.rotation.eulerAngles;


        }


        public override void FixedUpdateNetwork()
        {
            // ---------------------------------test---------------------------------（对全身施力，从input获取数据）（但是会发抖。。。。。）（最简单算法）
            if (GetInput(out NetworkInputData data))
            {

                //get the Y axis of the head direction 
                //Quaternion headDirectionY = Quaternion.Euler(0, headDirection.rotation.eulerAngles.y, 0);
                Quaternion headDirectionY = Quaternion.Euler(0, data.headDirection.y, 0);

                //使用unity输入的横轴和纵轴计算出一个方向向量，控制bodyGroup的前后左右移动
                var leftDirection = headDirectionY * data.directionFromLeftStick;
                bodyGroup.Translate(leftDirection * 0.1f);

                //用右摇杆让XRgroup移动（不用了）
                // var rightDirection = data.directionFromRightStick;
                // XRGroup.Translate(rightDirection * 0.1f);




                if (data.bodyTorque != null)
                {
                    torqueRecordOut[0] = data.bodyTorque;
                    torqueRecordOut[1] = data.headTorque;
                    torqueRecordOut[2] = data.leftArmTorque;
                    torqueRecordOut[3] = data.rightArmTorque;
                    torqueRecordOut[4] = data.leftForearmTorque;
                    torqueRecordOut[5] = data.rightForearmTorque;
                    torqueRecordOut[6] = data.leftHandTorque;
                    torqueRecordOut[7] = data.rightHandTorque;

                    //遍历所有的刚体对象
                    for (int i = 0; i < rigidbodies.Length; i++)
                    {
                        // 施加扭矩
                        rigidbodies[i].angularVelocity *= slowDownAngularVelocity;
                        float maxChange = maxRotationChange * Time.fixedDeltaTime;
                        rigidbodies[i].angularVelocity = Vector3.MoveTowards(rigidbodies[i].angularVelocity, torqueRecordOut[i], maxChange);
                    }
                }
            }
        }
    }
}
