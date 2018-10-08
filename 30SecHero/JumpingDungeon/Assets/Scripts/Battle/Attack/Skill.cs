using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Skill : MonoBehaviour
{
    [Tooltip("技能名稱(一樣的技能名稱不會重複獲得)")]
    [SerializeField]
    public string PSkillName;
    [Tooltip("玩家獲得此技能的持續時間)")]
    [SerializeField]
    protected float PSkillDuration;
    [Tooltip("偵測有目標在半徑範圍內才會攻擊)")]
    [SerializeField]
    protected int DetecteRadius = 800;
    [Tooltip("傷害倍率(傷害=傷害倍率x腳色攻擊力))")]
    [SerializeField]
    protected float DamagePercent = 1;

    protected Role Myself;
    protected Dictionary<string, object> AmmoData = new Dictionary<string, object>();
    protected Transform AmmoParent;
    protected List<Ammo> SubordinateAmmos;
    protected int AttackTimes;
    [HideInInspector]
    public float PSkillTimer;
    protected Role Target;
    protected bool CanAttack;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetecteRadius);
    }
    protected virtual void Awake()
    {
        if (PSkillName == null)
            PSkillName = gameObject.name;
        if (gameObject.tag == Force.Player.ToString())
        {
            Myself = GetComponent<PlayerRole>();
        }
        else if (gameObject.tag == Force.Enemy.ToString())
        {
            Myself = GetComponent<EnemyRole>();
        }
        if (DamagePercent < 0)
            DamagePercent = 0;
        CanAttack = !Myself.Buffers.ContainsKey(RoleBuffer.Stun);
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
        SubordinateAmmos = new List<Ammo>();
    }
    public virtual void SetCanAttack(bool _bool)
    {
        CanAttack = _bool;
    }
    public virtual void PlayerInitSkill()
    {
        Awake();
        enabled = false;
    }
    public virtual void PlayerGetSkill()
    {
        if (SubordinateAmmos.Count == 0)
            AttackTimes = 0;
        PSkillTimer = PSkillDuration;
    }
    protected virtual void SpawnAttackPrefab()
    {
        //Set AmmoData
        AmmoData.Clear();
        AmmoData.Add("Damage", (int)(Myself.Damage * DamagePercent));
        AmmoData.Add("AttackerForce", Myself.MyForce);
        Myself.Attack();
    }
    public void InactivePlayerSkill()
    {
        DestroySubAmmos();
        enabled = false;
    }
    void DestroySubAmmos()
    {
        for (int i = 0; i < SubordinateAmmos.Count; i++)
        {
            if (SubordinateAmmos[i])
                SubordinateAmmos[i].SelfDestroy();
            SubordinateAmmos.RemoveAt(i);
        }
    }
    protected virtual void Update()
    {
        AutoDetectTarge();//要再TimeFunc之前，不然會一殺死怪物就觸發技能
        TimerFunc();
    }
    protected virtual void AutoDetectTarge()
    {
        if (Myself.MyForce == Force.Player)
        {
            Target = ((PlayerRole)Myself).ClosestEnemy;
        }
    }
    protected virtual void TimerFunc()
    {
    }
}
