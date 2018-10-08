using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    [SerializeField]
    ParticleSystem DeathParticle;

    public int Floor { get; private set; }

    public void Init(int _floor)
    {
        Floor = _floor;
    }
    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.tag.ToString() == Force.Player.ToString())
        {
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        if (DeathParticle)
            EffectEmitter.EmitParticle(DeathParticle, transform.position, Vector3.zero, null);
        BattleManage.SpawnNextGate(Floor);
        Destroy(gameObject);
    }
}
