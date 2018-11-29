using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class PurchaseData : Data
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
            return GameDictionary.String_PurchaseDic[ID.ToString()].GetString(0, Player.UseLanguage);
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
            return GameDictionary.String_PurchaseDic[ID.ToString()].GetString(1, Player.UseLanguage);
        }
        private set { return; }
    }
    public int Gain { get; private set; }
    public int PayEmerald { get; private set; }
    public int PayKreds { get; private set; }
    public PurchaseType MyType;
    public string IconString;
    const string ImagePath = "Images/Main/{0}";

    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, PurchaseData> _dic, string _dataName)
    {
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", _dataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[_dataName];
        for (int i = 0; i < items.Count; i++)
        {
            PurchaseData data = new PurchaseData(items[i]);
            data.DataName = _dataName;
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }
    }
    public void SetPurchasePrice(int _price)
    {
        PayKreds = _price;
    }
    PurchaseData(JsonData _item)
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
                    case "Gain":
                        Gain = int.Parse(item[key].ToString());
                        break;
                    case "PayEmerald":
                        PayEmerald = int.Parse(item[key].ToString());
                        break;
                    case "Type":
                        MyType = MyEnum.ParseEnum<PurchaseType>(item[key].ToString());
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
    public Sprite GetICON()
    {
        return Resources.Load<Sprite>(string.Format(ImagePath, IconString));
    }
}
