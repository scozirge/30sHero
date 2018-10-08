using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Player
{
    public static bool IsInit;
    public static Language UseLanguage { get; private set; }
    public static string AC { get; private set; }
    public static int Gold { get; private set; }
    public static int Emerald { get; private set; }

    //Equip
    public static WeaponData MyWeapon;
    public static ArmorData MyArmor;
    public static AccessoryData[] MyAccessorys = new AccessoryData[2] { null, null };
    //Items
    public static Dictionary<EquipType, Dictionary<long, EquipData>> Itmes = new Dictionary<EquipType, Dictionary<long, EquipData>>();

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
        Player.SetLanguage(Language.EN);
        Properties = GameSettingData.GetNewRolePropertiesDic(0);
        EquipPlus = GameSettingData.GetNewRolePropertiesDic(0);
        EquipMultiple = GameSettingData.GetNewRolePropertiesDic(1);
        StrengthenPlus = GameSettingData.GetNewRolePropertiesDic(0);
        StrengthenMultiple = GameSettingData.GetNewRolePropertiesDic(1);
        StrengthenDic = StrengthenData.GetNewStrengthenDic(0);
        //測試用
        GainGold(100);
        StrengthenDic[1].LV = 3;
        Dictionary<long, EquipData> list = new Dictionary<long, EquipData>();
        WeaponData w = WeaponData.GetNewWeapon(1, 4, 3);
        WeaponData w2 = WeaponData.GetNewWeapon(2, 5, 2);
        list.Add(w.UID, w);
        list.Add(w2.UID, w2);
        Itmes.Add(EquipType.Weapon, list);
        List<WeaponData> MyLies = new List<WeaponData>();
        MyLies.Add(w);
        MyLies.Add(w2);
        MyLies.Remove(w);

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
        //PlayerPrefs.DeleteAll();//清除玩家資料

        //if (PlayerPrefs.GetString("AC") != "")
        //    AC = PlayerPrefs.GetString("AC");
        //if (PlayerPrefs.GetString("ACPass") != "")
        //    ACPass = PlayerPrefs.GetString("ACPass");
        //if (PlayerPrefs.GetString("Name") != "")
        //    Name = PlayerPrefs.GetString("Name");
        IsInit = true;
    }
    public static void UpdateRecord(Dictionary<string, object> _data)
    {
    }
    public static void AutoLogin()
    {
    }
    public static void Equip(WeaponData _data)
    {
        if (MyWeapon != null)
            MyWeapon.IsEquiped = false;
        MyWeapon = _data;
        MyWeapon.IsEquiped = true;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
    }
    public static void Equip(ArmorData _data)
    {
        if (MyArmor != null)
            MyArmor.IsEquiped = false;
        MyArmor = _data;
        MyArmor.IsEquiped = true;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
    }
    public static void Equip(AccessoryData _data, int _index)
    {
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        if (MyAccessorys[_index] != null)
            MyAccessorys[_index].IsEquiped = false;
        MyAccessorys[_index] = _data;
        MyAccessorys[_index].IsEquiped = true;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
    }
    public static void TakeOff(WeaponData _data)
    {
        if (MyWeapon != null)
            MyWeapon.IsEquiped = false;
        MyWeapon = null;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Minus);
    }
    public static void TakeOff(ArmorData _data)
    {
        if (MyArmor != null)
            MyArmor.IsEquiped = false;
        MyArmor = null;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Minus);
    }
    public static void TakeOff(AccessoryData _data, int _index)
    {
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        if (MyAccessorys[_index] != null)
            MyAccessorys[_index].IsEquiped = false;
        MyAccessorys[_index] = null;
    }
    public static void SellEquip(EquipData _data)
    {
        if (Itmes[_data.Type].ContainsKey(_data.UID))
        {
            GainGold(_data.SellGold);
            Itmes[_data.Type].Remove(_data.UID);
        }
        else
            Debug.LogWarning("Sell Equip isn't in Items");
    }
    public static void GainGold(int _gold)
    {
        Gold += _gold;
        Main.UpdateResource();
    }
    public static void GainEmerald(int _emerald)
    {
        Emerald += _emerald;
        Main.UpdateResource();
    }
    public static void UpgradeStrengthen(StrengthenData _data)
    {
        GainGold(-_data.GetPrice());
        _data.LevelUp();
    }
}
