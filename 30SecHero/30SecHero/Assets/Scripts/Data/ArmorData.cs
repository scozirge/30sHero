using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class ArmorData : EquipData
{
    public override EquipType Type { get { return EquipType.Armor; } }
    static long MaxUID;
    public override int SellGold { get { return GameSettingData.GetArmorGold(LV, Quality); } }
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
    public int Gold
    {
        get
        {
            return GameSettingData.GetArmorGold(LV, Quality);
        }
    }
    protected override void SetRandomProperties()
    {
        base.SetRandomProperties();
        Properties[RoleProperty.Health] += GameSettingData.GetArmorHealth(LV);
    }
    public ArmorData(JsonData _item)
        : base(_item)
    {
    }
    public static ArmorData GetNewArmor(int _uid, int _id, int _equipSlot, int _lv, int _quality)
    {
        ArmorData data = GameDictionary.ArmorDic[_id].MemberwiseClone() as ArmorData;
        data.UID = _uid;
        data.LV = _lv;
        data.Quality = _quality;
        data.SetRandomProperties();
        if (_equipSlot == 2)
            Player.Equip(data);
        return data;
    }
    public override void SetEquipStatus(bool _isEquiped, int _equipSlot)
    {
        base.SetEquipStatus(_isEquiped, _equipSlot);
        if (_isEquiped)
            EquipSlot = 2;
        else
            EquipSlot = 0;
    }
}
