using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Role))]
public class SuicideBombing : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] PrepareParticle;
    [SerializeField]
    Bomb AttackPrefab;
    [SerializeField]
    Collider2D Dector;
    [SerializeField]
    protected float PrepareTime;


    protected Force TargetForce;
    protected bool Detected;
    protected Role Myself;
    protected float PrepareTimer;
    protected Transform AmmoParent;
    protected Dictionary<string, object> AmmoData = new Dictionary<string, object>();

    void Awake()
    {
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
        Myself = GetComponent<Role>();
        if (Myself.tag.ToString() == Force.Player.ToString())
            TargetForce = Force.Enemy;
        else
            TargetForce = Force.Player;
        PrepareTimer = PrepareTime;
    }
    void Update()
    {
        PrefareTimerFunc();
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (Detected)
            return;
        if (_col.tag.ToString() == TargetForce.ToString())
        {
            TriggerTarget(_col.GetComponent<Role>());
        }
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
    protected void Boom()
    {
        GameObject go = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo com = go.GetComponent<Ammo>();
        go.transform.SetParent(AmmoParent);
        go.transform.position = transform.position;
        //Set AmmoData
        AmmoData.Clear();
        AmmoData.Add("Damage", Myself.Damage);
        AmmoData.Add("AttackerForce", Myself.MyForce);
        com.Init(AmmoData);
        SelfDestroy();
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
            Boom();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
