using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Ammo : MonoBehaviour
{
    public virtual bool IsLaunch { get; protected set; }
    public virtual float AmmoSpeed { get; protected set; }
    public virtual Vector3 Ammovelocity { get; protected set; }
    public virtual int Damage { get; protected set; }

    public virtual void Init(Dictionary<string, object> _dic)
    {
        Damage = int.Parse(_dic["Damage"].ToString());
        AmmoSpeed = int.Parse(_dic["AmmoSpeed"].ToString());
        Ammovelocity = (Vector3)(_dic["Direction"]) * AmmoSpeed;
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
        transform.position += Ammovelocity * Time.deltaTime;
    }
    protected virtual void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
