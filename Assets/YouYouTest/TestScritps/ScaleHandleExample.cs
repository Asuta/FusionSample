using UnityEngine;
using UnityEditor;

public class ScaleHandleExample : MonoBehaviour
{
    public float scale = 1f;
    public Vector3 centerPoint = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public float handleSize = 1f;

    private void OnDrawGizmos()
    {
        scale = Handles.ScaleValueHandle(scale, centerPoint, rotation, handleSize, Handles.CubeHandleCap, 0.5f);
    }
}