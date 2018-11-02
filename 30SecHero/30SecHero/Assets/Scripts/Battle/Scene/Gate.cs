using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    [SerializeField]
    ParticleSystem DeathParticle;
    [Tooltip("牆壁破壞音效")]
    [SerializeField]
    AudioClip DestroySound;
    [SerializeField]
    GainEquipPerform PerformPrefab;



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
        SpawnPreformObj();
        if (DeathParticle)
            EffectEmitter.EmitParticle(DeathParticle, transform.position, Vector3.zero, null);
        BattleManage.SpawnNextGate(Floor);
        CameraController.PlayMotion("Shake1");
        AudioPlayer.PlaySound(DestroySound);
        Destroy(gameObject);
    }
    void SpawnPreformObj()
    {
        GameObject parentGO = GameObject.FindGameObjectWithTag("LootParent");
        GainEquipPerform gep = Instantiate(PerformPrefab, Vector3.zero, Quaternion.identity) as GainEquipPerform;
        gep.transform.SetParent(parentGO.transform);
        gep.transform.localPosition = transform.position;
        gep.Perform(BattleManage.ExpectEquipDataList);
    }
}
