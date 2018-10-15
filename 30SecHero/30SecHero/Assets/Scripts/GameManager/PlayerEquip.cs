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
}
