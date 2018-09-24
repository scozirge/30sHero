using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class ArmorData : EquipData
{
    public override EquipType Type { get { return EquipType.Armor; } }
    static long MaxUID;
    public override int BaseHealth
    {
        get
        {
            return GameSettingData.GetArmorHealth(LV);
        }
    }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, ArmorData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            ArmorData data = new ArmorData(items[i]);
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }
    }
    public override string Name
    {
        get
        {
            if (!GameDictionary.String_ArmorDic.ContainsKey(ID.ToString()))
            {
                Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
                return "NullText";
            }
            return GameDictionary.String_ArmorDic[ID.ToString()].GetString(Player.UseLanguage);
        }
        protected set { return; }
    }
    public int Defence
    {
        get
        {
            return GameSettingData.GetArmorHealth(LV);
        }
    }
    public int Gold
    {
        get
        {
            return GameSettingData.GetArmorGold(LV, Quality);
        }
    }
    public ArmorData(JsonData _item)
        : base(_item)
    {
    }
    public static ArmorData GetNewArmor(int _id, int _lv, int _quality, bool _isEquiped)
    {
        ArmorData data = GameDictionary.ArmorDic[_id].MemberwiseClone() as ArmorData;
        MaxUID++;
        data.UID = MaxUID;
        data.LV = _lv;
        data.Quality = _quality;
        data.IsEquiped = _isEquiped;
        data.SetRandomProperties();
        return data;
    }

}
