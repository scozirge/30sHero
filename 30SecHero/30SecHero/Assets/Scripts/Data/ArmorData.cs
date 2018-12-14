using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class ArmorData : EquipData
{
    public override EquipType Type { get { return EquipType.Armor; } }
    public override int SellGold { get { return GameSettingData.GetArmorGold(LV, Quality); } }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, ArmorData> _dic, string _dataName)
    {
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", _dataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[_dataName];
        for (int i = 0; i < items.Count; i++)
        {
            ArmorData data = new ArmorData(items[i]);
            data.DataName = _dataName;
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
        PropertiesStr = GetPropertiesStr();
    }
    public ArmorData(JsonData _item)
        : base(_item)
    {
    }
    static int GetRandomID()
    {
        List<int> keys = new List<int>(GameDictionary.ArmorDic.Keys);
        int randIndex = UnityEngine.Random.Range(0, keys.Count);
        return keys[randIndex];
    }
    public override int SetUID()
    {
        base.SetUID();
        MaxUID++;
        UID = MaxUID;
        //Debug.Log("Type=" + Type + "  UID=" + UID);
        return UID;
    }
    protected override void SetRandomEnchant()
    {
        base.SetRandomEnchant();
        int enchantID = EnchantData.GetRandomEquipEnchant(EnchantType.Armor);
        MyEnchant = GameDictionary.EnchantDic[enchantID];
    }
    public static ArmorData GetRandomNewArmor(int _lv, int _quality)
    {
        ArmorData data = GameDictionary.ArmorDic[GetRandomID()].MemberwiseClone() as ArmorData;
        data.SetUID();
        data.LV = _lv;
        data.Quality = _quality;
        data.SetRandomProperties();
        if (_quality == 5)
            data.SetRandomEnchant();
        return data;
    }
    public static ArmorData GetNewArmor(int _uid, int _id, int _equipSlot, int _lv, int _quality, string _propertiesStr, int _enchantID)
    {
        ArmorData data = GameDictionary.ArmorDic[_id].MemberwiseClone() as ArmorData;
        data.PropertiesStr = _propertiesStr;
        data.SetPropertiesByStr();
        data.UID = _uid;
        EquipData.UpdateMaxUID(data.UID);
        data.LV = _lv;
        data.Quality = _quality;
        if (GameDictionary.EnchantDic.ContainsKey(_enchantID))
            data.MyEnchant = GameDictionary.EnchantDic[_enchantID];
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
