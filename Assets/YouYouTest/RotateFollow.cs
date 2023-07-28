using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFollo : MonoBehaviour
{
    public Transform positionTarget;
    public Transform rotationTarget;

    private Transform thisT;


    private void Awake()
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
        //让自己的位置和旋转跟随目标
        thisT.position = positionTarget.position;
        //让自己的旋转跟随目标，不过只跟随Y轴
        thisT.rotation = Quaternion.Euler(0, rotationTarget.eulerAngles.y, 0);
    }
}
