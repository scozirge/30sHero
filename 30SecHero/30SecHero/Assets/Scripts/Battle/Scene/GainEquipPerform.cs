using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainEquipPerform : MonoBehaviour
{
    [SerializeField]
    float SpanwFinishTime;
    [SerializeField]
    Transform Target;
    [SerializeField]
    PerformEquip PerformEquipPrefab;

    public void Perform(List<EquipData> _dataList)
    {
        float interval = SpanwFinishTime / _dataList.Count;
        for (int i = 0; i < _dataList.Count; i++)
        {
            StartCoroutine(WaitForSpawn(_dataList[i], interval * i));
        }
    }

    void SpawnEquip(EquipData _data)
    {
        if(BattleManage.BM.MyPlayer!=null)
        {
            PerformEquip equip = Instantiate(PerformEquipPrefab, Vector3.zero, Quaternion.identity) as PerformEquip;
            equip.transform.SetParent(transform);
            equip.transform.position = BattleManage.BM.MyPlayer.transform.position;
            equip.Init(_data, Target);
        }
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    IEnumerator WaitForSpawn(EquipData _data, float _time)
    {
        yield return new WaitForSeconds(_time);
        SpawnEquip(_data);
    }
}
