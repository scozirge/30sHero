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
    public static float RandomMaxMoveDecay;
    public static float RandomAvatarTime;
    public static float RandomAvatarDrop;
    public static float RandomSkillTime;
    public static float RandomSkillDrop;
    public static float RandomEquipDrop;
    public static int RandomGoldDrop;
    public static float RandomBloodThirsty;
    public static float RandomPotionEfficiency;



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
                            case "RandomMaxMoveDecay":
                                RandomMaxMoveDecay = float.Parse(item[key].ToString());
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
    public static int GetWeaponAttack(int _lv)
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
    public static Dictionary<string, float> GetRandomEquipProperties(int _quality, int _lv)
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        for (int i = 0; i < _quality; i++)
        {
            int rand = UnityEngine.Random.Range(0, 15);
            switch (rand)
            {
                case 0:
                    if (dic.ContainsKey("RandomStrength"))
                        dic["RandomStrength"] += RandomStrength * _lv;
                    else
                        dic.Add("RandomStrength", RandomStrength * _lv);
                    break;
                case 1:
                    if (dic.ContainsKey("RandomHealth"))
                        dic["RandomHealth"] += RandomHealth * _lv;
                    else
                        dic.Add("RandomHealth", RandomHealth * _lv);
                    break;
                case 2:
                    if (dic.ContainsKey("RandomShield"))
                        dic["RandomShield"] += RandomShield * _lv;
                    else
                        dic.Add("RandomShield", RandomShield * _lv);
                    break;
                case 3:
                    if (dic.ContainsKey("RandomShieldRecovery"))
                        dic["RandomShieldRecovery"] += RandomShieldRecovery * _lv;
                    else
                        dic.Add("RandomShieldRecovery", RandomShieldRecovery * _lv);
                    break;
                case 4:
                    if (dic.ContainsKey("RandomMoveSpeed"))
                        dic["RandomMoveSpeed"] += RandomMoveSpeed * _lv;
                    else
                        dic.Add("RandomMoveSpeed", RandomMoveSpeed * _lv);
                    break;
                case 5:
                    if (dic.ContainsKey("RandomMaxMoveSpeed"))
                        dic["RandomMaxMoveSpeed"] += RandomMaxMoveSpeed * _lv;
                    else
                        dic.Add("RandomMaxMoveSpeed", RandomMaxMoveSpeed * _lv);
                    break;
                case 6:
                    if (dic.ContainsKey("RandomMaxMoveDecay"))
                        dic["RandomMaxMoveDecay"] += RandomMaxMoveDecay * _lv;
                    else
                        dic.Add("RandomMaxMoveDecay", RandomMaxMoveDecay * _lv);
                    break;
                case 7:
                    if (dic.ContainsKey("RandomAvatarTime"))
                        dic["RandomAvatarTime"] += RandomAvatarTime * _lv;
                    else
                        dic.Add("RandomAvatarTime", RandomAvatarTime * _lv);
                    break;
                case 8:
                    if (dic.ContainsKey("RandomAvatarDrop"))
                        dic["RandomAvatarDrop"] += RandomAvatarDrop * _lv;
                    else
                        dic.Add("RandomAvatarDrop", RandomAvatarDrop * _lv);
                    break;
                case 9:
                    if (dic.ContainsKey("RandomSkillTime"))
                        dic["RandomSkillTime"] += RandomSkillTime * _lv;
                    else
                        dic.Add("RandomSkillTime", RandomSkillTime * _lv);
                    break;
                case 10:
                    if (dic.ContainsKey("RandomSkillDrop"))
                        dic["RandomSkillDrop"] += RandomSkillDrop * _lv;
                    else
                        dic.Add("RandomSkillDrop", RandomSkillDrop * _lv);
                    break;
                case 11:
                    if (dic.ContainsKey("RandomEquipDrop"))
                        dic["RandomEquipDrop"] += RandomEquipDrop * _lv;
                    else
                        dic.Add("RandomEquipDrop", RandomEquipDrop * _lv);
                    break;
                case 12:
                    if (dic.ContainsKey("RandomGoldDrop"))
                        dic["RandomGoldDrop"] += RandomGoldDrop * _lv;
                    else
                        dic.Add("RandomGoldDrop", RandomGoldDrop * _lv);
                    break;
                case 13:
                    if (dic.ContainsKey("RandomBloodThirsty"))
                        dic["RandomBloodThirsty"] += RandomBloodThirsty * _lv;
                    else
                        dic.Add("RandomBloodThirsty", RandomBloodThirsty * _lv);
                    break;
                case 14:
                    if (dic.ContainsKey("RandomPotionEfficiency"))
                        dic["RandomPotionEfficiency"] += RandomPotionEfficiency * _lv;
                    else
                        dic.Add("RandomPotionEfficiency", RandomPotionEfficiency * _lv);
                    break;
            }
        }
        return dic;
    }
}
