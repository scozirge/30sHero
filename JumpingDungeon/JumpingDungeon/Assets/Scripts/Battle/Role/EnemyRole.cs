using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRole : Role
{
    [SerializeField]
    EnemyAmmo AmmoPrefab;
    [SerializeField]
    Transform AmmoParent;
    [SerializeField]
    float AmmoSpeed;
    [SerializeField]
    float AttackInterval;
    float AttackTimer;


    PlayerRole Target;
    public List<EnemyAmmo> MyAmmos;


    protected override void Awake()
    {
        base.Awake();
        /*
        Targets = new List<Role>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < gos.Length; i++)
        {
            Targets.Add(gos[i].GetComponent<Role>());
        }
        */
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRole>();
        AttackTimer = AttackInterval;
    }

    protected override void Update()
    {
        base.Update();
        AttackTimerFunc();
    }

    void AttackTimerFunc()
    {
        if (AttackTimer > 0)
            AttackTimer -= Time.deltaTime;
        else
            Attack();
    }
    protected override void Attack()
    {
        base.Attack();
        AttackTimer = AttackInterval;

        //Generate Ammo and Launch it.
        Spawn();
    }
    public void Spawn()
    {
        //Spawn ammo
        MyAmmos = new List<EnemyAmmo>();
        GameObject ammoGO = Instantiate(AmmoPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        EnemyAmmo ea = ammoGO.GetComponent<EnemyAmmo>();
        MyAmmos.Add(ea);
        ea.transform.SetParent(AmmoParent);
        ea.transform.position = transform.position;
        //Set AmmoData
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Damage", Damage);
        data.Add("AmmoSpeed", AmmoSpeed);
        Vector3 Direction = (Target.transform.position - transform.position).normalized;
        data.Add("Direction", Direction);
        ea.Init(data);
    }
}
