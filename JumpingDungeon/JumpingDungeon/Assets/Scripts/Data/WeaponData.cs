using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class WeaponData : EquipData
{
    public override EquipType Type { get { return EquipType.Weapon; } }
    static long MaxUID;
    public override int BaseStrength
    {
        get
        {
            return GameSettingData.GetWeaponAttack(LV);
        }
    }
    public override int SellGold { get { return GameSettingData.GetWeaponGold(LV, Quality); } }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, WeaponData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            WeaponData data = new WeaponData(items[i]);
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
    public int Attack
    {
        get
        {
            return GameSettingData.GetWeaponAttack(LV);
        }
    }
    public int Gold
    {
        get
        {
            return GameSettingData.GetWeaponGold(LV, Quality);
        }
    }
    public WeaponData(JsonData _item)
        : base(_item)
    {
    }
    public static WeaponData GetNewWeapon(int _id, int _lv, int _quality)
    {
        WeaponData data = GameDictionary.WeaponDic[_id].MemberwiseClone() as WeaponData;
        MaxUID++;
        data.UID = MaxUID;
        data.LV = _lv;
        data.Quality = _quality;
        data.SetRandomProperties();
        return data;
    }

}
