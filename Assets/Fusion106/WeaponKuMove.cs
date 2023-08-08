using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class WeaponKuMove : NetworkBehaviour
{
    public Transform bodyT;
    public Vector3 offset;
    private Transform thisT;
    [Header("isLog?")]
    public bool isLog;

    private void Awake()
    {

        thisT = GetComponent<Transform>();
        //foreach all of chilldern to deactive
        for (int i = 0; i < thisT.childCount; i++)
        {

            thisT.GetChild(i).gameObject.SetActive(false);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        thisT = GetComponent<Transform>();
        //foreach all of chilldern to deactive
        for (int i = 0; i < thisT.childCount; i++)
        {
            thisT.GetChild(i).gameObject.transform.localPosition = Vector3.zero;

        }



    }

    private void Update()
    {
        thisT.position = bodyT.position + offset;
        Debug.LogError("thisT.position = ooooooooooooooooooo");
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        thisT.position = bodyT.position + offset;
        // if (isLog)
        // {
        //     Debug.LogError("thisT.position = " + thisT.position);
        // }

    }
}
