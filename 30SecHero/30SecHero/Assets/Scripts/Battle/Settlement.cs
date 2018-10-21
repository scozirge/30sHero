using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 partial class BattleManage
{
     [SerializeField]
     Text FloorClearText;
     [SerializeField]
     Text MaxFloorText;
     [SerializeField]
     Text MonsterKillsText;
     [SerializeField]
     Text GoldText;
     [SerializeField]
     Text EmeraldText;

     static int FloorGolds;
     static int NewFloorGolds;
     static int EnemyKillGolds;
     static int EnemyDropGolds;
     static int BossDropEmeralds;

     static int TotalGold;
     static int TtalEmerald;
     
     public static void EnemyDropGoldAdd(int _gold)
     {
         EnemyKillGolds += _gold;
         Debug.Log("_gold=" + _gold);
         Debug.Log("EnemyKillGolds=" + EnemyKillGolds);
     }
     public static void BossDropEmeraldAdd(int _emerald)
     {
         BossDropEmeralds += _emerald;
         Debug.Log("_emerald=" + _emerald);
         Debug.Log("BossDropEmeralds=" + BossDropEmeralds);
     }
     public void ShowSettlement()
     {
     }
}
