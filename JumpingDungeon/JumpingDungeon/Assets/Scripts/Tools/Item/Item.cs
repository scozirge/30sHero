using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Data MyData;
    public virtual void Set(Data _data)
    {
        MyData = _data;
    }
}
