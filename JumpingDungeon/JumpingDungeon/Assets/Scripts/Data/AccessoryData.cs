using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class AccessoryData : EquipData
{
    public override EquipType Type { get { return EquipType.Accessory; } }
    static long MaxUID;
    public override int SellGold { get { return GameSettingData.GetAccessoryGold(LV, Quality); } }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, AccessoryData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            AccessoryData data = new AccessoryData(items[i]);
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }
    }
    public override string Name
    {
        get
        {
            if (!GameDictionary.String_AccessoryDic.ContainsKey(ID.ToString()))
            {
                Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
                return "NullText";
            }
            return GameDictionary.String_AccessoryDic[ID.ToString()].GetString(Player.UseLanguage);
        }
        protected set { return; }
    }
    public int Gold
    {
        get
        {
            return GameSettingData.GetAccessoryGold(LV, Quality);
        }
    }
    protected override void SetRandomProperties()
    {
        base.SetRandomProperties();
        Properties[RoleProperty.Shield] += GameSettingData.GetAccessoryShield(LV);
    }
    public AccessoryData(JsonData _item)
        : base(_item)
    {
    }
    public static AccessoryData GetNewAccessory(int _id, int _lv, int _quality)
    {
        AccessoryData data = GameDictionary.AccessoryDic[_id].MemberwiseClone() as AccessoryData;
        MaxUID++;
        data.UID = MaxUID;
        data.LV = _lv;
        data.Quality = _quality;
        data.SetRandomProperties();
        return data;
    }
}
