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
            Player.ShowBaseProperties();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Player.ShowEquipProperties();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            BattleManage.BM.MyPlayer.ShowMyEnchantInfo();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Player.ShowStrengthenProperties();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            Player.ShowTotalProperties();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            BattleManage.BM.MyPlayer.AddBuffer(RoleBuffer.Freeze, 5);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            BattleManage.BM.MyPlayer.AddBuffer(RoleBuffer.Burn, 5);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            int damage = 10;
            BattleManage.BM.MyPlayer.BeAttack(Force.Enemy, ref damage, Vector2.zero);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteKey(LocoData.UseLanguage.ToString());
            PlayerPrefs.DeleteKey(LocoData.Init.ToString());
            PlayerPrefs.DeleteKey(LocoData.Equip.ToString());
            PlayerPrefs.DeleteKey(LocoData.Strengthen.ToString());
            PlayerPrefs.DeleteKey(LocoData.SoundOn.ToString());
            PlayerPrefs.DeleteKey(LocoData.MusicOn.ToString());
            PlayerPrefs.DeleteKey(LocoData.CurFloor.ToString());
            PlayerPrefs.DeleteKey(LocoData.MaxFloor.ToString());
            PlayerPrefs.DeleteKey(LocoData.MaxEnemyKills.ToString());
            PlayerPrefs.DeleteKey(LocoData.KillBossID.ToString());
            PlayerPrefs.DeleteKey(LocoData.Gold.ToString());
            PlayerPrefs.DeleteKey(LocoData.Emerald.ToString());
            PlayerPrefs.DeleteKey(LocoData.Enchant.ToString());
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            List<EquipData> GainEquipDataList = new List<EquipData>();
            GainEquipDataList.Add(WeaponData.GetRandomNewWeapon(99, 5));
            GainEquipDataList.Add(ArmorData.GetRandomNewArmor(99, 5));
            GainEquipDataList.Add(AccessoryData.GetRandomNewAccessory(99, 5));
            //裝備獲得
            if (Player.LocalData)
                Player.GainEquip_Local(GainEquipDataList);
            else
                //送server處理
                Player.Settlement(Player.Gold, Player.Emerald, Player.CurFloor, Player.MaxFloor, GainEquipDataList);
        }
    }
}
