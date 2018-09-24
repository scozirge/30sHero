using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Data MyData;
    protected MyUI ParentUI;

    public virtual void Set(Data _data, MyUI _ui)
    {
        MyData = _data;
        ParentUI = _ui;
    }
    public virtual void OnPress()
    {
    }
    public virtual void Filter(EquipType _type)
    {
    }

}
