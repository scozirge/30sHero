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
    public static float RandomStrength;
    public static float RandomHealth;
    public static float RandomShield;
    public static float RandomShieldRecharge;
    public static float RandomShieldRecovery;
    public static int RandomMoveSpeed;
    public static int RandomMaxMoveSpeed;
    public static float RandomMoveDecay;
    public static float RandomAvatarTime;
    public static float RandomAvatarPotionBuff;
    public static float RandomSkillTime;
    public static float RandomSkillDrop;
    public static float RandomEquipDrop;
    public static float RandomGoldDrop;
    public static float RandomBloodThirsty;
    public static float RandomPotionEfficiency;
    public static int MaxItemCount;
    public static string GrowingNumberColor;
    public static string DropingNumberColor;
    public static string NormalNumberColor;
    public static string EnchantTextColor;
    public static string StrengthenPath;
    public static string WeaponPath;
    public static string ArmorPath;
    public static string AccessoryPath;
    public static string SoulPath;
    public static string EnchantPath;
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
    public static float AvatarPotionBuff;
    public static float SkillTimeBuff;
    public static float SkillDrop;
    public static float EquipDrop;
    public static int GoldDrop;
    public static float BloodThirsty;
    public static float PotionEfficiency;
    public static float PotionDrop;
    public static float RushCD;
    public static float SkillAmmoInterval;
    public static float SkillFaceTargetAmmoInterval;
    public static float SkillAmmoSpeed;
    public static float SkillAmmoDamage;

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
    public static int DropWeaponWeight;
    public static int DropArmorWeight;
    public static int DropAccessoryWeight;
    public static float EnemyDropPotionProportion;
    public static float EnemyDropGoldProportion;
    public static int EnemyDropGold;
    public static float EnemyDropGoldOffset;
    public static int MaxEnemy;
    public static int MaxForeEnemy;
    public static int MaxBackEnemy;
    public static int MaxLoot;
    public static int FloorPlate;
    public static int FloorUpPlate;
    public static int MaxFloorPlate;
    public static int BossDebutPlate;
    public static float EnemyHPGrow;
    public static float EnemyDMGGrow;
    public static float BossHPGrow;
    public static float BossDMGGrow;




    //裝備可隨機的屬性類型(不可變動順序，要追加要往後加，不然資料庫的資料會對錯)
    public static List<RoleProperty> RandomPropertyList = new List<RoleProperty>() { RoleProperty.Strength, RoleProperty.Health, RoleProperty.Shield, RoleProperty.ShieldRecovery, RoleProperty.MoveSpeed, RoleProperty.MaxMoveSpeed, RoleProperty.MoveDecay, RoleProperty.AvatarTime, RoleProperty.AvatarPotionBuff, RoleProperty.SkillTimeBuff, RoleProperty.SkillDrop, RoleProperty.EquipDrop, RoleProperty.GoldDrop, RoleProperty.BloodThirsty, RoleProperty.PotionEfficiency, RoleProperty.ShieldReChargeTime };
    //裝備品階權重
    static List<int> EquipQualityWeightList = new List<int>();
    static List<int> DropEquipWeightList = new List<int>();

    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<string, GameSettingData> _dic, string _dataName)
    {
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", _dataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[_dataName];
        for (int i = 0; i < items.Count; i++)
        {
            GameSettingData data = new GameSettingData(items[i]);
            data.DataName = _dataName;
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
                                RandomStrength = float.Parse(item[key].ToString());
                                break;
                            case "RandomHealth":
                                RandomHealth = float.Parse(item[key].ToString());
                                break;
                            case "RandomShield":
                                RandomShield = float.Parse(item[key].ToString());
                                break;
                            case "RandomShieldRecharge":
                                RandomShieldRecharge = float.Parse(item[key].ToString());
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
                            case "RandomAvatarPotionBuff":
                                RandomAvatarPotionBuff = float.Parse(item[key].ToString());
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
                                RandomGoldDrop = float.Parse(item[key].ToString());
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
                            case "EnchantTextColor":
                                EnchantTextColor = item[key].ToString();
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
                            case "SoulPath":
                                SoulPath = item[key].ToString();
                                break;
                            case "EnchantPath":
                                EnchantPath = item[key].ToString();
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
                            case "AvatarPotionBuff":
                                AvatarPotionBuff = float.Parse(item[key].ToString());
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
                                GoldDrop = int.Parse(item[key].ToString());
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
                            case "RushCD":
                                RushCD = float.Parse(item[key].ToString());
                                break;
                            case "SkillAmmoInterval":
                                SkillAmmoInterval = float.Parse(item[key].ToString());
                                break;
                            case "SkillFaceTargetAmmoInterval":
                                SkillFaceTargetAmmoInterval = float.Parse(item[key].ToString());
                                break;
                            case "SkillAmmoSpeed":
                                SkillAmmoSpeed = float.Parse(item[key].ToString());
                                break;
                            case "SkillAmmoDamage":
                                SkillAmmoDamage = float.Parse(item[key].ToString());
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
                                EquipQualityWeightList.Add(NoEquipWeight);
                                break;
                            case "EquipQuality1Weight":
                                EquipQuality1Weight = int.Parse(item[key].ToString());
                                EquipQualityWeightList.Add(EquipQuality1Weight);
                                break;
                            case "EquipQuality2Weight":
                                EquipQuality2Weight = int.Parse(item[key].ToString());
                                EquipQualityWeightList.Add(EquipQuality2Weight);
                                break;
                            case "EquipQuality3Weight":
                                EquipQuality3Weight = int.Parse(item[key].ToString());
                                EquipQualityWeightList.Add(EquipQuality3Weight);
                                break;
                            case "EquipQuality4Weight":
                                EquipQuality4Weight = int.Parse(item[key].ToString());
                                EquipQualityWeightList.Add(EquipQuality4Weight);
                                break;
                            case "EquipQuality5Weight":
                                EquipQuality5Weight = int.Parse(item[key].ToString());
                                EquipQualityWeightList.Add(EquipQuality5Weight);
                                break;
                            case "DropWeaponWeight":
                                DropWeaponWeight = int.Parse(item[key].ToString());
                                DropEquipWeightList.Add(DropWeaponWeight);
                                break;
                            case "DropArmorWeight":
                                DropArmorWeight = int.Parse(item[key].ToString());
                                DropEquipWeightList.Add(DropArmorWeight);
                                break;
                            case "DropAccessoryWeight":
                                DropAccessoryWeight = int.Parse(item[key].ToString());
                                DropEquipWeightList.Add(DropAccessoryWeight);
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
                            case "MaxForeEnemy":
                                MaxForeEnemy = int.Parse(item[key].ToString());
                                break;
                            case "MaxBackEnemy":
                                MaxBackEnemy = int.Parse(item[key].ToString());
                                break;
                            case "MaxLoot":
                                MaxLoot = int.Parse(item[key].ToString());
                                break;
                            case "FloorPlate":
                                FloorPlate = int.Parse(item[key].ToString());
                                break;
                            case "FloorUpPlate":
                                FloorUpPlate = int.Parse(item[key].ToString());
                                break;
                            case "MaxFloorPlate":
                                MaxFloorPlate = int.Parse(item[key].ToString());
                                break;
                            case "BossDebutPlate":
                                BossDebutPlate = int.Parse(item[key].ToString());
                                break;
                            case "EnemyHPGrow":
                                EnemyHPGrow = float.Parse(item[key].ToString());
                                break;
                            case "EnemyDMGGrow":
                                EnemyDMGGrow = float.Parse(item[key].ToString());
                                break;
                            case "BossHPGrow":
                                BossHPGrow = float.Parse(item[key].ToString());
                                break;
                            case "BossDMGGrow":
                                BossDMGGrow = float.Parse(item[key].ToString());
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
    public static Dictionary<EnchantProperty, float> GetNewEnchantPropertiesDic(float _initValue)
    {
        Dictionary<EnchantProperty, float> dic = new Dictionary<EnchantProperty, float>();
        for (int i = 0; i < Enum.GetValues(typeof(EnchantProperty)).Length; i++)
        {
            dic.Add((EnchantProperty)i, _initValue);
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
                    if (_dic2.ContainsKey(keys[i]))
                        _dic1[keys[i]] += _dic2[keys[i]];
                }
                break;
            case Operator.Minus:
                for (int i = 0; i < keys.Count; i++)
                {
                    if (_dic2.ContainsKey(keys[i]))
                        _dic1[keys[i]] -= _dic2[keys[i]];
                }
                break;
            default:
                Debug.LogWarning(string.Format("對兩個腳色屬性字典做運算時不應該使用{0}", _operator.ToString()));
                break;
        }
    }
    public static void EnchantPropertyOperate(Dictionary<EnchantProperty, float> _dic1, Dictionary<EnchantProperty, float> _dic2, Operator _operator)
    {
        List<EnchantProperty> keys = new List<EnchantProperty>(_dic1.Keys);
        switch (_operator)
        {
            case Operator.Plus:
                for (int i = 0; i < keys.Count; i++)
                {
                    if (_dic2.ContainsKey(keys[i]))
                        _dic1[keys[i]] += _dic2[keys[i]];
                }
                break;
            case Operator.Minus:
                for (int i = 0; i < keys.Count; i++)
                {
                    if (_dic2.ContainsKey(keys[i]))
                        _dic1[keys[i]] -= _dic2[keys[i]];
                }
                break;
            default:
                Debug.LogWarning(string.Format("對兩個腳色附魔屬性字典做運算時不應該使用{0}", _operator.ToString()));
                break;
        }
    }
    public static Dictionary<RoleProperty, float> GetRandomEquipProperties(int _quality, int _lv)
    {
        Dictionary<RoleProperty, float> dic = GetNewRolePropertiesDic(0);
        int qualityPropertyCount = _quality;
        if (qualityPropertyCount < 0)
            qualityPropertyCount = 0;
        //_quality = RandomPropertyList.Count;
        for (int i = 0; i < qualityPropertyCount; i++)
        {
            int rand = UnityEngine.Random.Range(0, RandomPropertyList.Count);
            //rand = i;
            switch (RandomPropertyList[rand])
            {
                case RoleProperty.Strength:
                    dic[RandomPropertyList[rand]] += (int)(RandomStrength * _lv);
                    break;
                case RoleProperty.Health:
                    dic[RandomPropertyList[rand]] += (int)(RandomHealth * _lv);
                    break;
                case RoleProperty.Shield:
                    dic[RandomPropertyList[rand]] += (int)(RandomShield * _lv);
                    break;
                case RoleProperty.ShieldReChargeTime:
                    dic[RandomPropertyList[rand]] += RandomShieldRecharge;
                    break;
                case RoleProperty.ShieldRecovery:
                    dic[RandomPropertyList[rand]] += RandomShieldRecovery;
                    break;
                case RoleProperty.MoveSpeed:
                    dic[RandomPropertyList[rand]] += RandomMoveSpeed;
                    break;
                case RoleProperty.MaxMoveSpeed:
                    dic[RandomPropertyList[rand]] += RandomMaxMoveSpeed;
                    break;
                case RoleProperty.MoveDecay:
                    dic[RandomPropertyList[rand]] += RandomMoveDecay;
                    break;
                case RoleProperty.AvatarTime:
                    dic[RandomPropertyList[rand]] += RandomAvatarTime;
                    break;
                case RoleProperty.AvatarPotionBuff:
                    dic[RandomPropertyList[rand]] += RandomAvatarPotionBuff;
                    break;
                case RoleProperty.SkillTimeBuff:
                    dic[RandomPropertyList[rand]] += RandomSkillTime;
                    break;
                case RoleProperty.SkillDrop:
                    dic[RandomPropertyList[rand]] += RandomSkillDrop;
                    break;
                case RoleProperty.EquipDrop:
                    dic[RandomPropertyList[rand]] += RandomEquipDrop;
                    break;
                case RoleProperty.GoldDrop:
                    dic[RandomPropertyList[rand]] += RandomGoldDrop * _lv;
                    break;
                case RoleProperty.BloodThirsty:
                    dic[RandomPropertyList[rand]] += RandomBloodThirsty;
                    break;
                case RoleProperty.PotionEfficiency:
                    dic[RandomPropertyList[rand]] += RandomPotionEfficiency;
                    break;
                default:
                    Debug.LogWarning(string.Format("{0}:{1}不存在", rand, RandomPropertyList[rand]));
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
    /// <summary>
    /// 返回0~5 0代表無掉落 5代表品階最高品階
    /// </summary>
    /// <returns></returns>
    public static int GetRandomEquipQuality(int _extraWeight)
    {
        int quality = 0;
        List<int> weightList = new List<int>(EquipQualityWeightList);
        weightList[0] -= _extraWeight;
        if (weightList[0] < 0)
            weightList[0] = 0;
        quality = ProbabilityGetter.GetFromWeigth(weightList);
        return quality;
    }
    /// <summary>
    /// 返回1~5 BOSS掉落必定掉裝備
    /// </summary>
    /// <returns></returns>
    public static int GetBossRandomEquipQuality()
    {
        int quality = 0;
        List<int> weightList = new List<int>(EquipQualityWeightList);
        weightList[0] = 0;
        quality = ProbabilityGetter.GetFromWeigth(weightList);
        return quality;
    }
    public static int GetRandomEquipType()
    {
        int equipType = 0;
        equipType = ProbabilityGetter.GetFromWeigth(DropEquipWeightList);
        return equipType;
    }
}
