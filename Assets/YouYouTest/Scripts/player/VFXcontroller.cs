using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Fusion106;


public class VFXHurtEvent : UnityEvent<Vector3, float> { }

public class VFXcontroller : MonoBehaviour
{
    public ColliderHurt[] colliderHurt;
    public ParticleSystem bloodParticle;
    public PhysxBall physxBall;

    private void Awake()
    {
        physxBall.vfxHurtEvent.AddListener(OnGetHurt);
    }

    private void OnGetHurt(Vector3 arg0, float arg1)
    {
        //播放粒子特效
        bloodParticle.transform.position = arg0;
        bloodParticle.Play();
        //Debug.LogError("冒血了" + arg1);
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
