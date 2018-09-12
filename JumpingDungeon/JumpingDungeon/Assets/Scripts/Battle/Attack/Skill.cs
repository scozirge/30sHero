using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField]
    public string PSkillName;
    [SerializeField]
    protected float PSkillDuration;
    protected Role Myself;
    protected Dictionary<string, object> AmmoData = new Dictionary<string, object>();
    protected Transform AmmoParent;
    protected List<Ammo> SubordinateAmmos;
    protected int AttackTimes;
    [HideInInspector]
    public float PSkillTimer;

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
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
        SubordinateAmmos = new List<Ammo>();
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
        AmmoData.Add("Damage", Myself.Damage);
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
}
