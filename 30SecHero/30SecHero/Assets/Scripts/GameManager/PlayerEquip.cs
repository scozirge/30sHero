using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class Player
{
    //Equip
    public static WeaponData MyWeapon;
    public static ArmorData MyArmor;
    public static AccessoryData[] MyAccessorys = new AccessoryData[2] { null, null };
    //Items
    public static Dictionary<EquipType, Dictionary<long, EquipData>> Items = new Dictionary<EquipType, Dictionary<long, EquipData>>();
    public static int ItemCout
    {
        get
        {
            int count = 0;
            if (Items.ContainsKey(EquipType.Weapon))
                count += Items[EquipType.Weapon].Count;
            if (Items.ContainsKey(EquipType.Armor))
                count += Items[EquipType.Armor].Count;
            if (Items.ContainsKey(EquipType.Accessory))
                count += Items[EquipType.Accessory].Count;
            return count;
        }
    }
    public static long CurMaxWeaponUID { get; private set; }
    public static long CurMaxArmorUID { get; private set; }
    public static long CurMaxAccessoryUID { get; private set; }
    public static void UudateCurMaxEquipUID(EquipType _type, int _newUID)
    {
        switch (_type)
        {
            case EquipType.Weapon:
                if (_newUID > Player.CurMaxWeaponUID)
                    Player.CurMaxWeaponUID = _newUID;
                Debug.Log("Player.CurMaxWeaponUID=" + Player.CurMaxWeaponUID);
                break;
            case EquipType.Armor:
                if (_newUID > Player.CurMaxArmorUID)
                    Player.CurMaxArmorUID = _newUID;
                Debug.Log("Player.CurMaxArmorUID=" + Player.CurMaxArmorUID);
                break;
            case EquipType.Accessory:
                if (_newUID > Player.CurMaxAccessoryUID)
                    Player.CurMaxAccessoryUID = _newUID;
                Debug.Log("Player.CurMaxAccessoryUID=" + Player.CurMaxAccessoryUID);
                break;
        }
    }
    public static long GetCurMaxEquipUID(EquipType _type)
    {
        switch (_type)
        {
            case EquipType.Weapon:
                return CurMaxWeaponUID;
            case EquipType.Armor:
                return CurMaxArmorUID;
            case EquipType.Accessory:
                return CurMaxAccessoryUID;
            default:
                return 0;
        }
    }
    public static void UpToDateCurMaxEquipUID()
    {
        if (Items.ContainsKey(EquipType.Weapon) && Items[EquipType.Weapon].Count != 0)
            CurMaxWeaponUID = Items[EquipType.Weapon].Keys.Max();
        if (Items.ContainsKey(EquipType.Armor) && Items[EquipType.Armor].Count != 0)
            CurMaxArmorUID = Items[EquipType.Armor].Keys.Max();
        if (Items.ContainsKey(EquipType.Accessory) && Items[EquipType.Accessory].Count != 0)
            CurMaxAccessoryUID = Items[EquipType.Accessory].Keys.Max();
    }
    public static void EquipSaveLocalData()
    {
        //武器文字
        List<long> keys = new List<long>(Items[EquipType.Weapon].Keys);
        string dataStr = "";
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            dataStr += Items[EquipType.Weapon][keys[i]].UID + "," + Items[EquipType.Weapon][keys[i]].ID + "," + (int)Items[EquipType.Weapon][keys[i]].Type + "," + Items[EquipType.Weapon][keys[i]].EquipSlot + "," + Items[EquipType.Weapon][keys[i]].LV + "," + Items[EquipType.Weapon][keys[i]].Quality + "," + Items[EquipType.Weapon][keys[i]].PropertiesStr;
            if (Items[EquipType.Weapon][keys[i]].MyEnchant != null)
                dataStr += "," + Items[EquipType.Weapon][keys[i]].MyEnchant.ID;
        }
        //防具文字
        keys = new List<long>(Items[EquipType.Armor].Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            else
                if (dataStr != "")
                    dataStr += "/";
            dataStr += Items[EquipType.Armor][keys[i]].UID + "," + Items[EquipType.Armor][keys[i]].ID + "," + (int)Items[EquipType.Armor][keys[i]].Type + "," + Items[EquipType.Armor][keys[i]].EquipSlot + "," + Items[EquipType.Armor][keys[i]].LV + "," + Items[EquipType.Armor][keys[i]].Quality + "," + Items[EquipType.Armor][keys[i]].PropertiesStr;
            if (Items[EquipType.Armor][keys[i]].MyEnchant != null)
                dataStr += "," + Items[EquipType.Armor][keys[i]].MyEnchant.ID;
        }
        //飾品文字
        keys = new List<long>(Items[EquipType.Accessory].Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0)
                dataStr += "/";
            else
                if (dataStr != "")
                    dataStr += "/";
            dataStr += Items[EquipType.Accessory][keys[i]].UID + "," + Items[EquipType.Accessory][keys[i]].ID + "," + (int)Items[EquipType.Accessory][keys[i]].Type + "," + Items[EquipType.Accessory][keys[i]].EquipSlot + "," + Items[EquipType.Accessory][keys[i]].LV + "," + Items[EquipType.Accessory][keys[i]].Quality + "," + Items[EquipType.Accessory][keys[i]].PropertiesStr;
            if (Items[EquipType.Accessory][keys[i]].MyEnchant != null)
                dataStr += "," + Items[EquipType.Accessory][keys[i]].MyEnchant.ID;
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
        Debug.Log(_data.ID);
        if (Items[_data.Type].ContainsKey(_data.UID))
        {
            GainGold(_data.SellGold);
            Items[_data.Type].Remove(_data.UID);
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
            if (Items[_datas[i].Type].ContainsKey(_datas[i].UID))
            {
                if (getFirstEquip)
                    ids += ",";
                ids += _datas[i].UID.ToString();
                getFirstEquip = true;
                GainGold(_datas[i].SellGold);
                Items[_datas[i].Type].Remove(_datas[i].UID);
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
