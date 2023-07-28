using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromCenter : MonoBehaviour
{
    public bool autoMove;
    public Transform positionTarget;
    public Transform moveCenter;
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

        if (InputTest.Instance.inputActions.XRIRightHandInteraction.ButtonB.WasPressedThisFrame())
        {
            autoMove = !autoMove;
        }

        if (autoMove)
        {
            //通过移动自身位置（thisT），让moveCenter的移动到positionTarget的位置
            thisT.position += positionTarget.position - moveCenter.position;
        }
        else
        {
            if (InputTest.Instance.inputActions.XRIRightHandInteraction.ButtonA.inProgress)
            {
                //通过移动自身位置（thisT），让moveCenter的移动到positionTarget的位置
                thisT.position += positionTarget.position - moveCenter.position;
            }
        }



    }
}
