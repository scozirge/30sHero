using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public partial class GameDictionary
{
    //字典
    public static Dictionary<int, PurchaseData> PurchaseDic;
    public static Dictionary<int, StrengthenData> StrengthenDic;
    public static Dictionary<int, WeaponData> WeaponDic;
    public static Dictionary<int, ArmorData> ArmorDic;
    public static Dictionary<int, AccessoryData> AccessoryDic;
    public static Dictionary<string, GameSettingData> GameSettingDic;
    public static Dictionary<int, EnemyData> EnemyDic;
    //Case表
    public static Dictionary<int, CaseTableData> CaseTableDic;

    //String
    public static Dictionary<string, StringData> String_UIDic;
    public static Dictionary<string, StringData> String_PurchaseDic;
    public static Dictionary<string, StringData> String_StrengthenDic;
    public static Dictionary<string, StringData> String_WeaponDic;
    public static Dictionary<string, StringData> String_ArmorDic;
    public static Dictionary<string, StringData> String_AccessoryDic;
    public static Dictionary<string, StringData> String_CaseTableDic;

    ///// <summary>
    ///// 將Json資料寫入字典裡
    ///// </summary>
    static void LoadJsonDataToDic()
    {
        StringDataGetter StringGetter = new StringDataGetter();
        //字典
        GameSettingDic = new Dictionary<string, GameSettingData>();
        GameSettingData.SetData(GameSettingDic, "GameSetting");
        PurchaseDic = new Dictionary<int, PurchaseData>();
        PurchaseData.SetData(PurchaseDic, "Purchase");
        StrengthenDic = new Dictionary<int, StrengthenData>();
        StrengthenData.SetData(StrengthenDic, "Strengthen");
        WeaponDic = new Dictionary<int, WeaponData>();
        WeaponData.SetData(WeaponDic, "Weapon");
        ArmorDic = new Dictionary<int, ArmorData>();
        ArmorData.SetData(ArmorDic, "Armor");
        AccessoryDic = new Dictionary<int, AccessoryData>();
        AccessoryData.SetData(AccessoryDic, "Accessory");
        EnemyDic = new Dictionary<int, EnemyData>();
        EnemyData.SetData(EnemyDic, "Enemy");
        //Case
        CaseTableDic = new Dictionary<int, CaseTableData>();
        CaseTableData.SetData(CaseTableDic);

        //String
        String_UIDic = StringGetter.GetStringData("String_UI");
        String_PurchaseDic = StringGetter.GetStringData("String_Purchase");
        String_StrengthenDic = StringGetter.GetStringData("String_Strengthen");
        String_WeaponDic = StringGetter.GetStringData("String_Weapon");
        String_ArmorDic = StringGetter.GetStringData("String_Armor");
        String_AccessoryDic = StringGetter.GetStringData("String_Accessory");
        String_CaseTableDic = StringGetter.GetStringData("String_CaseTable");
    }
}
