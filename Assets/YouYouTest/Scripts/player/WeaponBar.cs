using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBar : MonoBehaviour
{
    public Transform gravityCenter;
    private Rigidbody selfRb;
    

    private void Awake() {
        selfRb = GetComponent<Rigidbody>();
        selfRb.centerOfMass = gravityCenter.localPosition;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
