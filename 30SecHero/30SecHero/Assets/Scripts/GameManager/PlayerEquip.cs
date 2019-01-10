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
    public static int ItemCout
    {
        get
        {
            int count = 0;
            if (Itmes.ContainsKey(EquipType.Weapon))
                count += Itmes[EquipType.Weapon].Count;
            if (Itmes.ContainsKey(EquipType.Armor))
                count += Itmes[EquipType.Armor].Count;
            if (Itmes.ContainsKey(EquipType.Accessory))
                count += Itmes[EquipType.Accessory].Count;
            return count;
        }
    }
    public static void EquipSaveLocalData()
    {
        //武器文字
        List<long> keys = new List<long>(Itmes[EquipType.Weapon].Keys);
        string dataStr = "";
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            dataStr += Itmes[EquipType.Weapon][keys[i]].UID + "," + Itmes[EquipType.Weapon][keys[i]].ID + "," + (int)Itmes[EquipType.Weapon][keys[i]].Type + "," + Itmes[EquipType.Weapon][keys[i]].EquipSlot + "," + Itmes[EquipType.Weapon][keys[i]].LV + "," + Itmes[EquipType.Weapon][keys[i]].Quality + "," + Itmes[EquipType.Weapon][keys[i]].PropertiesStr;
            if (Itmes[EquipType.Weapon][keys[i]].MyEnchant != null)
                dataStr += "," + Itmes[EquipType.Weapon][keys[i]].MyEnchant.ID;
        }
        //防具文字
        keys = new List<long>(Itmes[EquipType.Armor].Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            else
                if (dataStr != "")
                    dataStr += "/";
            dataStr += Itmes[EquipType.Armor][keys[i]].UID + "," + Itmes[EquipType.Armor][keys[i]].ID + "," + (int)Itmes[EquipType.Armor][keys[i]].Type + "," + Itmes[EquipType.Armor][keys[i]].EquipSlot + "," + Itmes[EquipType.Armor][keys[i]].LV + "," + Itmes[EquipType.Armor][keys[i]].Quality + "," + Itmes[EquipType.Armor][keys[i]].PropertiesStr;
            if (Itmes[EquipType.Armor][keys[i]].MyEnchant != null)
                dataStr += "," + Itmes[EquipType.Armor][keys[i]].MyEnchant.ID;
        }
        //飾品文字
        keys = new List<long>(Itmes[EquipType.Accessory].Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            else
                if (dataStr != "")
                    dataStr += "/";
            dataStr += Itmes[EquipType.Accessory][keys[i]].UID + "," + Itmes[EquipType.Accessory][keys[i]].ID + "," + (int)Itmes[EquipType.Accessory][keys[i]].Type + "," + Itmes[EquipType.Accessory][keys[i]].EquipSlot + "," + Itmes[EquipType.Accessory][keys[i]].LV + "," + Itmes[EquipType.Accessory][keys[i]].Quality + "," + Itmes[EquipType.Accessory][keys[i]].PropertiesStr;
            if (Itmes[EquipType.Accessory][keys[i]].MyEnchant != null)
                dataStr += "," + Itmes[EquipType.Accessory][keys[i]].MyEnchant.ID;
        }
        PlayerPrefs.SetString(LocoData.Equip.ToString(), dataStr);
    }
    public static void Equip(WeaponData _data)
    {
        //執行更換裝備
        EquipData origEquipData = MyWeapon;
        if (origEquipData != null)
        {
            GameSettingData.RolePropertyOperate(EquipPlus, origEquipData.Properties, Operator.Minus);
            origEquipData.SetEquipStatus(false, 0);
        }
        MyWeapon = _data;
        MyWeapon.SetEquipStatus(true, 1);
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
        //寫入資料
        if (EquipInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新Server玩家裝備");
                ServerRequest.ChangeEquip(_data.UID, _data.EquipSlot, (origEquipData != null) ? origEquipData.UID : 0, 0);
            }
            else
            {
                Debug.Log("更新Loco玩家裝備");
                EquipSaveLocalData();
            }
        }
        RefreshEquipEnchant();
        if (EquipPlus[RoleProperty.EquipDrop] > 20)
        {
            Debug.LogError("測試");
            Debug.Log("EquipPlus[RoleProperty.EquipDrop]=" + EquipPlus[RoleProperty.EquipDrop]);
        }
    }
    public static void Equip(ArmorData _data)
    {
        //執行更換裝備
        EquipData origEquipData = MyArmor;
        if (origEquipData != null)
        {
            GameSettingData.RolePropertyOperate(EquipPlus, origEquipData.Properties, Operator.Minus);
            origEquipData.SetEquipStatus(false, 0);
        }
        MyArmor = _data;
        MyArmor.SetEquipStatus(true, 2);
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
        //寫入資料
        if (EquipInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新Server玩家裝備");
                ServerRequest.ChangeEquip(_data.UID, _data.EquipSlot, (origEquipData != null) ? origEquipData.UID : 0, 0);
            }
            else
            {
                Debug.Log("更新Loco玩家裝備");
                EquipSaveLocalData();
            }
        }
        RefreshEquipEnchant();
    }
    public static void Equip(AccessoryData _data, int _index)
    {
        //執行更換裝備
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        EquipData origEquipData = MyAccessorys[_index];
        if (origEquipData != null)
        {
            GameSettingData.RolePropertyOperate(EquipPlus, origEquipData.Properties, Operator.Minus);
            origEquipData.SetEquipStatus(false, 0);
        }
        MyAccessorys[_index] = _data;
        MyAccessorys[_index].SetEquipStatus(true, 3 + _index);
        GameSettingData.RolePropertyOperate(EquipPlus, _data.Properties, Operator.Plus);
        //寫入資料
        if (EquipInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新Server玩家裝備");
                ServerRequest.ChangeEquip(_data.UID, _data.EquipSlot, (origEquipData != null) ? origEquipData.UID : 0, 0);
            }
            else
            {
                Debug.Log("更新Loco玩家裝備");
                EquipSaveLocalData();
            }
        }
        RefreshEquipEnchant();
    }
    public static void TakeOff(EquipType _type, int _index)
    {
        //執行更換裝備
        if (_index < 0 || _index > MyAccessorys.Length)
            return;
        EquipData originalEquip = null;
        switch (_type)
        {
            case EquipType.Weapon:
                originalEquip = Player.MyWeapon;
                MyWeapon = null;
                break;
            case EquipType.Armor:
                originalEquip = Player.MyArmor;
                MyArmor = null;
                break;
            case EquipType.Accessory:
                originalEquip = Player.MyAccessorys[_index];
                Player.MyAccessorys[_index] = null;
                break;
        }
        if (originalEquip != null)
        {
            originalEquip.SetEquipStatus(false, 0);
            GameSettingData.RolePropertyOperate(EquipPlus, originalEquip.Properties, Operator.Minus);
            //寫入資料
            if (EquipInitDataFinish)
            {
                if (!LocalData)
                {
                    Debug.Log("更新Server玩家裝備");
                    ServerRequest.ChangeEquip(originalEquip.UID, 0, 0, 0);
                }
                else
                {
                    Debug.Log("更新Loco玩家裝備");
                    EquipSaveLocalData();
                }
            }
        }
        RefreshEquipEnchant();
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
        //寫入資料
        string id = _data.UID.ToString();
        if (EquipInitDataFinish)
        {
            if (!LocalData)
            {
                if (id != "")
                {
                    Debug.Log("更新Server玩家裝備");
                    ServerRequest.SellEquip(id);
                }
            }
            else
            {
                Debug.Log("更新Loco玩家裝備");
                EquipSaveLocalData();
            }
        }
    }
    public static void SellEquips(List<EquipData> _datas)
    {
        //執行販賣裝備
        string ids = "";
        bool getFirstEquip = false;
        for (int i = 0; i < _datas.Count; i++)
        {
            if (Itmes[_datas[i].Type].ContainsKey(_datas[i].UID))
            {
                if (getFirstEquip)
                    ids += ",";
                ids += _datas[i].UID.ToString();
                getFirstEquip = true;
                GainGold(_datas[i].SellGold);
                Itmes[_datas[i].Type].Remove(_datas[i].UID);
            }
            else
                Debug.LogWarning("Sell Equip isn't in Items");
        }
        //寫入資料
        if (EquipInitDataFinish)
        {
            if (!LocalData)
            {
                if (ids != "")
                {
                    Debug.Log("更新Server玩家裝備");
                    ServerRequest.SellEquip(ids);
                }
            }
            else
            {
                Debug.Log("更新Loco玩家裝備");
                EquipSaveLocalData();
            }
        }
    }
}
