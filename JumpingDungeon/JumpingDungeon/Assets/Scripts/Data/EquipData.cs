using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public abstract class EquipData : Data
{
    public long UID;
    public virtual EquipType Type { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual string Description { get; protected set; }
    public string IconString;
    const string ImagePath = "Images/Main/{0}";
    public int Quality;
    public int LV;
    public string GetLVString()
    {
        return string.Format("{0}{1}", GameDictionary.String_UIDic["LV"].GetString(Player.UseLanguage), LV);
    }
    public bool IsEquiped;
    public virtual int BaseStrength { get; }
    public int Strength
    {
        get { return Mathf.RoundToInt(BaseStrength + RandomStrength); }
    }
    public virtual int BaseHealth { get; }
    public int Health
    {
        get { return Mathf.RoundToInt(BaseHealth + RandomHealth); }
    }
    public virtual int BaseShield { get; }
    public int Shield
    {
        get { return Mathf.RoundToInt(BaseShield + RandomShield); }
    }
    //Random Attributes
    public int RandomStrength;
    public int RandomHealth;
    public int RandomShield;
    public int RandomShieldRecovery;
    public int RandomMoveSpeed;
    public int RandomMaxMoveSpeed;
    public float RandomMaxMoveDecay;
    public float RandomAvatarTime;
    public float RandomAvatarDrop;
    public float RandomSkillTime;
    public float RandomSkillDrop;
    public float RandomEquipDrop;
    public int RandomGoldDrop;
    public float RandomBloodThirsty;
    public float RandomPotionEfficiency;

    protected EquipData(JsonData _item)
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
    protected void SetRandomProperties()
    {
        Dictionary<string, float> dic = GameSettingData.GetRandomEquipProperties(Quality, LV);
        if (dic.ContainsKey("RandomStrength"))
            RandomStrength = (int)dic["RandomStrength"];
        if (dic.ContainsKey("RandomHealth"))
            RandomHealth = (int)dic["RandomHealth"];
        if (dic.ContainsKey("RandomShield"))
            RandomShield = (int)dic["RandomShield"];
        if (dic.ContainsKey("RandomShieldRecovery"))
            RandomShieldRecovery = (int)dic["RandomShieldRecovery"];
        if (dic.ContainsKey("RandomMoveSpeed"))
            RandomMoveSpeed = (int)dic["RandomMoveSpeed"];
        if (dic.ContainsKey("RandomMaxMoveSpeed"))
            RandomMaxMoveSpeed = (int)dic["RandomMaxMoveSpeed"];
        if (dic.ContainsKey("RandomMaxMoveDecay"))
            RandomMaxMoveDecay = dic["RandomMaxMoveDecay"];
        if (dic.ContainsKey("RandomAvatarTime"))
            RandomAvatarTime = dic["RandomAvatarTime"];
        if (dic.ContainsKey("RandomAvatarDrop"))
            RandomAvatarDrop = dic["RandomAvatarDrop"];
        if (dic.ContainsKey("RandomSkillTime"))
            RandomSkillTime = dic["RandomSkillTime"];
        if (dic.ContainsKey("RandomSkillDrop"))
            RandomSkillDrop = dic["RandomSkillDrop"];
        if (dic.ContainsKey("RandomEquipDrop"))
            RandomEquipDrop = dic["RandomEquipDrop"];
        if (dic.ContainsKey("RandomGoldDrop"))
            RandomGoldDrop = (int)dic["RandomGoldDrop"];
        if (dic.ContainsKey("RandomBloodThirsty"))
            RandomBloodThirsty = dic["RandomBloodThirsty"];
        if (dic.ContainsKey("RandomPotionEfficiency"))
            RandomPotionEfficiency = dic["RandomPotionEfficiency"];
    }
}
