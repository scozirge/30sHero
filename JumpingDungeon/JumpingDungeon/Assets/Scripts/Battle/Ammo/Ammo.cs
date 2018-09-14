using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public partial class Ammo : MonoBehaviour
{
    [Tooltip("自身跟隨特效物件")]
    [SerializeField]
    ParticleSystem[] LocalParticles;
    [Tooltip("不跟隨的特效物件")]
    [SerializeField]
    ParticleSystem[] GlobalParticle;
    [Tooltip("子彈存活時間(秒)")]
    [SerializeField]
    float LifeTime;
    [Tooltip("子彈擊退力道")]
    [SerializeField]
    protected int KnockIntensity;
    [Tooltip("暈眩秒數")]
    [SerializeField]
    protected float StunIntensity;
    [Tooltip("燃燒秒數")]
    [SerializeField]
    protected float FireIntensity;
    [Tooltip("冰凍秒數")]
    [SerializeField]
    protected float IceIntensity;
    [Tooltip("詛咒秒數")]
    [SerializeField]
    protected float CurseIntensity;
    [Tooltip("子彈類型，選擇穿透就是子彈擊中玩家後不會移除，且可能造成多次傷害(炸彈類的子彈)")]
    [SerializeField]
    protected ShootAmmoType AmmoType;

    protected Force AttackerForce;
    protected Force TargetForce;
    protected bool IsLaunch;
    protected bool IsCausedDamage;
    protected int Damage;
    protected Role Target;
    float LifeTimer;
    protected Rigidbody2D MyRigi;
    protected Transform ParticleParent;
    float DestructMargin_Left;
    float DestructMargin_Right;

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
        DestroyOutSideAmmos();
    }
    public virtual void SelfDestroy()
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
    void DestroyOutSideAmmos()
    {
        DestructMargin_Left = (BattleManage.MyCameraControler.transform.position.x - (BattleManage.ScreenSize.x / 2 + 100));
        DestructMargin_Right = (BattleManage.MyCameraControler.transform.position.x + (BattleManage.ScreenSize.x / 2 + 100));
        if (transform.position.x < DestructMargin_Left ||
            transform.position.x > DestructMargin_Right ||
            transform.position.y > BattleManage.ScreenSize.y / 2 + 100 ||
            transform.position.y < -(BattleManage.ScreenSize.y / 2 + 100)
            )
        {
            SelfDestroy();
        }
    }
}
