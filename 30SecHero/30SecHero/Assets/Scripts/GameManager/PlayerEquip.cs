using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    //Equip
    public static WeaponData MyWeapon;
    public static ArmorData MyArmor;
    public static AccessoryData[] MyAccessorys = new AccessoryData[2] { null, null };
    //Items
    public static Dictionary<EquipType, Dictionary<long, EquipData>> Itmes = new Dictionary<EquipType, Dictionary<long, EquipData>>();

    public static void EquipSaveLocalData()
    {
        //武器文字
        List<long> keys = new List<long>(Itmes[EquipType.Weapon].Keys);
        string dataStr = "";
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            dataStr += Itmes[EquipType.Weapon][keys[i]].UID + "," + Itmes[EquipType.Weapon][keys[i]].ID + "," + Itmes[EquipType.Weapon][keys[i]].Type + "," + Itmes[EquipType.Weapon][keys[i]].EquipSlot + "," + Itmes[EquipType.Weapon][keys[i]].LV + "," + Itmes[EquipType.Weapon][keys[i]].Quality;
        }
        //防具文字
        keys = new List<long>(Itmes[EquipType.Armor].Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            dataStr += Itmes[EquipType.Armor][keys[i]].UID + "," + Itmes[EquipType.Armor][keys[i]].ID + "," + Itmes[EquipType.Armor][keys[i]].Type + "," + Itmes[EquipType.Armor][keys[i]].EquipSlot + "," + Itmes[EquipType.Armor][keys[i]].LV + "," + Itmes[EquipType.Armor][keys[i]].Quality;
        }
        //飾品文字
        keys = new List<long>(Itmes[EquipType.Accessory].Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            dataStr += Itmes[EquipType.Accessory][keys[i]].UID + "," + Itmes[EquipType.Accessory][keys[i]].ID + "," + Itmes[EquipType.Accessory][keys[i]].Type + "," + Itmes[EquipType.Accessory][keys[i]].EquipSlot + "," + Itmes[EquipType.Accessory][keys[i]].LV + "," + Itmes[EquipType.Accessory][keys[i]].Quality;
        }
        PlayerPrefs.SetString(LocoData.Equip.ToString(), dataStr);
    }
    public static void Equip(WeaponData _data)
    {
        //執行更換裝備
        EquipData origEquipData = MyWeapon;
        if (origEquipData != null)
            origEquipData.SetEquipStatus(false, 0);
        MyWeapon = _data;
        MyWeapon.SetEquipStatus(true, 1);
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
        //寫入資料
        if (!LocalData)
        {
            ServerRequest.ChangeEquip(_data.UID, _data.EquipSlot, (origEquipData != null) ? origEquipData.UID : 0, 0);
        }
        else
        {
            EquipSaveLocalData();
        }
    }
    public static void Equip(ArmorData _data)
    {
        //執行更換裝備
        EquipData origEquipData = MyArmor;
        if (origEquipData != null)
            origEquipData.SetEquipStatus(false, 0);
        MyArmor = _data;
        MyArmor.SetEquipStatus(true, 2);
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
        //寫入資料
        if (!LocalData)
        {
            ServerRequest.ChangeEquip(_data.UID, _data.EquipSlot, (origEquipData != null) ? origEquipData.UID : 0, 0);
        }
        else
        {
            EquipSaveLocalData();
        }
    }
    public static void Equip(AccessoryData _data, int _index)
    {
        //執行更換裝備
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        EquipData origEquipData = MyAccessorys[_index];
        if (origEquipData != null)
            origEquipData.SetEquipStatus(false, 0);
        MyAccessorys[_index] = _data;
        MyAccessorys[_index].SetEquipStatus(true, 3 + _index);
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
        //寫入資料
        if (!LocalData)
        {
            ServerRequest.ChangeEquip(_data.UID, _data.EquipSlot, (origEquipData != null) ? origEquipData.UID : 0, 0);
        }
        else
        {
            EquipSaveLocalData();
        }
    }
    public static void TakeOff(WeaponData _data)
    {
        //執行更換裝備
        if (MyWeapon != null)
            MyWeapon.SetEquipStatus(false, 0);
        MyWeapon = null;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Minus);
        //寫入資料
        if (!LocalData)
        {
            ServerRequest.ChangeEquip(_data.UID, 0, 0, 0);
        }
        else
        {
            EquipSaveLocalData();
        }        
    }
    public static void TakeOff(ArmorData _data)
    {
        //執行更換裝備
        if (MyArmor != null)
            MyArmor.SetEquipStatus(false, 0);
        MyArmor = null;
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Minus);
        //寫入資料
        if (!LocalData)
        {
            ServerRequest.ChangeEquip(_data.UID, 0, 0, 0);
        }
        else
        {
            EquipSaveLocalData();
        } 
    }
    public static void TakeOff(AccessoryData _data, int _index)
    {
        //執行更換裝備
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        if (MyAccessorys[_index] != null)
            MyAccessorys[_index].SetEquipStatus(false, 0);
        MyAccessorys[_index] = null;
        //寫入資料
        if (!LocalData)
        {
            ServerRequest.ChangeEquip(_data.UID, 0, 0, 0);
        }
        else
        {
            EquipSaveLocalData();
        } 
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
}
