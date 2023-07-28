using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestLog : MonoBehaviour
{
    private Rigidbody thisRb;

    // Start is called before the first frame update
    void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() { 


    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Collision Enter"+other.gameObject.name);
    }
    private void OnCollisionStay(Collision other) {
        Debug.Log("Collision Stay"+other.gameObject.name);
        
    }
    private void OnCollisionExit(Collision other) {
        Debug.Log("Collision Exit"+other.gameObject.name);
        
    }
}
