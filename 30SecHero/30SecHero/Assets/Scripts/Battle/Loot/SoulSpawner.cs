using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    [SerializeField]
    Soul SoulPrefab;

    static SoulSpawner MySelf;

    void Awake()
    {
        MySelf = GetComponent<SoulSpawner>();
    }
    public static Soul SpawnSoul(Vector3 _pos)
    {
        Soul soul = Instantiate(MySelf.SoulPrefab, Vector3.zero, Quaternion.identity) as Soul;
        soul.transform.SetParent(MySelf.transform);
        soul.transform.localPosition = _pos;
        return soul;
    }
}
