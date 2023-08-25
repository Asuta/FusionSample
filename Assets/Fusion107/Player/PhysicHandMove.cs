using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicHandMove : MonoBehaviour
{
    private Transform thisT;
    public Transform bodyT; // 身体的Transform
    public Vector3 leftShoulderPosition;  // 左肩的世界坐标
    public Transform leftArmDirection; // 手臂的方向
    public TransferFunction leftForearmDirection; // 前臂的方向
    public TransferFunction leftHandDirection; // 手的方向

    public Vector3 rightShoulderPosition; // 右肩的世界坐标
    public Transform rightArmDirection; // 手臂的方向
    public TransferFunction rightForearmDirection; // 前臂的方向
    public TransferFunction rightHandDirection; // 手的方向
    [Header("test")]
    public Transform visualObj;      
    
    
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

    }
}
