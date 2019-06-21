using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    //Strengthen Dic
    public static Dictionary<int, StrengthenData> StrengthenDic = new Dictionary<int, StrengthenData>();
    //Enchant Dic
    public static Dictionary<int, EnchantData> EnchantDic = new Dictionary<int, EnchantData>();
    public static int GetPartnerEnchantCount()
    {
        int count = 0;
        List<int> keys = new List<int>(EnchantDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (EnchantDic[keys[i]].MyEnchantType == EnchantType.Enchant && EnchantDic[keys[i]].LV > 0)
                count++;
        }
        return count;
    }
    //Properties
    public static Dictionary<RoleProperty, float> Properties = new Dictionary<RoleProperty, float>();
    static Dictionary<RoleProperty, float> EquipPlus = new Dictionary<RoleProperty, float>();
    //static Dictionary<RoleProperty, float> EquipMultiple = new Dictionary<RoleProperty, float>();
    static Dictionary<RoleProperty, float> StrengthenPlus = new Dictionary<RoleProperty, float>();
    //static Dictionary<RoleProperty, float> StrengthenMultiple = new Dictionary<RoleProperty, float>();
    static Dictionary<EnchantProperty, float> EnchantPlus = new Dictionary<EnchantProperty, float>();

    static void InitProperty()
    {
        Properties = GameSettingData.GetNewRolePropertiesDic(0);
        EquipPlus = GameSettingData.GetNewRolePropertiesDic(0);
        //EquipMultiple = GameSettingData.GetNewRolePropertiesDic(0);
        StrengthenPlus = GameSettingData.GetNewRolePropertiesDic(0);
        //StrengthenMultiple = GameSettingData.GetNewRolePropertiesDic(0);
        EnchantPlus = GameSettingData.GetNewEnchantPropertiesDic(0);
        StrengthenDic = StrengthenData.GetNewStrengthenDic(0);
        EnchantDic = EnchantData.GetNewEnchantDic(0);
        Player.Properties[RoleProperty.Health] = GameSettingData.MaxHealth;
        Player.Properties[RoleProperty.Strength] = GameSettingData.BaseDamage;
        Player.Properties[RoleProperty.MoveSpeed] = GameSettingData.BaseMoveSpeed;
        Player.Properties[RoleProperty.Shield] = GameSettingData.MaxShield;
        Player.Properties[RoleProperty.ShieldRecovery] = GameSettingData.ShieldGenerateProportion;
        Player.Properties[RoleProperty.ShieldReChargeTime] = GameSettingData.ShieldRechargeTime;
        Player.Properties[RoleProperty.GainMoveFromKilling] = GameSettingData.GainMoveFromKilling;
        Player.Properties[RoleProperty.MoveDecay] = GameSettingData.MoveDepletedTime;
        Player.Properties[RoleProperty.MaxMoveSpeed] = GameSettingData.MaxExtraMove;
        Player.Properties[RoleProperty.AvatarTime] = GameSettingData.MaxAvaterTime;
        Player.Properties[RoleProperty.AvatarPotionBuff] = GameSettingData.AvatarPotionBuff;
        Player.Properties[RoleProperty.SkillTimeBuff] = GameSettingData.SkillTimeBuff;
        Player.Properties[RoleProperty.SkillDrop] = GameSettingData.SkillDrop;
        Player.Properties[RoleProperty.EquipDrop] = GameSettingData.EquipDrop;
        Player.Properties[RoleProperty.GoldDrop] = GameSettingData.GoldDrop;
        Player.Properties[RoleProperty.BloodThirsty] = GameSettingData.BloodThirsty;
        Player.Properties[RoleProperty.PotionEfficiency] = GameSettingData.PotionEfficiency;
        Player.Properties[RoleProperty.PotionDrop] = GameSettingData.PotionDrop;
        Player.Properties[RoleProperty.RushCD] = GameSettingData.RushCD;
        Player.Properties[RoleProperty.WeaponDamage] = GameSettingData.WeaponDamage;
    }
    public static float GetProperties(RoleProperty _property)
    {
        return
            Properties[_property]
            +
            (EquipPlus[_property] + StrengthenPlus[_property]);
    }
    public static float GetEnchantProperty(EnchantProperty _type)
    {
        if (EnchantPlus.ContainsKey(_type))
        {
            return EnchantPlus[_type];
        }
        return 0;
    }
    public static void ShowBaseProperties()
    {
        Debug.Log("/////ShowBaseProperties/////");
        List<RoleProperty> keys = new List<RoleProperty>(Properties.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i] + "=" + Properties[keys[i]]);
        }
    }
    public static void ShowEquipProperties()
    {
        Debug.Log("/////ShowEquipProperties/////");
        List<RoleProperty> keys = new List<RoleProperty>(EquipPlus.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i] + "_Plus=" + EquipPlus[keys[i]]);
        }

        Debug.Log("附魔資訊");
        bool showZeroData = false;
        List<EnchantProperty> keys2 = new List<EnchantProperty>(EnchantPlus.Keys);
        for (int i = 0; i < keys2.Count; i++)
        {
            if (!showZeroData && EnchantPlus[keys2[i]] != 0)
                Debug.Log(keys2[i] + "=" + EnchantPlus[keys2[i]]);
        }

        /*
        List<RoleProperty> keys2 = new List<RoleProperty>(EquipMultiple.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i] + "_Multiple=" + EquipMultiple[keys[i]]);
        }
        */
    }
    public static void ShowStrengthenProperties()
    {
        Debug.Log("/////ShowStrengthenProperties/////");
        List<RoleProperty> keys = new List<RoleProperty>(StrengthenPlus.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i] + "_Plus=" + StrengthenPlus[keys[i]]);
        }
        /*
        List<RoleProperty> keys2 = new List<RoleProperty>(StrengthenMultiple.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i] + "_Multiple=" + StrengthenMultiple[keys[i]]);
        }
        */
    }
    public static void ShowTotalProperties()
    {
        Debug.Log("/////ShowTotalProperties/////");
        List<RoleProperty> keys = new List<RoleProperty>(Properties.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i] + "=" + GetProperties(keys[i]));
        }

    }
}
