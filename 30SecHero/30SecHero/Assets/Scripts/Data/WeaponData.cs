using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class WeaponData : EquipData
{
    public override EquipType Type { get { return EquipType.Weapon; } }
    public override int SellGold { get { return GameSettingData.GetWeaponGold(LV, Quality); } }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, WeaponData> _dic, string _dataName)
    {
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", _dataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[_dataName];
        for (int i = 0; i < items.Count; i++)
        {
            WeaponData data = new WeaponData(items[i]);
            data.DataName = _dataName;
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }
    }
    public override string Name
    {
        get
        {
            if (!GameDictionary.String_WeaponDic.ContainsKey(ID.ToString()))
            {
                Debug.LogWarning(string.Format("{0}表不包含{1}的文字資料", DataName, ID));
                return "NullText";
            }
            return GameDictionary.String_WeaponDic[ID.ToString()].GetString(Player.UseLanguage);
        }
        protected set { return; }
    }
    public int Gold
    {
        get
        {
            return GameSettingData.GetWeaponGold(LV, Quality);
        }
    }
    protected override void SetRandomProperties()
    {
        base.SetRandomProperties();
        Properties[RoleProperty.Strength] += GameSettingData.GetWeaponStrength(LV);
        PropertiesStr = GetPropertiesStr();
    }
    public WeaponData(JsonData _item)
        : base(_item)
    {
    }
    static int GetRandomID()
    {
        List<int> keys = new List<int>(GameDictionary.WeaponDic.Keys);
        int randIndex = UnityEngine.Random.Range(0, keys.Count);
        return keys[randIndex];
    }
    public override long SetUID()
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
        int enchantID = EnchantData.GetRandomEquipEnchant(EnchantType.Weapon);
        MyEnchant = GameDictionary.EnchantDic[enchantID];
    }
    public static WeaponData GetRandomNewWeapon(int _lv, int _quality)
    {
        WeaponData data = GameDictionary.WeaponDic[GetRandomID()].MemberwiseClone() as WeaponData;
        data.SetUID();
        data.LV = _lv;
        data.Quality = _quality;
        data.SetRandomProperties();
        if (_quality == 5)
            data.SetRandomEnchant();
        return data;
    }
    public static WeaponData GetNewWeapon(int _uid, int _id, int _equipSlot, int _lv, int _quality, string _propertiesStr, int _enchantID)
    {
        WeaponData data = GameDictionary.WeaponDic[_id].MemberwiseClone() as WeaponData;
        data.PropertiesStr = _propertiesStr;
        data.SetPropertiesByStr();
        data.UID = _uid;
        EquipData.UpdateMaxUID(data.UID);
        data.LV = _lv;
        data.Quality = _quality;
        if (GameDictionary.EnchantDic.ContainsKey(_enchantID))
            data.MyEnchant = GameDictionary.EnchantDic[_enchantID];
        if (_equipSlot == 1)
            Player.Equip(data);
        return data;
    }
    public override void SetEquipStatus(bool _isEquiped, int _equipSlot)
    {
        base.SetEquipStatus(_isEquiped, _equipSlot);
        if (_isEquiped)
            EquipSlot = 1;
        else
            EquipSlot = 0;
    }
    //取得預設武器(沒穿裝備時預設顯示紙娃娃武器)
    public static WeaponData GetDefaultWeapon()
    {
        WeaponData data = GameDictionary.WeaponDic[1].MemberwiseClone() as WeaponData;
        return data;
    }
}
