using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class VelocityMove : NetworkBehaviour
{
    public Transform moveTarget;
    public float velocityNum;
    private Rigidbody thisRb;



    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotfrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;
    // [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Transform target;
    [Space]
    [Header("Springs")]
    [SerializeField] float climbForce = 1000f;
    [SerializeField] float climbDrag = 500f;
    Vector3 _previousPosition;
    Rigidbody _rigidbody;
    bool _isColliding;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void FixedUpdate()
    {
        //thisRb.velocity = (moveTarget.position - transform.position).normalized * velocityNum;
        if (HasStateAuthority == false)
        {
            PIDMovement();
        }

    }

    void PIDMovement()
    {
        
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (-_rigidbody.velocity) * kdg; ;
        _rigidbody.AddForce(force, ForceMode.Acceleration);
        
    }
}
