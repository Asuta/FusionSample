using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;
using System;

public class WeaponFather : MonoBehaviour
{
    public bool isIgnore = true;
    public Transform[] ignoreObjects;
    private List<Rigidbody> ignoreRigids;
    private List<Collider> ignoreColliders;
    public Transform[] ignoreSelfObjects;
    private List<Rigidbody> ignoreSelfRigids;
    private List<Collider> ignoreSelfColliders;
    //----------------------test----------------------
    public float numberofIgnoreRigids;
    public float numberofIgnoreSelfRigids;

    // Start is called before the first frame update
    void Start()
    {
        AllSet();
    }


    [ProPlayButton]
    public void AllSet()
    {
        SetIgnoreRigids();
        SetSelfIgnoreRigids();
        SetIgnore(isIgnore);
    }

    //让每个ignoreRigids中的rigidbody对ignoreSelfRigids的所有rigidbody进行忽略
    private void SetIgnore(bool v)
    {

        // for (int i = 0; i < numberofIgnoreRigids; i++)
        // {
        //     for (int j = 0; j < numberofIgnoreSelfRigids; j++)
        //     {
        //         Physics.IgnoreCollision(ignoreRigids[i].GetComponent<Collider>(), ignoreSelfRigids[j].GetComponent<Collider>(), v);
        //     }
        // }

        for (int i = 0; i < ignoreColliders.Count; i++)
        {
            for (int j = 0; j < ignoreSelfColliders.Count; j++)
            {
                Physics.IgnoreCollision(ignoreColliders[i], ignoreSelfColliders[j], v);
            }
        }

    }

    public void SetIgnoreRigids()
    {
        ignoreRigids = new List<Rigidbody>();
        ignoreColliders = new List<Collider>();
        for (int i = 0; i < ignoreObjects.Length; i++)
        {
            Rigidbody[] rigids = ignoreObjects[i].GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigid in rigids)
            {
                ignoreRigids.Add(rigid);
            }

            Collider[] colliders = ignoreObjects[i].GetComponentsInChildren<Collider>();
            Debug.Log("colliders.Length:" + colliders.Length);
            foreach (Collider collider in colliders)
            {
                ignoreColliders.Add(collider);
            }
        }
        numberofIgnoreRigids = ignoreRigids.Count;
    }

    public void SetSelfIgnoreRigids()
    {
        ignoreSelfRigids = new List<Rigidbody>();
        ignoreSelfColliders = new List<Collider>();
        for (int i = 0; i < ignoreSelfObjects.Length; i++)
        {
            Rigidbody[] rigids = ignoreSelfObjects[i].GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigid in rigids)
            {
                ignoreSelfRigids.Add(rigid);
            }

            Collider[] colliders = ignoreSelfObjects[i].GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                ignoreSelfColliders.Add(collider);
            }
        }
        numberofIgnoreSelfRigids = ignoreSelfRigids.Count;
    }


}
