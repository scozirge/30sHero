using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public abstract class EquipData : Data
{
    public int UID;
    protected static int MaxUID;//只使用本地資料才會用到
    public int EquipSlot { get; protected set; }
    public virtual EquipType Type { get; protected set; }
    public virtual string Name { get; protected set; }
    public EnchantData MyEnchant;

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
    public string PropertiesStr { get; protected set; }
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
    public static EquipData GetRandomNewEquip(int _lv, int _quality,EquipType _type)
    {
        int equipType = (int)_type;
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
    //server回傳UID時使用
    public void SetUID(int _uid)
    {
        UID = _uid;
    }
    protected static void UpdateMaxUID(int _uid)
    {
        if (_uid > MaxUID)
            MaxUID = _uid;
    }
    protected void SetPropertiesByStr()
    {
        Properties = GameSettingData.GetNewRolePropertiesDic(0);
        if (PropertiesStr == "")
            return;
        string[] properties = PropertiesStr.Split('&');
        for (int i = 0; i < properties.Length; i++)
        {
            string[] values = properties[i].Split('=');
            RoleProperty type = GameSettingData.RandomPropertyList[int.Parse(values[0])];
            float value = float.Parse(values[1]);
            Properties[type] = value;
        }
    }
    public virtual string GetPropertiesStr()
    {
        string str = "";
        for (int i = 0; i < GameSettingData.RandomPropertyList.Count; i++)
        {
            if (Properties.ContainsKey(GameSettingData.RandomPropertyList[i]))
            {
                //Debug.Log(GameSettingData.RandomPropertyList[i]);
                if (Properties[GameSettingData.RandomPropertyList[i]] != 0)
                {
                    if (str != "")
                        str += "&";
                    str += i + "=" + Properties[GameSettingData.RandomPropertyList[i]];
                }
            }
        }
        return str;
    }
    protected virtual void SetRandomEnchant()
    {
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
        if (MyEnchant != null)
        {
            PropertyText enchantPT = new PropertyText();
            enchantPT.Text = MyEnchant.Description(0);//MyEnchant.Name + "\r\n" + MyEnchant.Description(0)
            enchantPT.Comparison = Comparator.Equal;
            enchantPT.ColorCode = GameSettingData.EnchantTextColor;
            enchantPT.DisableSizeFilter = true;
            enchantPT.AutoHeighWithLineCount = true;
            enchantPT.Width = 550;
            list.Add(enchantPT);
        }
        for (int i = 0; i < GameSettingData.RandomPropertyList.Count; i++)
        {
            if (Properties[GameSettingData.RandomPropertyList[i]] == 0)
                continue;
            PropertyText pt = new PropertyText();
            pt.Height = 60;
            float value = GetVealue(GameSettingData.RandomPropertyList[i], Properties[GameSettingData.RandomPropertyList[i]]);
            if (value >= 0)
                pt.Text = string.Format("+{1}{2} {0}{3}", StringData.GetString(GameSettingData.RandomPropertyList[i].ToString()), value, GetUnit(GameSettingData.RandomPropertyList[i]), GetUnitAfterTitle(GameSettingData.RandomPropertyList[i]));
            else
                pt.Text = string.Format("{1}{2} {0}{3}", StringData.GetString(GameSettingData.RandomPropertyList[i].ToString()), value, GetUnit(GameSettingData.RandomPropertyList[i]), GetUnitAfterTitle(GameSettingData.RandomPropertyList[i]));
            pt.Comparison = Comparator.Equal;
            pt.ColorCode = GameSettingData.NormalNumberColor;
            list.Add(pt);
        }
        return list;
    }
    public List<PropertyText> GetPropertyTextList(EquipData _data)
    {
        List<PropertyText> list = new List<PropertyText>();
        if(MyEnchant!=null)
        {
            PropertyText enchantPT = new PropertyText();
            enchantPT.Text = MyEnchant.Description(0);//MyEnchant.Name + "\r\n" + MyEnchant.Description(0)
            enchantPT.Comparison = Comparator.Equal;
            enchantPT.ColorCode = GameSettingData.EnchantTextColor;
            enchantPT.DisableSizeFilter = true;
            enchantPT.AutoHeighWithLineCount = true;
            enchantPT.Width = 550;
            list.Add(enchantPT);
        }

        for (int i = 0; i < GameSettingData.RandomPropertyList.Count; i++)
        {
            
            if (Properties[GameSettingData.RandomPropertyList[i]] == 0)//if (Properties[GameSettingData.RandomPropertyList[i]] == 0 && _data.Properties[GameSettingData.RandomPropertyList[i]] == 0)比較兩個裝備的數值
                continue;
            float valueDiff = Properties[GameSettingData.RandomPropertyList[i]] - _data.Properties[GameSettingData.RandomPropertyList[i]];
            PropertyText pt = new PropertyText();
            pt.Height = 60;
            float value = GetVealue(GameSettingData.RandomPropertyList[i], Properties[GameSettingData.RandomPropertyList[i]]);
            if (value >= 0)
                pt.Text = string.Format("+{1}{2} {0}{3}", StringData.GetString(GameSettingData.RandomPropertyList[i].ToString()), value, GetUnit(GameSettingData.RandomPropertyList[i]), GetUnitAfterTitle(GameSettingData.RandomPropertyList[i]));
            else
                pt.Text = string.Format("{1}{2} {0}{3}", StringData.GetString(GameSettingData.RandomPropertyList[i].ToString()), value, GetUnit(GameSettingData.RandomPropertyList[i]), GetUnitAfterTitle(GameSettingData.RandomPropertyList[i]));
            if (valueDiff > 0)
            {
                pt.Comparison = Comparator.Equal;//Comparator.Greater;
                pt.ColorCode = GameSettingData.NormalNumberColor; //GameSettingData.GrowingNumberColor;
            }
            else if (valueDiff < 0)
            {
                pt.Comparison = Comparator.Equal; //Comparator.Less;
                pt.ColorCode = GameSettingData.NormalNumberColor; //GameSettingData.DropingNumberColor;
            }
            else
            {
                pt.Comparison = Comparator.Equal;
                pt.ColorCode = GameSettingData.NormalNumberColor;
            }
            list.Add(pt);
        }
        return list;
    }
    float GetVealue(RoleProperty _type, float _value)
    {
        switch (_type)
        {
            case RoleProperty.ShieldRecovery:
                _value *= 100;
                break;
            case RoleProperty.ShieldReChargeTime:
                _value *= -1;
                break;
            case RoleProperty.SkillDrop:
                _value *= 100;
                break;
            case RoleProperty.BloodThirsty:
                _value *= 100;
                break;
            case RoleProperty.PotionEfficiency:
                _value *= 100;
                break;
        }
        return _value;
    }
    string GetUnit(RoleProperty _type)
    {
        string str = "";
        switch (_type)
        {
            case RoleProperty.ShieldReChargeTime:
                str = StringData.GetString("Sec");
                break;
            case RoleProperty.ShieldRecovery:
                str = StringData.GetString("Percent");
                break;
            case RoleProperty.MoveSpeed:
                //str = StringData.GetString("Meter") + StringData.GetString("Divide") + StringData.GetString("Sec");
                break;
            case RoleProperty.MaxMoveSpeed:
                //str = StringData.GetString("Meter") + StringData.GetString("Divide") + StringData.GetString("Sec");
                break;
            case RoleProperty.MoveDecay:
                str = StringData.GetString("Sec");
                break;
            case RoleProperty.AvatarTime:
                str = StringData.GetString("Sec");
                break;
            case RoleProperty.AvatarPotionBuff:
                str = StringData.GetString("Sec");
                break;
            case RoleProperty.SkillTimeBuff:
                str = StringData.GetString("Sec");
                break;
            case RoleProperty.SkillDrop:
                str = StringData.GetString("Percent");
                break;
            case RoleProperty.GoldDrop:
                str = StringData.GetString("G");
                break;
            case RoleProperty.BloodThirsty:
                str = StringData.GetString("Percent");
                break;
            case RoleProperty.PotionEfficiency:
                str = StringData.GetString("Percent");
                break;
        }
        return str;
    }
    string GetUnitAfterTitle(RoleProperty _type)
    {
        string str = "";
        switch (_type)
        {
            case RoleProperty.ShieldRecovery:
                //str = StringData.GetString("Per") + StringData.GetString("Sec");
                break;
        }
        return str;
    }
}
