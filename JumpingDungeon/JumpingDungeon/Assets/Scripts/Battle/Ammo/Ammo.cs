using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public partial class Ammo : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] LocalParticles;
    [SerializeField]
    ParticleSystem[] GlobalParticle;
    [SerializeField]
    float LifeTime;
    [SerializeField]
    protected int KnockIntensity;
    [SerializeField]
    protected float StunIntensity;
    [SerializeField]
    protected float FireIntensity;
    [SerializeField]
    protected float IceIntensity;

    protected Force AttackerForce;
    protected Force TargetForce;
    protected bool IsLaunch;
    protected int Damage;
    protected Role Target;
    float LifeTimer;
    protected Rigidbody2D MyRigi;
    protected Transform ParticleParent;

    public virtual void Init(Dictionary<string, object> _dic)
    {
        ParticleParent = GameObject.FindGameObjectWithTag("ParticleParent").transform;
        MyRigi = GetComponent<Rigidbody2D>();
        LifeTimer = LifeTime;
        AttackerForce = ((Force)(_dic["AttackerForce"]));
        if (AttackerForce == Force.Player)
            TargetForce = Force.Enemy;
        else
            TargetForce = Force.Player;
        Damage = int.Parse(_dic["Damage"].ToString());
        SpawnParticles();
    }
    protected virtual void SpawnParticles()
    {
        for (int i = 0; i < LocalParticles.Length; i++)
        {
            if (LocalParticles[i] == null)
                continue;
            EffectEmitter.EmitParticle(LocalParticles[i], Vector3.zero, Vector3.zero, transform);
        }
        for (int i = 0; i < GlobalParticle.Length; i++)
        {
            if (GlobalParticle[i] == null)
                continue;
            EffectEmitter.EmitParticle(GlobalParticle[i], transform.position, Vector3.zero, ParticleParent);
        }
    }
    public virtual void Launch()
    {
        IsLaunch = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.tag.ToString() == TargetForce.ToString())
        {
            TriggerTarget(_col.GetComponent<Role>());
        }
    }
    protected virtual void TriggerTarget(Role _curTarget)
    {
    }
    protected virtual void OnTriggerStay2D(Collider2D _col)
    {
        string tagName = _col.tag.ToString();
        if (tagName == TargetForce.ToString())
        {
            TriggerTarget(_col.GetComponent<Role>());
        }
    }
    protected virtual void Update()
    {
        if (!IsLaunch)
            return;
        LIfeTimerFunc();
    }
    protected virtual void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
    protected virtual void LIfeTimerFunc()
    {
        if (LifeTimer > 0)
            LifeTimer -= Time.deltaTime;
        else
            SelfDestroy();
    }
}
