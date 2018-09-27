using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void OnPress()
    {
    }
    public virtual void Filter(EquipType _type)
    {
    }
    public virtual void RefreshText()
    {
    }
    public virtual void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
