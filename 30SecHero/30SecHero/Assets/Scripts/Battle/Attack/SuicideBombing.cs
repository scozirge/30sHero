using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Role))]
public class SuicideBombing : Skill
{
    [Tooltip("產生炸彈子彈後是否死亡")]
    [SerializeField]
    bool Suicide;
    [Tooltip("爆炸前的特效物件")]
    [SerializeField]
    ParticleSystem[] PrepareParticle;
    [Tooltip("炸彈子彈物件")]
    [SerializeField]
    Bomb AttackPrefab;
    //Collider2D Dector;
    [Tooltip("距離爆炸的準備時間(秒)")]
    [SerializeField]
    protected float PrepareTime;


    protected Force TargetForce;
    protected bool Detected;
    protected float PrepareTimer;

    protected override void Awake()
    {
        base.Awake();
        if (Myself.tag.ToString() == Force.Player.ToString())
            TargetForce = Force.Enemy;
        else
        {
            GameObject go = GameObject.FindGameObjectWithTag(Force.Player.ToString());
            if (go != null)
                Target = go.GetComponent<PlayerRole>();
            TargetForce = Force.Player;
        }
        PrepareTimer = PrepareTime;
        //Dector = transform.GetComponentInChildrenExcludeSelf<Collider2D>();
    }
    public override void PlayerGetSkill(float _skillTimeBuff)
    {
        base.PlayerGetSkill(_skillTimeBuff);
        Detected = false;
    }
    public override void LaunchAISpell()
    {
        base.LaunchAISpell();
        Detected = true;
    }
    protected override void Update()
    {
        base.Update();
        PrefareTimerFunc();
    }
    protected override void TimerFunc()
    {
        if (BehaviorSkill)
            return;
        if (!CanAttack)
            return;
        if (!Target)
            return;
        if (Vector3.Distance(Target.transform.position, transform.position) > DetecteRadius)
            return;
        if (Detected)
            return;
        base.TimerFunc();

        TriggerTarget(Target.GetComponent<Role>());
    }
    protected virtual void TriggerTarget(Role _curTarget)
    {
        Detected = true;
        SpawnPreparePrefab();
    }
    protected void SpawnPreparePrefab()
    {
        for (int i = 0; i < PrepareParticle.Length; i++)
        {
            if (PrepareParticle[i] == null)
                continue;
            EffectEmitter.EmitParticle(PrepareParticle[i], Vector3.zero, Vector3.zero, transform);
        }
    }
    public override void SpawnAttackPrefab()
    {
        base.SpawnAttackPrefab();
        if (gameObject.tag == Force.Player.ToString())
            AmmoData.Add("TargetRoleTag", Force.Enemy);
        else
            AmmoData.Add("TargetRoleTag", Force.Player);
        AmmoData.Add("Target", Target);
        //自身子彈
        if(AttackPrefab)
        {
            GameObject go = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            Ammo ammo = go.GetComponent<Ammo>();
            go.transform.SetParent(AmmoParent);
            go.transform.position = transform.position;
            ammo.Init(AmmoData);
        }
        if (Suicide)
        {
            if (Myself.MyForce != Force.Player)
                SelfDestroy();
            else
            {
                this.enabled = false;
            }
        }
        else
        {
            Detected = false;
        }
    }
    protected void PrefareTimerFunc()
    {
        if (!Detected)
            return;
        if (PrepareTimer > 0)
        {
            PrepareTimer -= Time.deltaTime;
        }
        else
        {
            SpawnAttackPrefab();
            PrepareTimer = PrepareTime;
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
