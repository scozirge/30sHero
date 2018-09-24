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
    public string Description
    {
        get
        {
            if (!GameDictionary.String_StrengthenDic.ContainsKey(ID.ToString()))
            {
                Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
                return "NullText";
            }
            string valueString = "";
            if (ShowPercentage)
                valueString = string.Format("{0}{1}", TextManager.ToPercent(BaseValue + LV * LevelUpValue).ToString("0.0"), "%");
            else
                valueString = string.Format("{0}", BaseValue + LV * LevelUpValue);
            return string.Format(GameDictionary.String_StrengthenDic[ID.ToString()].GetString(1, Player.UseLanguage), valueString);
        }
        private set { return; }
    }
    bool ShowPercentage;
    public string StrengthenType;
    public float BaseValue;
    public float LevelUpValue;
    public int BaseGold;
    public int LevelUpGold;
    public string IconString;
    public int LV;
    public string GetLVString()
    {
        return string.Format("{0}{1}", GameDictionary.String_UIDic["LV"].GetString(Player.UseLanguage), LV);
    }
    const string ImagePath = "Images/Main/{0}";

    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, StrengthenData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            StrengthenData data = new StrengthenData(items[i]);
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
                        if (Player.StrengthenLVDic.ContainsKey(ID))
                            LV = Player.StrengthenLVDic[ID];
                        break;
                    case "StrengthenType":
                        StrengthenType = item[key].ToString();
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
    public int LevelUp()
    {
        return ++LV;
    }
    public int GetPrice()
    {
        return BaseGold + LV * LevelUpGold;
    }
    public Sprite GetICON()
    {
        return Resources.Load<Sprite>(string.Format(ImagePath, IconString));
    }
}
