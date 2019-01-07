using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    [SerializeField]
    List<ParticleSystem> DeathParticles;
    [Tooltip("牆壁破壞音效")]
    [SerializeField]
    AudioClip DestroySound;
    [SerializeField]
    GainEquipPerform PerformPrefab;
    [SerializeField]
    int DropGoldCount = 10;


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
        SpawnGold();
        DeathParticles.RemoveAll(item => item == null);
        if (DeathParticles!=null)
        {
            for(int i=0;i<DeathParticles.Count;i++)
            {
                EffectEmitter.EmitParticle(DeathParticles[i], transform.position, Vector3.zero, null);
            }
        }
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
    void SpawnGold()
    {
        //int gold = GameSettingData.FloorPassGold * BattleManage.Floor;
        //表演用，實際通關獲得的錢在settlement計算
        //DropGold
        for (int i = 0; i < DropGoldCount; i++)
        {
                ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
                if (loot) loot.Init(ResourceType.Gold, 0);
        }
    }
}
