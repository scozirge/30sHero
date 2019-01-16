using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class StrengthenData : Data
{
    public string Name
    {
        get
        {
            if (!GameDictionary.String_StrengthenDic.ContainsKey(ID.ToString()))
            {
                Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
                return "NullText";
            }
            return GameDictionary.String_StrengthenDic[ID.ToString()].GetString(0, Player.UseLanguage);
        }
        private set { return; }
    }
    public string Description()
    {
        if (!GameDictionary.String_StrengthenDic.ContainsKey(ID.ToString()))
        {
            Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
            return "NullText";
        }
        string valueString = "";
        if (ShowPercentage)
            valueString = string.Format("{0}{1}{2}{3}{4}", TextManager.ToPercent(BaseValue + (LV) * LevelUpValue).ToString("0"), StringData.GetString("Percent"), StringData.GetString("Arrow2"), TextManager.ToPercent(BaseValue + (LV + 1) * LevelUpValue).ToString("0"), StringData.GetString("Percent"));
        else
            valueString = string.Format("{0}{1}{2}", BaseValue + (LV) * LevelUpValue, StringData.GetString("Arrow2"), BaseValue + (LV + 1) * LevelUpValue);
        return string.Format(GameDictionary.String_StrengthenDic[ID.ToString()].GetString(1, Player.UseLanguage), valueString);
    }
    public string Description(int _offset)
    {
        if (!GameDictionary.String_StrengthenDic.ContainsKey(ID.ToString()))
        {
            Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
            return "NullText";
        }
        string valueString = "";
        if (ShowPercentage)
            valueString = string.Format("{0}{1}", TextManager.ToPercent(BaseValue + (LV + _offset) * LevelUpValue).ToString("0"), StringData.GetString("Percent"));
        else
            valueString = string.Format("{0}", BaseValue + (LV + _offset) * LevelUpValue);
        return string.Format(GameDictionary.String_StrengthenDic[ID.ToString()].GetString(1, Player.UseLanguage), valueString);
    }
    bool ShowPercentage;
    public RoleProperty PropertyType;
    public Dictionary<RoleProperty, float> Properties = new Dictionary<RoleProperty, float>();
    public float BaseValue;
    public float LevelUpValue;
    public int BaseGold;
    public int LevelUpGold;
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
    public static void SetData(Dictionary<int, StrengthenData> _dic, string _dataName)
    {
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", _dataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[_dataName];
        for (int i = 0; i < items.Count; i++)
        {
            StrengthenData data = new StrengthenData(items[i]);
            data.DataName = _dataName;
            data.Properties.Add(data.PropertyType, 0);
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }

    }
    StrengthenData(JsonData _item)
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
                    case "StrengthenType":
                        PropertyType = (RoleProperty)Enum.Parse(typeof(RoleProperty), item[key].ToString());
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
                    case "BaseGold":
                        BaseGold = int.Parse(item[key].ToString());
                        break;
                    case "LevelUpGold":
                        LevelUpGold = int.Parse(item[key].ToString());
                        break;
                    case "MaxLevel":
                        MaxLevel = int.Parse(item[key].ToString());
                        break;
                    case "Icon":
                        IconString = item[key].ToString();
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
        if (LV >= MaxLevel)
            return false;
        else
            return true;
    }
    public int GetPrice()
    {
        return BaseGold + LV * LevelUpGold;
    }
    public float GetValue()
    {
        if (LV > 0)
            return BaseValue + LV * LevelUpValue;
        else
            return 0;
    }
    public Sprite GetICON()
    {
        return Resources.Load<Sprite>(string.Format(GameSettingData.StrengthenPath, IconString));
    }
    public static Dictionary<int, StrengthenData> GetNewStrengthenDic(int _lv)
    {
        Dictionary<int, StrengthenData> dic = new Dictionary<int, StrengthenData>();
        List<int> keys = new List<int>(GameDictionary.StrengthenDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            dic.Add(keys[i], GetNewStrengthenData(keys[i], _lv));
        }
        return dic;
    }
    public static StrengthenData GetNewStrengthenData(int _id, int _lv)
    {
        StrengthenData data = GameDictionary.StrengthenDic[_id].MemberwiseClone() as StrengthenData;
        data.InitSet(_lv);
        return data;
    }
}
