using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Debugger : MonoBehaviour
{


    // Update is called once per frame
    void KeyDetector()
    {
        /*
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name=="Battle")
        {

        }
        */

        if (Input.GetKeyDown(KeyCode.Z))
        {
            BattleManage.BM.MyPlayer.HealHP(10000);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            int aa = 10000000;
            BattleManage.BM.MyPlayer.ReceiveDmg(ref aa);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            BattleManage.BM.MyPlayer.ShowMyEnchantInfo();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            KongregateAPIBehaviour.EndLogin = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            AudioPlayer.FadeOutMusic("Battle", 1f);
            AudioPlayer.FadeInMusic(GameManager.GM.BossFightMusic, "BossFight", 2f,1);
            //Player.ShowStrengthenProperties();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            AudioPlayer.FadeOutMusic("BossFight", 0.5f);
            AudioPlayer.FadeInMusic(GameManager.GM.FightMusic2, "Battle", 2f, 1);
            //Player.ShowTotalProperties();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //ServerRequest.Test();

            KongregateAPIBehaviour.ResetData();
            Player.ResetData();
            GameManager.KG.Init();
            PopupUI.CallCutScene("Init");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Player.ClearLocoData();

        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Gain GOld 1000");
            Player.GainGold(1000);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Gain Emerald 10");
            Player.GainEmerald(10,false);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            List<EquipData> GainEquipDataList = new List<EquipData>();
            GainEquipDataList.Add(WeaponData.GetRandomNewWeapon(10, 5));
            GainEquipDataList.Add(ArmorData.GetRandomNewArmor(10, 5));
            GainEquipDataList.Add(AccessoryData.GetRandomNewAccessory(10, 5));
            //裝備獲得
            if (Player.LocalData)
                Player.GainEquip_Local(GainEquipDataList);
            else
                //送server處理
                Player.Settlement(Player.Gold, Player.Emerald,Player.FreeEmerald, Player.CurFloor, Player.MaxFloor, GainEquipDataList);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            EnchantData ed = EnchantData.GetUnLockRandomEnchant();
            if (ed != null)
            {
                Player.EnchantUpgrade(ed,false);
                Debug.Log("Unlock Enchant:" + ed.Name + " LV:" + ed.LV);
            }
            else
            {
                Debug.Log("沒有可以獲得的附魔了");
            }
        }
    }
}
