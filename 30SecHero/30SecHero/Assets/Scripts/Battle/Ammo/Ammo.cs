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
    [Tooltip("攻擊者跟隨特效物件")]
    [SerializeField]
    ParticleSystem[] LocalAttackerParticles;
    [Tooltip("命中特效-自身子彈")]
    [SerializeField]
    protected ParticleSystem[] DeadParticles;
    [Tooltip("命中特效-目標")]
    [SerializeField]
    ParticleSystem HitTargetParticle;
    [Tooltip("命中音效")]
    [SerializeField]
    protected AudioClip HitTargetSound;
    [Tooltip("子彈存活時間(秒)")]
    [SerializeField]
    float LifeTime;
    [Tooltip("子彈擊退力道")]
    [SerializeField]
    protected int KnockIntensity;
    [Tooltip("子彈類型，選擇穿透就是子彈擊中玩家後不會移除，且可能造成多次傷害(炸彈類的子彈)")]
    [SerializeField]
    protected ShootAmmoType AmmoType;
    [Tooltip("傷害間隔，只有子彈類型是穿透(代表擊中敵方後不會消失)才需要設定，如果填0代表同一個目標只會命中一次")]
    [SerializeField]
    protected float DamageInterval = 0.3f;
    [Tooltip("暈眩(秒數)、燃燒(秒數)、冰凍(秒數)、傷害Buff(秒數,增/減值)、無敵(秒數)、格檔(時間,秒數)")]
    [SerializeField]
    protected List<BufferData> Buffers;
    [Tooltip("是否會觸發衝刺中的玩家")]
    [SerializeField]
    protected bool TriggerOnRushRole = true;


    protected bool OutSideDestroy = true;
    protected Force AttackerRoleTag;
    protected Force TargetRoleTag;
    protected bool IsLaunch;
    [HideInInspector]
    public int Value;
    protected float ValuePercent;
    protected Role Target;
    float LifeTimer;
    protected Rigidbody2D MyRigi;
    protected Transform ParticleParent;
    float DestructMargin_Left;
    float DestructMargin_Right;
    protected Role Attacker;
    protected List<MyTimer> ReadyToDamageTimers;
    protected Dictionary<string, bool> ReadyToDamageTargets;
    public bool IsPlayerGetSkill;


    public virtual void TriggerHitCondition(Role _role)
    {
        if (Buffers == null)
            return;

        for (int i = 0; i < Buffers.Count; i++)
        {
            _role.AddBuffer(Buffers[i].GetMemberwiseClone());
        }
    }
    public void SetBuffersTime(float _time)
    {
        for (int i = 0; i < Buffers.Count; i++)
        {
            Buffers[i].Time = _time;
        }
    }
    public void AddBuffer(params BufferData[] _datas)
    {
        for (int j = 0; j < _datas.Length; j++)
        {
            for (int i = 0; i < Buffers.Count; i++)
            {
                if (Buffers[i].Type == _datas[j].Type)
                {
                    Buffers[i] = _datas[j];
                    break;
                }
            }
            Buffers.Add(_datas[j]);
        }
    }
    public virtual void Init(Dictionary<string, object> _dic)
    {
        ReadyToDamageTargets = new Dictionary<string, bool>();
        ReadyToDamageTimers = new List<MyTimer>();
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
        ValuePercent = float.Parse(_dic["DamagePercent"].ToString());
        Attacker = (Role)(_dic["Attacker"]);
        SpawnParticles();
        if (IsPlayerGetSkill)
        {
            TurnOffLight();
        }
    }
    void TurnOffLight()
    {
        Light[] lights = GetComponentsInChildren<Light>();
        if (lights != null)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = false;
            }
        }
    }
    protected virtual void SpawnParticles()
    {
        //產生特效
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
        if(Attacker)
        {
            for (int i = 0; i < LocalAttackerParticles.Length; i++)
            {
                if (LocalAttackerParticles[i] == null)
                    continue;
                EffectEmitter.EmitParticle(LocalAttackerParticles[i], Vector3.zero, Vector3.zero, Attacker.transform);
            }
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
        if (TargetRoleTag.ToString() == _col.tag.ToString())
        {
            Role role = _col.GetComponent<Role>();
            TriggerTarget(role, transform.position);
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D _col)
    {
        if (TargetRoleTag.ToString() == _col.tag.ToString())
        {
            Role role = _col.GetComponent<Role>();
            TriggerTarget(role, transform.position);
        }
    }
    protected void ReadyToDamageTimeOutFunc(string _key)
    {
        if (ReadyToDamageTargets.ContainsKey(_key))
        {
            ReadyToDamageTargets[_key] = true;
        }
    }
    protected bool CheckReadyToDamageTarget(Role _role)
    {
        string id = _role.GetInstanceID().ToString();
        if (ReadyToDamageTargets.ContainsKey(id))
        {
            if (ReadyToDamageTargets[id])
            {
                ReadyToDamageTargets[id] = false;
                return true;
            }
            else
                return false;
        }
        else
        {
            if (DamageInterval > 0)
                ReadyToDamageTimers.Add(new MyTimer(DamageInterval, ReadyToDamageTimeOutFunc, true, true, id));
            ReadyToDamageTargets.Add(id, false);
            return true;
        }
    }
    protected virtual void TriggerTarget(Role _role, Vector2 _pos)
    {
        if (HitTargetSound)
            AudioPlayer.PlaySound(HitTargetSound);
        TriggerHitCondition(_role);
        if (HitTargetParticle != null)
            EffectEmitter.EmitParticle(HitTargetParticle, _role.transform.position, Vector3.zero, ParticleParent);
        SpawnDeadParticles(_pos);
    }
    protected virtual void Update()
    {
        if (!IsLaunch)
            return;
        LIfeTimerFunc();
        for (int i = 0; i < ReadyToDamageTimers.Count; i++)
        {
            ReadyToDamageTimers[i].RunTimer();
        }
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
