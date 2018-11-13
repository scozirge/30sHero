using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : Ammo
{
    [SerializeField]
    float DamageHPRatio;
    [Tooltip("陷阱開啟時間")]
    [SerializeField]
    float ActiveTime;
    [Tooltip("陷阱關閉時間")]
    [SerializeField]
    float InActiveTime;
    Collider2D[] MyColliders;
    Image[] MyImages;
    ParticleSystem[] MyParticles;



    void Start()
    {
        Dictionary<string, object> dataDic = new Dictionary<string, object>();
        dataDic.Add("AttackerForce", Force.Enemy);
        dataDic.Add("TargetRoleTag", Force.Player);
        dataDic.Add("Damage", 0);
        Init(dataDic);
        OutSideDestroy = false;
    }
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Launch();
        if (InActiveTime > 0 && ActiveTime > 0)
        {
            MyColliders = GetComponentsInChildren<Collider2D>();
            MyImages = GetComponentsInChildren<Image>();
            MyParticles = GetComponentsInChildren<ParticleSystem>();
            ActiveTrap(true);
        }
    }
    protected override void LIfeTimerFunc()
    {
    }
    protected override void TriggerTarget(Role _role, Vector2 _pos)
    {
        if (_role.BuffersExist(RoleBuffer.Untouch))
            return;
        if (!TriggerOnRushRole && _role.OnRush)
            return;
        base.TriggerTarget(_role, _pos);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        Value = (int)(_role.MaxHealth * DamageHPRatio);
        int damage = Value;
        _role.BeAttack(AttackerRoleTag, ref damage, force);
    }
    void ActiveTrap(bool _active)
    {
        if (MyColliders != null && MyColliders.Length > 0)
        {
            for (int i = 0; i < MyColliders.Length; i++)
            {
                MyColliders[i].enabled = _active;

            }
        }
        if (MyImages != null && MyImages.Length > 0)
        {
            for (int i = 0; i < MyImages.Length; i++)
            {
                MyImages[i].enabled = _active;

            }
        }
        if (MyParticles != null && MyParticles.Length > 0)
        {
            for (int i = 0; i < MyParticles.Length; i++)
            {
                if (_active)
                    MyParticles[i].Play();
                else
                    MyParticles[i].Stop();
            }
        }
        Debug.Log("MyParticles" + MyParticles.Length);
        if (_active)
            StartCoroutine(WatiToInActive());
        else
            StartCoroutine(WatiToActive());
    }
    IEnumerator WatiToActive()
    {
        yield return new WaitForSeconds(InActiveTime);
        ActiveTrap(true);
    }
    IEnumerator WatiToInActive()
    {
        yield return new WaitForSeconds(ActiveTime);
        ActiveTrap(false);
    }
}
