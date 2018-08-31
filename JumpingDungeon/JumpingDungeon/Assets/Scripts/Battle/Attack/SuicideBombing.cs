using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyRole))]
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



    protected bool Detected;
    protected EnemyRole Myself;
    protected float PrepareTimer;
    protected Transform AmmoParent;

    void Awake()
    {
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
        Myself = GetComponent<EnemyRole>();
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
        switch (_col.gameObject.tag)
        {
            case "Player":
                Detected = true;
                SpawnPreparePrefab();
                break;
            default:
                break;
        }
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
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Damage", Myself.Damage);
        com.Init(data);
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
