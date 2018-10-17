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



    //PlayerRole Attributes
    public static int BaseStrength;
    public static int BaseHealth;
    public static int BaseShield;
    public static int BaseShieldRecovery;

    public static int BaseMoveSpeed;
    public static int BaseMaxMoveSpeed;
    public static float BaseAvatarTime;
    public static float BaseSkillTime;
    public static float BaseSkillDrop;
    public static int BaseGoldDrop;
    public static float BaseEquipDrop;
    public static float BaseBloodthirsty;
    public static float BasePotionEfficacy;
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
        if(PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Player.Gold);
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Player.Emerald);
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
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Player.Gold);
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Player.Emerald);
            }
        }
    }
    public static void GainGold(int _gold)
    {
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
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Player.Gold);
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Player.Emerald);
            }
        }
    }
    public static void GainEmerald(int _emerald)
    {
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
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Player.Gold);
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Player.Emerald);
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
                ServerRequest.StrengthenUpgrade(_data.ID, _data.LV + 1, Player.Gold);
            }
        }
    }
}
