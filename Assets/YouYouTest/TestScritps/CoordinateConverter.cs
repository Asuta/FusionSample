using UnityEngine;

public class CoordinateConverter : MonoBehaviour
{
    public Transform childTransform; // 子物体的Transform组件
    public Vector3 localCoordinate;  // 子物体的本地坐标
    public Transform moveTarget;

    void Start()
    {

    }





    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector3 worldCoordinate = transform.TransformPoint(childTransform.localPosition);
        Debug.Log("世界坐标: " + worldCoordinate);



        moveTarget.position = worldCoordinate;
    }
}