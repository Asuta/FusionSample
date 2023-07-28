using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKuMove : MonoBehaviour
{
    public Transform bodyT;
    public Vector3 offset;
    private Transform thisT;
    // Start is called before the first frame update
    void Start()
    {
        thisT = GetComponent<Transform>();
        //foreach all of chilldern to deactive
        for (int i = 0; i < thisT.childCount; i++)
        {
            thisT.GetChild(i).gameObject.transform.localPosition = Vector3.zero;
            thisT.GetChild(i).gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        thisT.position = bodyT.position + offset;
    }
}
