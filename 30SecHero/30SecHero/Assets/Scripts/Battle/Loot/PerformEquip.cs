using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PerformEquip : MonoBehaviour
{
    [SerializeField]
    Transform EquipTrans;
    [SerializeField]
    Image[] Icons;
    [SerializeField]
    float RotateFactor;
    [SerializeField]
    int MoveSpeed;
    [SerializeField]
    ParticleSystem DeathEffect;
    [SerializeField]
    int RandomStartVol;
    EquipData MyData;

    Rigidbody2D MyRigid;
    Transform TargetTrans;

    void Awake()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        MyRigid.velocity = new Vector2(Random.Range(-RandomStartVol, RandomStartVol), Random.Range(-RandomStartVol, RandomStartVol));
    }
    public void Init(EquipData _data, Transform _target)
    {
        MyData = _data;
        TargetTrans = _target;
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].sprite = MyData.Icons[i];
            Icons[i].SetNativeSize();
        }
        EquipTrans.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-20, 20)));
    }
    void FixedUpdate()
    {
        if (!MyRigid)
            return;
        if (TargetTrans == null)
            return;
        Vector2 targetVol = (TargetTrans.position - transform.position).normalized * MoveSpeed;
        MyRigid.velocity = Vector2.Lerp(MyRigid.velocity, targetVol, RotateFactor);
    }
    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == "Skull")
        {
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
