﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class Player
{
    public static Language UseLanguage { get; private set; }
    public static bool MusicOn { get; private set; }
    public static bool SoundOn { get; private set; }
    public static bool IsRigister { get; private set; }
    public static bool LVSort { get; private set; }
    /// <summary>
    /// 設定語言
    /// </summary>
    public static void SetLanguage(Language _language)
    {
        UseLanguage = _language;
        MyText.RefreshActivityTexts();
        switch (UseLanguage)
        {
            case Language.ZH_TW:
                PlayerPrefs.SetInt("UseLanguage", 0);
                break;
            case Language.ZH_CN:
                PlayerPrefs.SetInt("UseLanguage", 1);
                break;
            case Language.EN:
                PlayerPrefs.SetInt("UseLanguage", 2);
                break;
            default:
                PlayerPrefs.SetInt("UseLanguage", 0);
                break;
        }
    }
    public static void SetSound(bool _on)
    {
        SoundOn = _on;
        AudioPlayer.MuteSound(!SoundOn);
        if (SoundOn)
            PlayerPrefs.SetInt("SoundOn", 1);
        else
            PlayerPrefs.SetInt("SoundOn", 0);
    }
    public static void SetMusic(bool _on)
    {
        MusicOn = _on;
        AudioPlayer.MuteMusic(!MusicOn);
        if (MusicOn)
            PlayerPrefs.SetInt("MusicOn", 1);
        else
            PlayerPrefs.SetInt("MusicOn", 0);
    }
    public static void SetKongregateUserData_CB(string _name, int _kongregateID)
    {
        Name_K = _name;
        UserID_K = _kongregateID;
        ServerRequest.QuickSignUp();
        KongregateAPIBehaviour.ShowItemList();
    }
    public static void SignIn_CB(string[] _data)
    {
        ID = int.Parse(_data[0]);
        GainGold(int.Parse(_data[1]));
        GainEmerald(int.Parse(_data[2]));
        ServerRequest.GetEquip();
    }
    public static void GetStrengthen_CB(string[] _data)
    {
        for (int i = 0; i < _data.Length; i++)
        {
            string[] properties = _data[i].Split(',');
            int jid = int.Parse(properties[0]);
            int lv = int.Parse(properties[1]);
            if (StrengthenDic.ContainsKey(jid))
                StrengthenDic[jid].SetLV(lv);
        }
    }
    public static void GetEquip_CB(string[] _data)
    {
        Dictionary<long, EquipData> wlist = new Dictionary<long, EquipData>();
        Dictionary<long, EquipData> alist = new Dictionary<long, EquipData>();
        Dictionary<long, EquipData> aclist = new Dictionary<long, EquipData>();
        for (int i = 0; i < _data.Length; i++)
        {
            string[] properties = _data[i].Split(',');
            int uid = int.Parse(properties[0]);
            int jid = int.Parse(properties[1]);
            EquipType type = (EquipType)int.Parse(properties[2]);
            int equipSlot = int.Parse(properties[3]);
            int lv = int.Parse(properties[4]);
            int quality = int.Parse(properties[5]);

            switch (type)
            {
                case EquipType.Weapon:
                    WeaponData w = WeaponData.GetNewWeapon(uid, jid, equipSlot, lv, quality);
                    wlist.Add(uid, w);
                    break;
                case EquipType.Armor:
                    ArmorData a = ArmorData.GetNewArmor(uid, jid, equipSlot, lv, quality);
                    alist.Add(uid, a);
                    break;
                case EquipType.Accessory:
                    AccessoryData ac = AccessoryData.GetNewAccessory(uid, jid, equipSlot, lv, quality);
                    aclist.Add(uid, ac);
                    break;
            }
        }
        Itmes.Add(EquipType.Weapon, wlist);
        Itmes.Add(EquipType.Armor, alist);
        Itmes.Add(EquipType.Accessory, aclist);
        ServerRequest.GetStrengthen();
    }
    public static void StrengthenUpgrade_CB(string[] _data)
    {
        Debug.Log("強化成功");
        //int jid = int.Parse(_data[0]);
        //StrengthenUpgrade(jid);
    }
    public static void ChangeEquip_CB(string[] _data)
    {
        Debug.Log("更換裝備成功");
    }
    public static void SellEquip_CB(string[] _data)
    {
        Debug.Log("販賣裝備成功");
    }
}
