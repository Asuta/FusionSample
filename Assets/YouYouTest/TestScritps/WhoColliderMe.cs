using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoColliderMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        //log the name of the object that collided with me
        Debug.LogError(other.gameObject.name);   
        //make the object that collided with me a child of me
        //other.gameObject.transform.parent = transform;
        
        //log the name of the object'parent that I collided with
        Debug.LogError(other.gameObject.transform.parent.name);
    }
}
