using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Player
{
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

    public static int Strength
    {
        get
        {
            int result = BaseStrength;
            if (MyWeapon != null)
                result += MyWeapon.Strength;
            if (MyArmor != null)
                result += MyArmor.Strength;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].Strength;
            }
            return result;
        }
    }
    public static int Health
    {
        get
        {
            int result = BaseHealth;
            if (MyWeapon != null)
                result += MyWeapon.Health;
            if (MyArmor != null)
                result += MyArmor.Health;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].Health;
            }
            return result;
        }
    }
    public static int Shield
    {
        get
        {
            int result = BaseShield;
            if (MyWeapon != null)
                result += MyWeapon.Shield;
            if (MyArmor != null)
                result += MyArmor.Shield;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].Shield;
            }
            return result;
        }
    }
    public static int ShieldRecovery
    {
        get
        {
            int result = BaseShieldRecovery;
            if (MyWeapon != null)
                result += MyWeapon.RandomShieldRecovery;
            if (MyArmor != null)
                result += MyArmor.RandomShieldRecovery;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomShieldRecovery;
            }
            return result;
        }
    }

    public static int MoveSpeed
    {
        get
        {
            int result = BaseMoveSpeed;
            if (MyWeapon != null)
                result += MyWeapon.RandomMoveSpeed;
            if (MyArmor != null)
                result += MyArmor.RandomMoveSpeed;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomMoveSpeed;
            }
            return result;
        }
    }
    public static int MaxMoveSpeed
    {
        get
        {
            int result = BaseMaxMoveSpeed;
            if (MyWeapon != null)
                result += MyWeapon.RandomMaxMoveSpeed;
            if (MyArmor != null)
                result += MyArmor.RandomMaxMoveSpeed;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomMaxMoveSpeed;
            }
            return result;
        }
    }
    public static float AvatarTime
    {
        get
        {
            float result = BaseAvatarTime;
            if (MyWeapon != null)
                result += MyWeapon.RandomAvatarTime;
            if (MyArmor != null)
                result += MyArmor.RandomAvatarTime;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomAvatarTime;
            }
            return result;
        }
    }
    public static float SkillTime
    {
        get
        {
            float result = BaseSkillTime;
            if (MyWeapon != null)
                result += MyWeapon.RandomSkillTime;
            if (MyArmor != null)
                result += MyArmor.RandomSkillTime;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomSkillTime;
            }
            return result;
        }
    }
    public static float SkillDrop
    {
        get
        {
            float result = BaseSkillDrop;
            if (MyWeapon != null)
                result += MyWeapon.RandomSkillDrop;
            if (MyArmor != null)
                result += MyArmor.RandomSkillDrop;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomSkillDrop;
            }
            return result;
        }
    }
    public static int GoldDrop
    {
        get
        {
            int result = BaseGoldDrop;
            if (MyWeapon != null)
                result += MyWeapon.RandomGoldDrop;
            if (MyArmor != null)
                result += MyArmor.RandomGoldDrop;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomGoldDrop;
            }
            return result;
        }
    }
    public static float EquipDrop
    {
        get
        {
            float result = BaseEquipDrop;
            if (MyWeapon != null)
                result += MyWeapon.RandomEquipDrop;
            if (MyArmor != null)
                result += MyArmor.RandomEquipDrop;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomEquipDrop;
            }
            return result;
        }
    }
    public static float Bloodthirsty
    {
        get
        {
            float result = BaseBloodthirsty;
            if (MyWeapon != null)
                result += MyWeapon.RandomBloodThirsty;
            if (MyArmor != null)
                result += MyArmor.RandomBloodThirsty;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomBloodThirsty;
            }
            return result;
        }
    }
    public static float PotionEfficacy
    {
        get
        {
            float result = BasePotionEfficacy;
            if (MyWeapon != null)
                result += MyWeapon.RandomPotionEfficiency;
            if (MyArmor != null)
                result += MyArmor.RandomPotionEfficiency;
            for (int i = 0; i < MyAccessorys.Length; i++)
            {
                if (MyAccessorys[i] != null)
                    result += MyAccessorys[i].RandomPotionEfficiency;
            }
            return result;
        }
    }
    //Strengthen LV
    public static Dictionary<int, int> StrengthenLVDic = new Dictionary<int, int>();


    public static void Init()
    {
        Player.SetLanguage(Language.ZH_TW);
        StrengthenLVDic.Add(1, 3);
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
    }
    public static void Equip(ArmorData _data)
    {
        if (MyArmor != null)
            MyArmor.IsEquiped = false;
        MyArmor = _data;
        MyArmor.IsEquiped = true;
    }
    public static void Equip(AccessoryData _data, int _index)
    {
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        if (MyAccessorys[_index] != null)
            MyAccessorys[_index].IsEquiped = false;
        MyAccessorys[_index] = _data;
        MyAccessorys[_index].IsEquiped = true;
    }
    public static void TakeOff(WeaponData _data)
    {
        if (MyWeapon != null)
            MyWeapon.IsEquiped = false;
        MyWeapon = null;
    }
    public static void TakeOff(ArmorData _data)
    {
        if (MyArmor != null)
            MyArmor.IsEquiped = false;
        MyArmor = null;
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
            GainGolg(_data.SellGold);
            Itmes[_data.Type].Remove(_data.UID);
        }
        else
            Debug.LogWarning("Sell Equip isn't in Items");
    }
    public static void GainGolg(int _gold)
    {
        Gold += _gold;
        PlayerPanel.UpdateResource();
    }
    public static void GainEmerald(int _emerald)
    {
        Emerald += _emerald;
        PlayerPanel.UpdateResource();
    }
}
