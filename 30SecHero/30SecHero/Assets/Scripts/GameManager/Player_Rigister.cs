using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public partial class Player
{
    public static Language UseLanguage { get; private set; }
    public static bool LocalData = true;
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
                PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), 0);
                break;
            case Language.ZH_CN:
                PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), 1);
                break;
            case Language.EN:
                PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), 2);
                break;
            default:
                PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), 0);
                break;
        }
    }
    public static void UseLocalData(bool _bool)
    {
        LocalData = _bool;
        Debug.Log("UseLocalData:" + _bool);
        Player.Init();
        if (LocalData)
            GetLocalData();
        else
            ServerRequest.QuickSignUp();
    }
    public static void SetSound(bool _on)
    {
        SoundOn = _on;
        AudioPlayer.MuteSound(!SoundOn);
        if (SoundOn)
            PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 1);
        else
            PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 0);
    }
    public static void SetMusic(bool _on)
    {
        MusicOn = _on;
        AudioPlayer.MuteMusic(!MusicOn);
        if (MusicOn)
            PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 1);
        else
            PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 0);
    }
    public static void GetLocalData()
    {
        //資源
        int gold = PlayerPrefs.GetInt(LocoData.Gold.ToString());
        Debug.Log("gold=" + gold);
        if (gold != 0)
            SetGold(gold);
        int emerald = PlayerPrefs.GetInt(LocoData.Emerald.ToString());
        if (emerald != 0)
            SetEmerald(emerald);
        Debug.Log("emerald=" + emerald);
        //裝備
        string equipStr = PlayerPrefs.GetString(LocoData.Equip.ToString());
        Debug.Log("equipStr=" + equipStr);
        if(equipStr!="")
        {
            string[] equipData = equipStr.Split('/');
            GetEquip_CB(equipData);
        }
        //強化
        string strengthenStr = PlayerPrefs.GetString(LocoData.Strengthen.ToString());
        Debug.Log("strengthenStr=" + strengthenStr);
        if(strengthenStr!="")
        {
            string[] strengthenData = strengthenStr.Split('/');
            GetStrengthen_CB(strengthenData);
        }
    }
    public static void GetKongregateUserData_CB(string _name, int _kongregateID)
    {
        Name_K = _name;
        UserID_K = _kongregateID;
    }
    public static void SignIn_CB(string[] _data)
    {
        ID = int.Parse(_data[0]);
        SetGold(int.Parse(_data[1]));
        SetEmerald(int.Parse(_data[2]));
        ServerRequest.GetEquip();
        ServerRequest.GetStrengthen();
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
    public static void PurchaseEmerald_CB(string[] _data)
    {
        Debug.Log("購買綠寶石成功");
    }
}
