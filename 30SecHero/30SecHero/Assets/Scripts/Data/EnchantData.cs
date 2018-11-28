using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class EnchantData : Data
{
    public string Name
    {
        get
        {
            if (!GameDictionary.String_EnchantDic.ContainsKey(ID.ToString()))
            {
                Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
                return "NullText";
            }
            return GameDictionary.String_EnchantDic[ID.ToString()].GetString(0, Player.UseLanguage);
        }
        private set { return; }
    }
    public string Description(int _offset)
    {
        if (!GameDictionary.String_EnchantDic.ContainsKey(ID.ToString()))
        {
            Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
            return "NullText";
        }
        string valueString = "";
        if (ShowPercentage)
            valueString = string.Format("{0}{1}", TextManager.ToPercent(BaseValue + (LV + _offset) * LevelUpValue).ToString("0.0"), "%");
        else
            valueString = string.Format("{0}", BaseValue + (LV + _offset) * LevelUpValue);
        return string.Format(GameDictionary.String_EnchantDic[ID.ToString()].GetString(1, Player.UseLanguage), valueString);
    }
    bool ShowPercentage;
    public EnchantProperty PropertyType;
    public EnchantType MyEnchantType;
    public Dictionary<EnchantProperty, float> Properties = new Dictionary<EnchantProperty, float>();
    public float BaseValue;
    public float LevelUpValue;
    public int BaseEmerald;
    public int LevelUpEmerald;
    public int MaxLevel;
    public string IconString;
    public int LV { get; private set; }
    public string GetLVString(int _plus)
    {
        return string.Format("{0}{1}", StringData.GetString("LV"), LV + _plus);
    }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, EnchantData> _dic, string _dataName)
    {
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", _dataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[_dataName];
        for (int i = 0; i < items.Count; i++)
        {
            EnchantData data = new EnchantData(items[i]);
            data.DataName = _dataName;
            data.Properties.Add(data.PropertyType, 0);
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }

    }
    EnchantData(JsonData _item)
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
                    case "EnchantType":
                        PropertyType = (EnchantProperty)Enum.Parse(typeof(EnchantProperty), item[key].ToString());
                        break;
                    case "BaseValue":
                        BaseValue = float.Parse(item[key].ToString());
                        break;
                    case "LevelUpValue":
                        LevelUpValue = float.Parse(item[key].ToString());
                        break;
                    case "ShowPercentage":
                        if (bool.Parse(item[key].ToString()))
                            ShowPercentage = true;
                        break;
                    case "BaseEmerald":
                        BaseEmerald = int.Parse(item[key].ToString());
                        break;
                    case "LevelUpEmerald":
                        LevelUpEmerald = int.Parse(item[key].ToString());
                        break;
                    case "MaxLevel":
                        MaxLevel = int.Parse(item[key].ToString());
                        break;
                    case "Icon":
                        IconString = item[key].ToString();
                        break;
                    case "Type":
                        MyEnchantType = MyEnum.ParseEnum<EnchantType>(item[key].ToString());
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
    public void InitSet(int _lv)
    {
        if (_lv < 0)
            return;
        LV = _lv;
        Properties[PropertyType] = GetValue();
    }
    public void LVUP()
    {
        LV++;
        Properties[PropertyType] = GetValue();
    }
    public bool CanUpgrade()
    {
        if (Player.Emerald < GetPrice() || LV >= MaxLevel)
            return false;
        else
            return true;
    }
    public int GetPrice()
    {
        return BaseEmerald + LV * LevelUpEmerald;
    }
    public float GetValue()
    {
        if (LV > 0)
            return BaseValue + (LV - 1) * LevelUpValue;
        else
            return 0;
    }
    public Sprite GetICON()
    {
        return Resources.Load<Sprite>(string.Format(GameSettingData.EnchantPath, IconString));
    }
    public static Dictionary<int, EnchantData> GetNewEnchantDic(int _lv)
    {
        Dictionary<int, EnchantData> dic = new Dictionary<int, EnchantData>();
        List<int> keys = new List<int>(GameDictionary.EnchantDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            dic.Add(keys[i], GetNewEnchantData(keys[i], _lv));
        }
        return dic;
    }
    public static EnchantData GetNewEnchantData(int _id, int _lv)
    {
        EnchantData data = GameDictionary.EnchantDic[_id].MemberwiseClone() as EnchantData;
        data.InitSet(_lv);
        return data;
    }
}
