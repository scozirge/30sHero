using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strengthen : MonoBehaviour
{
    [SerializeField]
    ItemSpawner MySpanwer;

    List<StrengthenItem> ItemList = new List<StrengthenItem>();

    void OnEnable()
    {
        MySpanwer.Spawn();
    }


}
