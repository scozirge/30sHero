using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyUI : MonoBehaviour
{
    public virtual void SetActive(bool _bool)
    {
        gameObject.SetActive(_bool);
    }
    public virtual void OnEnable()
    {
    }
    public virtual void RefreshText()
    {
    }
}
