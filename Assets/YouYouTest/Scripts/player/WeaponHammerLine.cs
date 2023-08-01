using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponHammerLine : MonoBehaviour
{
    public Transform lineStart;
    public Transform lineEnd;
    public LineRenderer lineRenderer;
    public float lineWidth = 0.1f;
    //define the color 
    public Color c1 = Color.yellow;

    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, lineStart.position);
        lineRenderer.SetPosition(1, lineEnd.position);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.startColor = c1;
    }
}
