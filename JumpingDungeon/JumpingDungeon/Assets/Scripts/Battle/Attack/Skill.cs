using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField]
    public string SkillName;
    public float SkillDuration;
    protected Role Myself;
    protected Dictionary<string, object> AmmoData = new Dictionary<string, object>();
    protected Transform AmmoParent;


    protected virtual void Awake()
    {
        if (SkillName == null)
            SkillName = gameObject.name;
        if (gameObject.tag == Force.Player.ToString())
        {
            Myself = GetComponent<PlayerRole>();
        }
        else if(gameObject.tag==Force.Enemy.ToString())
        {
            Myself = GetComponent<EnemyRole>();
        }
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
    }
    public virtual void PlayerGetSkill()
    {
        enabled = false;
    }
    protected virtual void SpawnAttackPrefab()
    {
        //Set AmmoData
        AmmoData.Clear();
        AmmoData.Add("Damage", Myself.Damage);
        AmmoData.Add("AttackerForce", Myself.MyForce);
        Myself.Attack();
    }
}
