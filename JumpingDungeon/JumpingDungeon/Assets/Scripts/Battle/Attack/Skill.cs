using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField]
    public string SkillName;
    public float SkillDuration;
    protected virtual void Awake()
    {
        if (SkillName == null)
            SkillName = gameObject.name;
    }
    public virtual void PlayerGetSkill()
    {
        enabled = false;
    }
}
