using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public partial class Ammo : MonoBehaviour
{
    [SerializeField]
    float LifeTime;
    [SerializeField]
    protected int KnockForce;
    [SerializeField]
    protected float StunDuration;

    protected bool IsLaunch;
    protected int Damage;
    protected Role Target;
    float LifeTimer;
    protected Rigidbody2D MyRigi;

    public virtual void Init(Dictionary<string, object> _dic)
    {
        MyRigi = GetComponent<Rigidbody2D>();
        LifeTimer = LifeTime;
        Damage = int.Parse(_dic["Damage"].ToString());
        Target = ((Role)_dic["Target"]);
    }
    public virtual void Launch()
    {
        IsLaunch = true;
    }
    protected virtual void OnTriggerEnter2D(Collider2D _col)
    {
    }
    protected virtual void Update()
    {
        if (!IsLaunch)
            return;
        LIfeTimerFunc();
    }
    protected virtual void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
    protected virtual void LIfeTimerFunc()
    {
        if (LifeTimer > 0)
            LifeTimer -= Time.deltaTime;
        else
            SelfDestroy();
    }
}
