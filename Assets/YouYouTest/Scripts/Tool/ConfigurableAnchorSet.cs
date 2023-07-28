using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class ConfigurableAnchorSet : MonoBehaviour
{
    public Rigidbody anchorRb;
    public Transform selfAnchorPosition;
    public Vector3 selfAnchorPositionV3;
    public Transform connectedAnchorPosition;
    public Vector3 connectedAnchorPositionV3;
    private ConfigurableJoint joint;
    private Transform thisT;
    public Transform targetRotation;
    private Rigidbody selfRb;
    // Start is called before the first frame update


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        selfRb = gameObject.GetComponent<Rigidbody>();
        thisT = transform;
        thisT.rotation = targetRotation.rotation;
    }

    void Start()
    {

    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {

        SetJointAnchor(anchorRb, selfAnchorPosition, connectedAnchorPosition);
    }

    [ProPlayButton]
    private void ButtonToSetJointAnchor()
    {
        SetJointAnchor(anchorRb, selfAnchorPosition, connectedAnchorPosition);
    }

    private void SetJointAnchor(Rigidbody anchorRb, Transform selfAnchorPosition, Transform connectedAnchorPosition)
    {
        selfRb.isKinematic = true;
        thisT.rotation = targetRotation.rotation;
        joint = gameObject.GetComponent<ConfigurableJoint>();
        joint.connectedBody = anchorRb;
        joint.autoConfigureConnectedAnchor = false;
        //selfAnchorPosition.position = connectedAnchorPosition.position;
        Vector3 distance = connectedAnchorPosition.position - selfAnchorPosition.position;
        thisT.position = thisT.position + distance;
        joint.anchor = selfAnchorPosition.localPosition;
        joint.connectedAnchor = connectedAnchorPosition.localPosition;

        // lockeck all axis and montion
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        selfAnchorPositionV3 = selfAnchorPosition.localPosition;
        connectedAnchorPositionV3 = connectedAnchorPosition.localPosition;
        selfRb.isKinematic = false;
        Debug.Log("SetJointAnchor");

    }





    // Update is called once per frame
    void Update()
    {
        //todoo:取消A键判断这个东西   这个只是为了测试（已经有一个按钮了）
        if(Input.GetKeyDown(KeyCode.A))
        {
            SetJointAnchor(anchorRb, selfAnchorPosition, connectedAnchorPosition);
        }
    }



}
