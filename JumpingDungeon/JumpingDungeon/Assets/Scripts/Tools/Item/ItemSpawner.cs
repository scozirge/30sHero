using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    Item ItemPrefab;

    public Item Spawn(Data _data, MyUI _ui)
    {
        Item t = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity) as Item;
        t.transform.SetParent(transform);
        t.transform.localPosition = Vector3.zero;
        t.Set(_data, _ui);
        return t;
    }
}
