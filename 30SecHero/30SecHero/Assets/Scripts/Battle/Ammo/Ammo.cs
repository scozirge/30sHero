using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Ammo : MonoBehaviour
{
    [Tooltip("自身跟隨特效物件")]
    [SerializeField]
    ParticleSystem[] LocalParticles;
    [Tooltip("不跟隨的特效物件")]
    [SerializeField]
    ParticleSystem[] GlobalParticle;
    [Tooltip("命中特效-自身子彈")]
    [SerializeField]
    protected ParticleSystem[] DeadParticles;
    [Tooltip("命中特效-目標")]
    [SerializeField]
    ParticleSystem HitTargetParticle;
    [Tooltip("子彈存活時間(秒)")]
    [SerializeField]
    float LifeTime;
    [Tooltip("子彈擊退力道")]
    [SerializeField]
    protected int KnockIntensity;
    [Tooltip("子彈類型，選擇穿透就是子彈擊中玩家後不會移除，且可能造成多次傷害(炸彈類的子彈)")]
    [SerializeField]
    protected ShootAmmoType AmmoType;
    [Tooltip("傷害間隔，只有子彈類型是穿透(代表擊中敵方後不會消失)才需要設定")]
    [SerializeField]
    protected float DamageInterval = 0.3f;
    [Tooltip("暈眩(秒數)、燃燒(秒數)、冰凍(秒數)、傷害Buff(秒數,增/減值)、無敵(秒數)、格檔(時間,秒數)")]
    [SerializeField]
    protected BufferData[] Buffers;
    [Tooltip("是否會觸發衝刺中的玩家")]
    [SerializeField]
    protected bool TriggerOnRushRole = true;

    protected bool OutSideDestroy = true;
    protected Force AttackerRoleTag;
    protected Force TargetRoleTag;
    protected bool IsLaunch;
    protected bool IsCausedDamage;
    [HideInInspector]
    public int Value;
    protected Role Target;
    float LifeTimer;
    protected Rigidbody2D MyRigi;
    protected Transform ParticleParent;
    float DestructMargin_Left;
    float DestructMargin_Right;

    protected MyTimer DamageTime;
    protected bool ReadyToDamage;
    protected float DamageIntervalTimer;


    public virtual void TriggerHitCondition(Role _role)
    {
        if (Buffers == null)
            return;

        for (int i = 0; i < Buffers.Length; i++)
        {
            _role.AddBuffer(Buffers[i].GetMemberwiseClone());
        }
    }
    public virtual void Init(Dictionary<string, object> _dic)
    {
        ReadyToDamage = true;
        if (DamageInterval <= 0)
            DamageInterval = 0.1f;
        DamageTime = new MyTimer(DamageInterval, DamageTimeOutFunc, false, false);
        DamageIntervalTimer = DamageInterval;
        ParticleParent = GameObject.FindGameObjectWithTag("ParticleParent").transform;
        MyRigi = GetComponentInParent<Rigidbody2D>();
        if (MyRigi == null)
        {
            MyRigi = gameObject.AddComponent<Rigidbody2D>();
            MyRigi.gravityScale = 0;
        }
        LifeTimer = LifeTime;
        AttackerRoleTag = ((Force)(_dic["AttackerForce"]));
        TargetRoleTag = ((Force)(_dic["TargetRoleTag"]));
        if (AttackerRoleTag == Force.Player)
        {
            tag = AmmoForce.PlayerAmmo.ToString();
        }
        else
        {
            tag = AmmoForce.EnemyAmmo.ToString();
        }
        Value = int.Parse(_dic["Damage"].ToString());
        SpawnParticles();
    }
    protected void DamageTimeOutFunc()
    {
        DamageIntervalTimer = DamageInterval;
        ReadyToDamage = true;
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
    protected virtual void SpawnDeadParticles(Vector2 _pos)
    {

        for (int i = 0; i < DeadParticles.Length; i++)
        {
            if (DeadParticles[i] == null)
                continue;
            EffectEmitter.EmitParticle(DeadParticles[i], _pos, Vector3.zero, ParticleParent);
        }
    }
    public virtual void Launch()
    {
        IsLaunch = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D _col)
    {
        if (!ReadyToDamage)
            return;
        if (TargetRoleTag.ToString() == _col.tag.ToString())
        {
            TriggerTarget(_col.GetComponent<Role>(), transform.position);
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToDamage)
            return;
        if (TargetRoleTag.ToString() == _col.tag.ToString())
            TriggerTarget(_col.GetComponent<Role>(), transform.position);
    }

    protected virtual void TriggerTarget(Role _role, Vector2 _pos)
    {
        ReadyToDamage = false;
        TriggerHitCondition(_role);
        if (AmmoType != ShootAmmoType.Permanent)
            IsCausedDamage = true;
        DamageTime.StartRunTimer = true;
        if (HitTargetParticle != null)
            EffectEmitter.EmitParticle(HitTargetParticle, _role.transform.position, Vector3.zero, ParticleParent);
        SpawnDeadParticles(_pos);
    }
    protected virtual void Update()
    {
        if (!IsLaunch)
            return;
        LIfeTimerFunc();
        if (!ReadyToDamage && !IsCausedDamage)
            DamageTime.RunTimer();
        if (OutSideDestroy)
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
    protected virtual void DestroyOutSideAmmos()
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
    public virtual void ForceReverse()
    {
    }
}
