using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicHandMove : MonoBehaviour
{
    private Transform thisT;
    public Transform bodyT; // 身体的Transform
    public Vector3 leftShoulderPosition;  // 左肩的世界坐标
    public Transform leftArmDirection; // 手臂的方向
    public Transform leftForearmDirection; // 前臂的方向
    public Transform leftHandDirection; // 手的方向
    public Transform leftArmRigid; // 手臂的刚体
    public Transform leftForearmRigid; // 前臂的刚体
    public Transform leftHandRigid; // 手的刚体

    public Vector3 rightShoulderPosition; // 右肩的世界坐标
    public Transform rightArmDirection; // 手臂的方向
    public Transform rightForearmDirection; // 前臂的方向
    public Transform rightHandDirection; // 手的方向
    public Transform rightArmRigid; // 手臂的刚体
    public Transform rightForearmRigid; // 前臂的刚体
    public Transform rightHandRigid; // 手的刚体

    [Header("test")]
    public Transform visualObj;
    public float length;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        thisT = transform;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 worldPosition =  bodyT.TransformPoint(leftShoulderPosition);
        //visualObj.position = worldPosition;

        Vector3 worldPosition = bodyT.TransformPoint(leftShoulderPosition);
        worldPosition = worldPosition + leftArmDirection.rotation * Vector3.right * leftArmRigid.localScale.x + leftForearmDirection.rotation * Vector3.right * leftForearmRigid.localScale.x + leftHandDirection.rotation * Vector3.right * leftHandRigid.localScale.x;
        visualObj.position = worldPosition;

    }
}
