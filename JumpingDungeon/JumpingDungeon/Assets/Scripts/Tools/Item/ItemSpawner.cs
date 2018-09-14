using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    Item ItemPrefab;

    public Item Spawn()
    {
        Item t = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity) as Item;
        t.transform.SetParent(transform);
        t.transform.localPosition = Vector3.zero;
        return t;
    }
    public Item Spawn(Vector3 _pos)
    {
        Item t = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity) as Item;
        t.transform.SetParent(transform);
        t.transform.localPosition = _pos;
        return t;
    }
}
