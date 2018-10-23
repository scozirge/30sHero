using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Player
{
    public static bool IsInit;
    public static int ID { get; private set; }
    public static int UserID_K { get; private set; }
    public static string Name_K { get; private set; }
    public static int Gold { get; private set; }
    public static int Emerald { get; private set; }
    public static List<int> KillBossID = new List<int>();
    public static int MaxFloor { get; private set; }
    public static int MaxEnemyKills { get; private set; }



    //Strengthen Dic
    public static Dictionary<int, StrengthenData> StrengthenDic = new Dictionary<int, StrengthenData>();
    //Properties
    static Dictionary<RoleProperty, float> Properties = new Dictionary<RoleProperty, float>();
    static Dictionary<RoleProperty, float> EquipPlus = new Dictionary<RoleProperty, float>();
    static Dictionary<RoleProperty, float> EquipMultiple = new Dictionary<RoleProperty, float>();
    static Dictionary<RoleProperty, float> StrengthenPlus = new Dictionary<RoleProperty, float>();
    static Dictionary<RoleProperty, float> StrengthenMultiple = new Dictionary<RoleProperty, float>();

    public static float GetProperties(RoleProperty _property)
    {
        return
            Properties[_property]
            *
            (EquipMultiple[_property] + StrengthenMultiple[_property])
            +
            (EquipPlus[_property] + StrengthenPlus[_property]);
    }

    public static void Init()
    {
        if (IsInit)
            return;
        SetLanguage((Language)PlayerPrefs.GetInt("UseLanguage"));
        if (PlayerPrefs.GetInt("MusicOn") == 1)
            SetMusic(true);
        else
            SetMusic(false);
        if (PlayerPrefs.GetInt("SoundOn") == 1)
            SetSound(true);
        else
            SetSound(false);
        Properties = GameSettingData.GetNewRolePropertiesDic(0);
        EquipPlus = GameSettingData.GetNewRolePropertiesDic(0);
        EquipMultiple = GameSettingData.GetNewRolePropertiesDic(1);
        StrengthenPlus = GameSettingData.GetNewRolePropertiesDic(0);
        StrengthenMultiple = GameSettingData.GetNewRolePropertiesDic(1);
        StrengthenDic = StrengthenData.GetNewStrengthenDic(0);
        //測試用
        /*
        Dictionary<long, EquipData> list = new Dictionary<long, EquipData>();
        WeaponData w = WeaponData.GetNewWeapon(1, 4, 3);
        WeaponData w2 = WeaponData.GetNewWeapon(2, 5, 2);
        list.Add(w.UID, w);
        list.Add(w2.UID, w2);
        Itmes.Add(EquipType.Weapon, list);

        list = new Dictionary<long, EquipData>();
        ArmorData a = ArmorData.GetNewArmor(2, 2, 1, false);
        ArmorData a2 = ArmorData.GetNewArmor(3, 3, 3, false);
        list.Add(a.UID, a);
        list.Add(a2.UID, a2);
        Itmes.Add(EquipType.Armor, list);

        list = new Dictionary<long, EquipData>();
        AccessoryData ad = AccessoryData.GetNewAccessory(1, 5, 1);
        AccessoryData ad2 = AccessoryData.GetNewAccessory(2, 4, 2);
        AccessoryData ad3 = AccessoryData.GetNewAccessory(3, 3, 3);
        AccessoryData ad4 = AccessoryData.GetNewAccessory(1, 2, 4);
        AccessoryData ad5 = AccessoryData.GetNewAccessory(2, 1, 5);
        list.Add(ad.UID, ad);
        list.Add(ad2.UID, ad2);
        list.Add(ad3.UID, ad3);
        list.Add(ad4.UID, ad4);
        list.Add(ad5.UID, ad5);
        Itmes.Add(EquipType.Accessory, list);
        list = new Dictionary<long, EquipData>();
         */
        IsInit = true;
    }
    public static void SetGold(int _gold)
    {
        Gold = _gold;
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Gold);
            }
        }
    }
    public static void SetEmerald(int _emerald)
    {
        Emerald = _emerald;
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Emerald);
            }
        }
    }
    public static void GainGold(int _gold)
    {
        if (_gold == 0)
            return;
        Gold += _gold;
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Gold);
            }
        }
    }
    public static void GainEmerald(int _emerald)
    {
        if (_emerald == 0)
            return;
        Emerald += _emerald;
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Emerald);
            }
        }
    }
    public static void StrengthenUpgrade(StrengthenData _data)
    {
        //執行強化
        GainGold(-_data.GetPrice());
        if (StrengthenDic.ContainsKey(_data.ID))
        {
            StrengthenDic[_data.ID].LVUP();
        }
        //寫入資料
        if (StrengthenInitDataFinish)
        {
            if (LocalData)
            {
                Debug.Log("更新Loco玩家強化");
                //強化
                List<int> keys = new List<int>(StrengthenDic.Keys);
                string dataStr = "";
                for (int i = 0; i < keys.Count; i++)
                {
                    if (i != 0)
                        dataStr += "/";
                    dataStr += StrengthenDic[keys[i]].ID + "," + StrengthenDic[keys[i]].LV;
                }
                PlayerPrefs.SetString(LocoData.Strengthen.ToString(), dataStr);
            }
            else
            {
                Debug.Log("更新server玩家強化");
                ServerRequest.StrengthenUpgrade(_data.ID, _data.LV, Gold);
            }
        }
    }
    public static void SetMaxFloor_Local(int _maxFloor)
    {
        if (_maxFloor == MaxFloor)
            return;
        MaxFloor = _maxFloor;
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            Debug.Log("更新Loco玩家樓層");
            PlayerPrefs.SetInt(LocoData.MaxFloor.ToString(), MaxFloor);
        }
    }
    public static void SetMaxEnemyKills_Local(int _maxEnemyKills)
    {
        if (MaxEnemyKills == _maxEnemyKills)
            return;
        MaxEnemyKills = _maxEnemyKills;
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            Debug.Log("更新Loco玩家殺敵數");
            PlayerPrefs.SetInt(LocoData.MaxEnemyKills.ToString(), MaxEnemyKills);
        }
    }
    public static void GainEquip_Local(List<EquipData> _datas)
    {
        if (_datas == null || _datas.Count == 0)
            return;
        //寫入資料
        if (EquipInitDataFinish)
        {
            Debug.Log("更新Loco玩家裝備");
            for (int i = 0; i < _datas.Count; i++)
            {
                //Debug.Log("Type=" + _datas[i].Type + "  UID=" + _datas[i].UID);
                if (!Itmes[_datas[i].Type].ContainsKey(_datas[i].UID))
                    Itmes[_datas[i].Type].Add(_datas[i].UID, _datas[i]);
                else
                    Debug.LogWarning("重複裝備UID  Type=" + _datas[i].Type + "  UID=" + _datas[i].UID);
            }
            EquipSaveLocalData();
        }
    }
    static List<EquipData> CurGainEquipDatas;
    public static void Settlement(int _gold, int _emerald, int _maxFloor, List<EquipData> _equipDatas)
    {
        if (_equipDatas.Count != 0)
            CurGainEquipDatas = _equipDatas;
        else
            CurGainEquipDatas = null;
        Debug.Log("更新Server玩家裝備");
        string addEquipStr = "";
        for (int i = 0; i < _equipDatas.Count; i++)
        {
            if (i != 0)
                addEquipStr += "/";
            addEquipStr += _equipDatas[i].ID + "," + (int)_equipDatas[i].Type + "," + _equipDatas[i].EquipSlot + "," + _equipDatas[i].LV + "," + _equipDatas[i].Quality + "," + ID;
        }
        //Debug.Log("gold=" + _gold + " emerald=" + _emerald + " maxFloor=" + _maxFloor + " addEquipStr=" + addEquipStr);
        ServerRequest.Settlement(_gold, _emerald, _maxFloor, addEquipStr);
    }
    public static void Settlement_CB(string[] _data)
    {
        //設定玩家資料
        Gold = int.Parse(_data[0]);
        Emerald = int.Parse(_data[1]);
        MaxFloor = int.Parse(_data[2]);
        //設定裝備
        if (CurGainEquipDatas != null && CurGainEquipDatas.Count != 0)
        {
            string[] equipUID = _data[3].Split(',');
            if (equipUID.Length != CurGainEquipDatas.Count)
            {
                Debug.LogWarning("結算server回傳資料錯誤");
                return;
            }
            for (int i = 0; i < CurGainEquipDatas.Count; i++)
            {
                if (!Itmes[CurGainEquipDatas[i].Type].ContainsKey(CurGainEquipDatas[i].UID))
                {
                    Itmes[CurGainEquipDatas[i].Type].Add(CurGainEquipDatas[i].UID, CurGainEquipDatas[i]);
                    CurGainEquipDatas[i].SetUID(int.Parse(equipUID[i]));
                }
                else
                {
                    Debug.LogWarning("重複裝備UID  Type=" + CurGainEquipDatas[i].Type + "  UID=" + CurGainEquipDatas[i].UID);
                }
            }
        }
        //顯示結果在結算
        BattleManage.BM.ShowResult();
    }
}
