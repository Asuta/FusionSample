using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class GetColliderTest : MonoBehaviour
{
    public Transform getTarget;
    public int colliderCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ProButton]
    public void GetCollider()
    {
        Collider[] colliders = getTarget.GetComponentsInChildren<Collider>();
        colliderCount = colliders.Length;
    }
}
