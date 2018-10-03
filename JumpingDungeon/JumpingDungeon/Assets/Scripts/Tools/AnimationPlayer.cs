using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField]
    Animator MyAni;

    public void PlayTrigger(string _motion, float _normalizedTime)
    {
        if (Animator.StringToHash(string.Format("Base Layer.{0}", _motion)) != MyAni.GetCurrentAnimatorStateInfo(0).fullPathHash)
            MyAni.Play(_motion, 0, _normalizedTime);
        else
            MyAni.StopPlayback();//重播
    }
    public void PlayTrigger_NoPlayback(string _motion, float _normalizedTime)
    {
        if (Animator.StringToHash(string.Format("Base Layer.{0}", _motion)) != MyAni.GetCurrentAnimatorStateInfo(0).fullPathHash)
            MyAni.Play(_motion, 0, _normalizedTime);
    }
    public void PlayFloat(string _motion, float _value)
    {
        MyAni.SetFloat(_motion, _value);
    }
}
