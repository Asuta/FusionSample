using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PummyMove : MonoBehaviour
{
    public Transform targetT;
    public float height = 1f;
    private Transform thisT;

    // Start is called before the first frame update
    void Start()
    {
        thisT = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // //让thisT的高度一直的等于targetT的高度减去height
        // thisT.position = new Vector3(
        //     thisT.position.x,
        //     targetT.position.y - height,
        //     thisT.position.z
        // );

        //让thisT的高度一直的等于targetT的高度减去height
        thisT.position = new Vector3(
            targetT.position.x,
            targetT.position.y - height,
            targetT.position.z
        );
    }
}
