using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Skill : MonoBehaviour
{
    [Tooltip("註解")]
    [TextArea]
    public string Description;
    [Tooltip("RoleBehavior用腳本技能，設定為True時技能只會透過腳本來施放(玩家獲得時會自動改回false)")]
    [SerializeField]
    public bool BehaviorSkill;
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
    public float DamagePercent = 1;
    [Tooltip("怪物攻擊時是否不要移動")]
    [SerializeField]
    public bool AttackStopMove;
    [Tooltip("停止移動時間")]
    [SerializeField]
    public float StopMoveTime;
    [Tooltip("施法音效(不管一次施法有幾發子彈都還是只會觸發一次聲音)")]
    [LabelOverride("施法音效")]
    [SerializeField]
    public AudioClip SpellSound;
    [Tooltip("子彈是否跟隨腳色本身跑")]
    [SerializeField]
    protected bool SpawnedInSelf;
    [LabelOverride("吸血比例")]
    [Tooltip("每發攻擊的吸血比例")]
    [SerializeField]
    protected float VampireProportion;

    protected Role Myself;
    protected Dictionary<string, object> AmmoData = new Dictionary<string, object>();
    protected Transform AmmoParent;
    protected List<Ammo> SubordinateAmmos;
    protected int AttackTimes;
    [HideInInspector]
    public float PSkillTimer;
    protected Role Target;
    protected bool CanAttack;
    public bool IsPlayerGetSkill;

    public virtual void LaunchAISpell()
    {
        Spell();
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetecteRadius);
        /*
        if (Selection.Contains(gameObject))
        {

        }
        */
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
        PSkillTimer = PSkillDuration;
        CanAttack = !Myself.Buffers.ContainsKey(RoleBuffer.Stun);
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
        if (SpawnedInSelf)
            AmmoParent = transform;
        SubordinateAmmos = new List<Ammo>();
    }
    public virtual void SetCanAttack(bool _bool)
    {
        CanAttack = _bool;
    }
    public virtual void PlayerInitSkill()
    {
        Awake();
        BehaviorSkill = false;
        enabled = false;
        AttackStopMove = false;
        IsPlayerGetSkill = true;
    }
    public virtual void PlayerGetSkill(float _skillTimeBuff)
    {
        if (SubordinateAmmos.Count == 0)
            AttackTimes = 0;
        PSkillTimer = PSkillDuration + _skillTimeBuff;
    }
    public virtual void SpawnAttackPrefab()
    {
        //Set AmmoData
        AmmoData.Clear();
        AmmoData.Add("Damage", (int)(Myself.Damage * DamagePercent));
        AmmoData.Add("DamagePercent", DamagePercent);
        AmmoData.Add("AttackerForce", Myself.MyForce);
        AmmoData.Add("Attacker", Myself);
        AmmoData.Add("VampireProportion", VampireProportion);
        Myself.Attack(this);
    }
    public virtual void Spell()
    {
        if (SpellSound)
            AudioPlayer.PlaySound(SpellSound);
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
    public virtual void Freeze(bool _freeze)
    {
    }
}
