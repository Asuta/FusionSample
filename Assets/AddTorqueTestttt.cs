using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorqueTestttt : MonoBehaviour
{
    public Transform targetTransform;
    public float torque = 10f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 计算刚体应该旋转到的朝向
        Quaternion targetRotation = targetTransform.rotation;
        // 计算刚体目前与目标角度之间的差距
        Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);
        // 根据差距计算出需要施加的扭矩
        Vector3 torqueVector =
            new Vector3(deltaRotation.x, deltaRotation.y, deltaRotation.z) * torque;
        // 施加扭矩
        rb.AddTorque(torqueVector, ForceMode.Force);
    }
}
