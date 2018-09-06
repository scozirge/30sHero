﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField]
    Animator MyAni;

    public void PlayTrigger(string _motion, float _normalizedTime)
    {
        Debug.Log("c");
        if (Animator.StringToHash(string.Format("Base Layer.{0}", _motion)) != MyAni.GetCurrentAnimatorStateInfo(0).fullPathHash)
            MyAni.Play(_motion, 0, _normalizedTime);
        else
            MyAni.StopPlayback();//重播
    }
    public void PlayInt(string _motion, int _int)
    {
        Debug.Log("d");
        MyAni.SetInteger(_motion, _int);
    }
}