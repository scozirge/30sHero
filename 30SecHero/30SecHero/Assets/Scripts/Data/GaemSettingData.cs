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
    public static float RandomShieldRecovery;
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
    public static string WeaponPath;
    public static string ArmorPath;
    public static string AccessoryPath;
    public static float FreezeMove;
    public static float BurnDamage;
    public static float CurseDamageReduce;
    public static float BurnInterval;
    //腳色基本數值
    public static int MaxHealth;
    public static int BaseDamage;
    public static int BaseMoveSpeed;
    public static int MaxShield;
    public static float ShieldGenerateProportion;
    public static float ShieldRechargeTime;
    public static int GainMoveFromKilling;
    public static float MoveDepletedTime;
    public static int MaxExtraMove;
    public static float MaxAvaterTime;
    public static float EnergyDrop;
    public static float AvatarTimeBuff;
    public static float SkillTimeBuff;
    public static float SkillDrop;
    public static float EquipDrop;
    public static float GoldDrop;
    public static float BloodThirsty;
    public static float PotionEfficiency;
    public static float PotionDrop;
    //關卡數值
    public static float PotionInterval;
    public static float PotionProportion;
    public static float EnemyFirstHalfInterval;
    public static float EnemySecondHalfInterval;
    public static int EnemyFirstHalfMinCount;
    public static int EnemyFirstHalfMaxCount;
    public static int EnemySecondHalfMinCount;
    public static int EnemySecondHalfMaxCount;
    public static float EnemySpawnInterval;
    public static int FloorPassGold;
    public static int NewFloorPassGold;
    public static float BossEmeraldProportion;
    public static int BossEmerald;
    public static int NewBossEmerald;
    public static int EnemyGold;
    public static int NoEquipWeight;
    public static int EquipQuality1Weight;
    public static int EquipQuality2Weight;
    public static int EquipQuality3Weight;
    public static int EquipQuality4Weight;
    public static int EquipQuality5Weight;
    public static float EnemyDropPotionProportion;
    public static float EnemyDropGoldProportion;
    public static int EnemyDropGold;
    public static float EnemyDropGoldOffset;
    public static int MaxEnemy;
    public static int MaxLoot;
    public static int FloorPlate;
    public static int BossDebutPlate;





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
                                RandomShieldRecovery = float.Parse(item[key].ToString());
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
                                WeaponPath = item[key].ToString();
                                break;
                            case "ArmorPath":
                                ArmorPath = item[key].ToString();
                                break;
                            case "AccessoryPath":
                                AccessoryPath = item[key].ToString();
                                break;
                            case "FreezeMove":
                                FreezeMove = float.Parse(item[key].ToString());
                                break;
                            case "BurnDamage":
                                BurnDamage = float.Parse(item[key].ToString());
                                break;
                            case "BurnInterval":
                                BurnInterval = float.Parse(item[key].ToString());
                                break;
                            case "CurseDamageReduce":
                                CurseDamageReduce = float.Parse(item[key].ToString());
                                break;
                            //腳色基本數值
                            case "MaxHealth":
                                MaxHealth = int.Parse(item[key].ToString());
                                break;
                            case "BaseDamage":
                                BaseDamage = int.Parse(item[key].ToString());
                                break;
                            case "BaseMoveSpeed":
                                BaseMoveSpeed = int.Parse(item[key].ToString());
                                break;
                            case "MaxShield":
                                MaxShield = int.Parse(item[key].ToString());
                                break;
                            case "ShieldGenerateProportion":
                                ShieldGenerateProportion = float.Parse(item[key].ToString());
                                break;
                            case "ShieldRechargeTime":
                                ShieldRechargeTime = float.Parse(item[key].ToString());
                                break;
                            case "GainMoveFromKilling":
                                GainMoveFromKilling = int.Parse(item[key].ToString());
                                break;
                            case "MoveDepletedTime":
                                MoveDepletedTime = float.Parse(item[key].ToString());
                                break;
                            case "MaxExtraMove":
                                MaxExtraMove = int.Parse(item[key].ToString());
                                break;
                            case "MaxAvaterTime":
                                MaxAvaterTime = float.Parse(item[key].ToString());
                                break;
                            case "EnergyDrop":
                                EnergyDrop = float.Parse(item[key].ToString());
                                break;
                            case "AvatarTimeBuff":
                                AvatarTimeBuff = float.Parse(item[key].ToString());
                                break;
                            case "SkillTimeBuff":
                                SkillTimeBuff = float.Parse(item[key].ToString());
                                break;
                            case "SkillDrop":
                                SkillDrop = float.Parse(item[key].ToString());
                                break;
                            case "EquipDrop":
                                EquipDrop = float.Parse(item[key].ToString());
                                break;
                            case "GoldDrop":
                                GoldDrop = float.Parse(item[key].ToString());
                                break;
                            case "BloodThirsty":
                                BloodThirsty = float.Parse(item[key].ToString());
                                break;
                            case "PotionEfficiency":
                                PotionEfficiency = float.Parse(item[key].ToString());
                                break;
                            case "PotionDrop":
                                PotionDrop = float.Parse(item[key].ToString());
                                break;
                            //關卡數值
                            case "PotionInterval":
                                PotionInterval = float.Parse(item[key].ToString());
                                break;
                            case "PotionProportion":
                                PotionProportion = float.Parse(item[key].ToString());
                                break;
                            case "EnemyFirstHalfInterval":
                                EnemyFirstHalfInterval = float.Parse(item[key].ToString());
                                break;
                            case "EnemySecondHalfInterval":
                                EnemySecondHalfInterval = float.Parse(item[key].ToString());
                                break;
                            case "EnemyFirstHalfMinCount":
                                EnemyFirstHalfMinCount = int.Parse(item[key].ToString());
                                break;
                            case "EnemyFirstHalfMaxCount":
                                EnemyFirstHalfMaxCount = int.Parse(item[key].ToString());
                                break;
                            case "EnemySecondHalfMinCount":
                                EnemySecondHalfMinCount = int.Parse(item[key].ToString());
                                break;
                            case "EnemySecondHalfMaxCount":
                                EnemySecondHalfMaxCount = int.Parse(item[key].ToString());
                                break;
                            case "EnemySpawnInterval":
                                EnemySpawnInterval = float.Parse(item[key].ToString());
                                break;
                            case "FloorPassGold":
                                FloorPassGold = int.Parse(item[key].ToString());
                                break;
                            case "NewFloorPassGold":
                                NewFloorPassGold = int.Parse(item[key].ToString());
                                break;
                            case "BossEmeraldProportion":
                                BossEmeraldProportion = float.Parse(item[key].ToString());
                                break;
                            case "BossEmerald":
                                BossEmerald = int.Parse(item[key].ToString());
                                break;
                            case "NewBossEmerald":
                                NewBossEmerald = int.Parse(item[key].ToString());
                                break;
                            case "EnemyGold":
                                EnemyGold = int.Parse(item[key].ToString());
                                break;
                            case "NoEquipWeight":
                                NoEquipWeight = int.Parse(item[key].ToString());
                                break;
                            case "EquipQuality1Weight":
                                EquipQuality1Weight = int.Parse(item[key].ToString());
                                break;
                            case "EquipQuality2Weight":
                                EquipQuality2Weight = int.Parse(item[key].ToString());
                                break;
                            case "EquipQuality3Weight":
                                EquipQuality3Weight = int.Parse(item[key].ToString());
                                break;
                            case "EquipQuality4Weight":
                                EquipQuality4Weight = int.Parse(item[key].ToString());
                                break;
                            case "EquipQuality5Weight":
                                EquipQuality5Weight = int.Parse(item[key].ToString());
                                break;
                            case "EnemyDropPotionProportion":
                                EnemyDropPotionProportion = float.Parse(item[key].ToString());
                                break;
                            case "EnemyDropGoldProportion":
                                EnemyDropGoldProportion = float.Parse(item[key].ToString());
                                break;
                            case "EnemyDropGold":
                                EnemyDropGold = int.Parse(item[key].ToString());
                                break;
                            case "EnemyDropGoldOffset":
                                EnemyDropGoldOffset = float.Parse(item[key].ToString());
                                break;
                            case "MaxEnemy":
                                MaxEnemy = int.Parse(item[key].ToString());
                                break;
                            case "MaxLoot":
                                MaxLoot = int.Parse(item[key].ToString());
                                break;
                            case "FloorPlate":
                                FloorPlate = int.Parse(item[key].ToString());
                                break;
                            case "BossDebutPlate":
                                BossDebutPlate = int.Parse(item[key].ToString());
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
    public static string GetEquipIconPath(EquipType _type)
    {
        switch (_type)
        {
            case EquipType.Weapon:
                return WeaponPath;
            case EquipType.Armor:
                return ArmorPath;
            case EquipType.Accessory:
                return AccessoryPath;
            default:
                return "";
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
    public static int GetEnemyDropGold(int _floor)
    {
        float randOffset = UnityEngine.Random.Range(-EnemyDropGoldOffset, EnemyDropGoldOffset);
        int gold = (int)((_floor * EnemyDropGold) * (1 - randOffset));
        if (gold < 0)
            gold = 0;
        return gold;
    }
}
