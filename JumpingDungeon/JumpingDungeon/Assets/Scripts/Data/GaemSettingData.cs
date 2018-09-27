using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class GameSettingData : Data
{
    public new static string ID;
    public static int WeaponValue;
    public static int WeaponLVUpValue;
    public static int WeaponGold;
    public static int WeaponLVUpGold;
    public static int ArmorValue;
    public static int ArmorLVUpValue;
    public static int ArmorGold;
    public static int ArmorLVUpGold;
    public static int AccessoryValue;
    public static int AccessoryLVUpValue;
    public static int AccessoryGold;
    public static int AccessoryLVUpGold;
    public static float GoldQuality1;
    public static float GoldQuality2;
    public static float GoldQuality3;
    public static float GoldQuality4;
    public static float GoldQuality5;
    public static Dictionary<int, float> GoldQualityDic = new Dictionary<int, float>();
    public static int RandomStrength;
    public static int RandomHealth;
    public static int RandomShield;
    public static int RandomShieldRecovery;
    public static int RandomMoveSpeed;
    public static int RandomMaxMoveSpeed;
    public static float RandomMoveDecay;
    public static float RandomAvatarTime;
    public static float RandomAvatarDrop;
    public static float RandomSkillTime;
    public static float RandomSkillDrop;
    public static float RandomEquipDrop;
    public static int RandomGoldDrop;
    public static float RandomBloodThirsty;
    public static float RandomPotionEfficiency;
    public static int MaxItemCount;
    public static string GrowingNumberColor;
    public static string DropingNumberColor;
    public static string NormalNumberColor;
    public static string StrengthenPath;
    public static string EquipPath;

    //裝備可隨機的屬性類型
    static List<RoleProperty> RandomPropertyList = new List<RoleProperty>() { RoleProperty.Strength, RoleProperty.Health, RoleProperty.Shield, RoleProperty.ShieldRecovery, RoleProperty.MoveSpeed, RoleProperty.MaxMoveSpeed, RoleProperty.MoveDecay, RoleProperty.AvatarTime, RoleProperty.AvatarDrop, RoleProperty.SkillTime, RoleProperty.SkillDrop, RoleProperty.EquipDrop, RoleProperty.GoldDrop, RoleProperty.BloodThirsty, RoleProperty.PotionEfficiency };


    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<string, GameSettingData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            GameSettingData data = new GameSettingData(items[i]);
            string id = items[i]["ID"].ToString();
            _dic.Add(id, data);
        }
    }

    GameSettingData(JsonData _item)
    {
        try
        {
            JsonData item = _item;
            foreach (string key in item.Keys)
            {
                switch (key)
                {
                    case "ID":
                        ID = item[key].ToString();
                        break;
                    case "Value":
                        switch (ID)
                        {
                            case "WeaponValue":
                                WeaponValue = int.Parse(item[key].ToString());
                                break;
                            case "WeaponLVUpValue":
                                WeaponLVUpValue = int.Parse(item[key].ToString());
                                break;
                            case "WeaponGold":
                                WeaponGold = int.Parse(item[key].ToString());
                                break;
                            case "WeaponLVUpGold":
                                WeaponLVUpGold = int.Parse(item[key].ToString());
                                break;
                            case "ArmorValue":
                                ArmorValue = int.Parse(item[key].ToString());
                                break;
                            case "ArmorLVUpValue":
                                ArmorLVUpValue = int.Parse(item[key].ToString());
                                break;
                            case "ArmorGold":
                                ArmorGold = int.Parse(item[key].ToString());
                                break;
                            case "ArmorLVUpGold":
                                ArmorLVUpGold = int.Parse(item[key].ToString());
                                break;
                            case "AccessoryValue":
                                AccessoryValue = int.Parse(item[key].ToString());
                                break;
                            case "AccessoryLVUpValue":
                                AccessoryLVUpValue = int.Parse(item[key].ToString());
                                break;
                            case "AccessoryGold":
                                AccessoryGold = int.Parse(item[key].ToString());
                                break;
                            case "AccessoryLVUpGold":
                                AccessoryLVUpGold = int.Parse(item[key].ToString());
                                break;
                            case "GoldQuality1":
                                GoldQuality1 = float.Parse(item[key].ToString());
                                GoldQualityDic.Add(1, GoldQuality1);
                                break;
                            case "GoldQuality2":
                                GoldQuality2 = float.Parse(item[key].ToString());
                                GoldQualityDic.Add(2, GoldQuality2);
                                break;
                            case "GoldQuality3":
                                GoldQuality3 = float.Parse(item[key].ToString());
                                GoldQualityDic.Add(3, GoldQuality3);
                                break;
                            case "GoldQuality4":
                                GoldQuality4 = float.Parse(item[key].ToString());
                                GoldQualityDic.Add(4, GoldQuality4);
                                break;
                            case "GoldQuality5":
                                GoldQuality5 = float.Parse(item[key].ToString());
                                GoldQualityDic.Add(5, GoldQuality5);
                                break;
                            case "RandomStrength":
                                RandomStrength = int.Parse(item[key].ToString());
                                break;
                            case "RandomHealth":
                                RandomHealth = int.Parse(item[key].ToString());
                                break;
                            case "RandomShield":
                                RandomShield = int.Parse(item[key].ToString());
                                break;
                            case "RandomShieldRecovery":
                                RandomShieldRecovery = int.Parse(item[key].ToString());
                                break;
                            case "RandomMoveSpeed":
                                RandomMoveSpeed = int.Parse(item[key].ToString());
                                break;
                            case "RandomMaxMoveSpeed":
                                RandomMaxMoveSpeed = int.Parse(item[key].ToString());
                                break;
                            case "RandomMoveDecay":
                                RandomMoveDecay = float.Parse(item[key].ToString());
                                break;
                            case "RandomAvatarTime":
                                RandomAvatarTime = float.Parse(item[key].ToString());
                                break;
                            case "RandomAvatarDrop":
                                RandomAvatarDrop = float.Parse(item[key].ToString());
                                break;
                            case "RandomSkillTime":
                                RandomSkillTime = float.Parse(item[key].ToString());
                                break;
                            case "RandomSkillDrop":
                                RandomSkillDrop = float.Parse(item[key].ToString());
                                break;
                            case "RandomEquipDrop":
                                RandomEquipDrop = float.Parse(item[key].ToString());
                                break;
                            case "RandomGoldDrop":
                                RandomGoldDrop = int.Parse(item[key].ToString());
                                break;
                            case "RandomBloodThirsty":
                                RandomBloodThirsty = float.Parse(item[key].ToString());
                                break;
                            case "RandomPotionEfficiency":
                                RandomPotionEfficiency = float.Parse(item[key].ToString());
                                break;
                            case "MaxItemCount":
                                MaxItemCount = int.Parse(item[key].ToString());
                                break;
                            case "GrowingNumberColor":
                                GrowingNumberColor = item[key].ToString();
                                break;
                            case "DropingNumberColor":
                                DropingNumberColor = item[key].ToString();
                                break;
                            case "NormalNumberColor":
                                NormalNumberColor = item[key].ToString();
                                break;
                            case "StrengthenPath":
                                StrengthenPath = item[key].ToString();
                                break;
                            case "EquipPath":
                                EquipPath = item[key].ToString();
                                break;
                            default:
                                Debug.LogWarning(string.Format("{0}表有不明屬性:{1}", DataName, key));
                                break;
                        }
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
    public static int GetWeaponStrength(int _lv)
    {
        return WeaponValue + _lv * WeaponLVUpValue;
    }
    public static int GetWeaponGold(int _lv, int _quality)
    {
        return Mathf.RoundToInt((WeaponGold + _lv * WeaponLVUpGold) * GoldQualityDic[_quality]);
    }
    public static int GetArmorHealth(int _lv)
    {
        return ArmorValue + _lv * ArmorLVUpValue;
    }
    public static int GetArmorGold(int _lv, int _quality)
    {
        return Mathf.RoundToInt((ArmorGold + _lv * ArmorLVUpGold) * GoldQualityDic[_quality]);
    }
    public static int GetAccessoryShield(int _lv)
    {
        return AccessoryValue + _lv * AccessoryLVUpValue;
    }
    public static int GetAccessoryGold(int _lv, int _quality)
    {
        return Mathf.RoundToInt((AccessoryGold + _lv * AccessoryLVUpGold) * GoldQualityDic[_quality]);
    }
    public static Dictionary<RoleProperty, float> GetNewRolePropertiesDic(float _initValue)
    {
        Dictionary<RoleProperty, float> dic = new Dictionary<RoleProperty, float>();
        for (int i = 0; i < Enum.GetValues(typeof(RoleProperty)).Length; i++)
        {
            dic.Add((RoleProperty)i, _initValue);
        }
        return dic;
    }
    public static void RolePropertyOperate(Dictionary<RoleProperty, float> _dic1, Dictionary<RoleProperty, float> _dic2, Operator _operator)
    {
        List<RoleProperty> keys = new List<RoleProperty>(_dic1.Keys);
        switch (_operator)
        {
            case Operator.Plus:
                for (int i = 0; i < keys.Count; i++)
                {
                    _dic1[keys[i]] += _dic2[keys[i]];
                }
                break;
            case Operator.Minus:
                for (int i = 0; i < keys.Count; i++)
                {
                    _dic1[keys[i]] -= _dic2[keys[i]];
                }
                break;
            default:
                Debug.LogWarning(string.Format("對兩個腳色屬性字典做運算時不應該使用{0}", _operator.ToString()));
                break;
        }
    }
    public static Dictionary<RoleProperty, float> GetRandomEquipProperties(int _quality, int _lv)
    {
        Dictionary<RoleProperty, float> dic = GetNewRolePropertiesDic(0);

        for (int i = 0; i < _quality; i++)
        {
            int rand = UnityEngine.Random.Range(0, RandomPropertyList.Count);
            switch (RandomPropertyList[rand])
            {
                case RoleProperty.Strength:
                    dic[(RoleProperty)rand] += RandomStrength * _lv;
                    break;
                case RoleProperty.Health:
                    dic[(RoleProperty)rand] += RandomHealth * _lv;
                    break;
                case RoleProperty.Shield:
                    dic[(RoleProperty)rand] += RandomShield * _lv;
                    break;
                case RoleProperty.ShieldRecovery:
                    dic[(RoleProperty)rand] += RandomShieldRecovery * _lv;
                    break;
                case RoleProperty.MoveSpeed:
                    dic[(RoleProperty)rand] += RandomMoveSpeed * _lv;
                    break;
                case RoleProperty.MaxMoveSpeed:
                    dic[(RoleProperty)rand] += RandomMaxMoveSpeed * _lv;
                    break;
                case RoleProperty.MoveDecay:
                    dic[(RoleProperty)rand] += RandomMoveDecay * _lv;
                    break;
                case RoleProperty.AvatarTime:
                    dic[(RoleProperty)rand] += RandomAvatarTime * _lv;
                    break;
                case RoleProperty.AvatarDrop:
                    dic[(RoleProperty)rand] += RandomAvatarDrop * _lv;
                    break;
                case RoleProperty.SkillTime:
                    dic[(RoleProperty)rand] += RandomSkillTime * _lv;
                    break;
                case RoleProperty.SkillDrop:
                    dic[(RoleProperty)rand] += RandomSkillDrop * _lv;
                    break;
                case RoleProperty.EquipDrop:
                    dic[(RoleProperty)rand] += RandomEquipDrop * _lv;
                    break;
                case RoleProperty.GoldDrop:
                    dic[(RoleProperty)rand] += RandomGoldDrop * _lv;
                    break;
                case RoleProperty.BloodThirsty:
                    dic[(RoleProperty)rand] += RandomBloodThirsty * _lv;
                    break;
                case RoleProperty.PotionEfficiency:
                    dic[(RoleProperty)rand] += RandomPotionEfficiency * _lv;
                    break;
                default:
                    Debug.LogWarning(string.Format("{0}:{1}不存在", rand, (RoleProperty)rand));
                    break;
            }
        }
        return dic;
    }
}
