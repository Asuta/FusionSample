using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class UpDown : MonoBehaviour
{
    public float amplitude = 0.5f; // 振幅
    public float frequency = 1f; // 频率

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // 计算浮动的值
        float floatY = startPos.y + amplitude * Mathf.Sin(frequency * Time.time);

        // 更新物体的位置
        transform.position = new Vector3(transform.position.x, floatY, transform.position.z);
    }
}
