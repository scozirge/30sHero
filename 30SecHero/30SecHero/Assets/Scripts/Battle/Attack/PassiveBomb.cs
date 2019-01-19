using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Role))]
public class PassiveBomb : Skill
{
    [Tooltip("炸彈子彈物件")]
    [SerializeField]
    Bomb AttackPrefab;
    [Tooltip("被攻擊幾次觸發")]
    [SerializeField]
    int BeAttackTimesTrigger;
    [Tooltip("多久重製被攻擊觸發技能次數")]
    [SerializeField]
    float ResetTime;
    int CurBeAttackTimes;

    MyTimer ResetTimer;

    protected Force TargetForce;

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
        if (ResetTime > 0)
            ResetTimer = new MyTimer(ResetTime, ResetBeAttackTimer, true, false);
        //Dector = transform.GetComponentInChildrenExcludeSelf<Collider2D>();
    }
    void ResetBeAttackTimer()
    {
        CurBeAttackTimes = 0;
        ResetTimer.StartRunTimer = true;
    }
    public void TriggerPassiveCheck()
    {
        CurBeAttackTimes++;
        ResetTimer.RestartCountDown();
        if(CurBeAttackTimes>=BeAttackTimesTrigger)
        {
            CurBeAttackTimes = 0;
            SpawnAttackPrefab();
        }
    }
    public override void PlayerGetSkill(float _skillTimeBuff)
    {
        base.PlayerGetSkill(_skillTimeBuff);
    }
    public override void LaunchAISpell()
    {
        base.LaunchAISpell();
    }
    protected override void Update()
    {
        base.Update();
        if (ResetTimer != null)
            ResetTimer.RunTimer();
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
        base.TimerFunc();

        TriggerTarget(Target.GetComponent<Role>());
    }
    protected virtual void TriggerTarget(Role _curTarget)
    {
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
        if (AttackPrefab)
        {
            GameObject go = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            Ammo ammo = go.GetComponent<Ammo>();
            go.transform.SetParent(AmmoParent);
            go.transform.position = transform.position;
            ammo.Init(AmmoData);
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
