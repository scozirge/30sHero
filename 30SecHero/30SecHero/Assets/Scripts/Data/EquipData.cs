using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public abstract class EquipData : Data
{
    public int UID;
    public int EquipSlot { get; protected set; }
    public virtual EquipType Type { get; protected set; }
    public virtual string Name { get; protected set; }

    //string[] IconStrings = new string[3];
    public Sprite[] Icons = new Sprite[3];
    public int Quality;
    public int LV;
    public virtual int SellGold { get; }
    public string GetLVString()
    {
        return string.Format("{0}{1}", GameDictionary.String_UIDic["LV"].GetString(Player.UseLanguage), LV);
    }
    public bool IsEquiped { get; protected set; }
    public Dictionary<RoleProperty, float> Properties = new Dictionary<RoleProperty, float>();
    public virtual void SetEquipStatus(bool _isEquiped, int _equipSlot)
    {
        IsEquiped = _isEquiped;
    }
    protected EquipData(JsonData _item)
    {
        try
        {
            JsonData item = _item;
            foreach (string key in item.Keys)
            {
                switch (key)
                {
                    case "ID":
                        ID = int.Parse(item[key].ToString());
                        break;
                    case "Icon1":
                        //IconStrings[0] = item[key].ToString();
                        Icons[0] = Resources.Load<Sprite>(string.Format(GameSettingData.GetEquipIconPath(Type), item[key].ToString()));
                        if (Icons[0] == null)
                            Debug.LogWarning(string.Format("裝備路徑找不到ICON{0}", string.Format(GameSettingData.GetEquipIconPath(Type), item[key].ToString())));
                        break;
                    case "Icon2":
                        //IconStrings[1] = item[key].ToString();
                        Icons[1] = Resources.Load<Sprite>(string.Format(GameSettingData.GetEquipIconPath(Type), item[key].ToString()));
                        if (Icons[1] == null)
                            Debug.LogWarning(string.Format("裝備路徑找不到ICON{0}", string.Format(GameSettingData.GetEquipIconPath(Type), item[key].ToString())));
                        break;
                    case "Icon3":
                        //IconStrings[2] = item[key].ToString();
                        Icons[2] = Resources.Load<Sprite>(string.Format(GameSettingData.GetEquipIconPath(Type), item[key].ToString()));
                        if (Icons[2] == null)
                            Debug.LogWarning(string.Format("裝備路徑找不到ICON{0}", string.Format(GameSettingData.GetEquipIconPath(Type), item[key].ToString())));
                        break;
                    default:
                        Debug.LogWarning(string.Format("{0}表有不明屬性:{1}", DataName, key));
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static EquipData GetRandomNewEquip(int _lv, int _quality)
    {
        int equipType = GameSettingData.GetRandomEquipType();
        switch ((EquipType)equipType)
        {
            case EquipType.Weapon:
                return WeaponData.GetRandomNewWeapon(_lv, _quality);
            case EquipType.Armor:
                return ArmorData.GetRandomNewArmor(_lv, _quality);
            case EquipType.Accessory:
                return AccessoryData.GetRandomNewAccessory(_lv, _quality);
            default:
                Debug.LogWarning("隨機取得亂數裝備錯誤");
                return null;
        }
    }
    public virtual int SetUID()
    {
        return UID;
    }
    protected virtual void SetRandomProperties()
    {
        Properties = GameSettingData.GetNewRolePropertiesDic(0);

        Dictionary<RoleProperty, float> dic = GameSettingData.GetRandomEquipProperties(Quality, LV);
        List<RoleProperty> kes = new List<RoleProperty>(Properties.Keys);
        for (int i = 0; i < kes.Count; i++)
        {
            Properties[kes[i]] = dic[kes[i]];
        }
    }
    public List<PropertyText> GetPropertyTextList()
    {
        List<PropertyText> list = new List<PropertyText>();
        List<RoleProperty> kes = new List<RoleProperty>(Properties.Keys);
        for (int i = 0; i < kes.Count; i++)
        {
            if (Properties[kes[i]] == 0)
                continue;
            PropertyText pt = new PropertyText();
            pt.Text = string.Format("{0}+{1}", StringData.GetString(kes[i].ToString()), Properties[kes[i]]);
            pt.Comparison = Comparator.Equal;
            pt.ColorCode = GameSettingData.NormalNumberColor;
            list.Add(pt);
        }
        return list;
    }
    public List<PropertyText> GetPropertyTextList(EquipData _data)
    {
        List<PropertyText> list = new List<PropertyText>();
        List<RoleProperty> kes = new List<RoleProperty>(Properties.Keys);
        for (int i = 0; i < kes.Count; i++)
        {
            if (Properties[kes[i]] == 0 && _data.Properties[kes[i]] == 0)
                continue;
            float valueDiff = Properties[kes[i]] - _data.Properties[kes[i]];
            PropertyText pt = new PropertyText();
            if (valueDiff >= 0)
            {
                pt.Text = string.Format("{0}+{1}", StringData.GetString(kes[i].ToString()), Properties[kes[i]]);
                if (valueDiff > 0)
                {
                    pt.Comparison = Comparator.Greater;
                    pt.ColorCode = GameSettingData.GrowingNumberColor;
                }
                else
                {
                    pt.Comparison = Comparator.Equal;
                    pt.ColorCode = GameSettingData.NormalNumberColor;
                }
            }
            else if (valueDiff < 0)
            {
                pt.Text = string.Format("{0}{1}", StringData.GetString(kes[i].ToString()), Properties[kes[i]]);
                pt.Comparison = Comparator.Less;
                pt.ColorCode = GameSettingData.DropingNumberColor;
            }
            list.Add(pt);
        }
        return list;
    }
}
