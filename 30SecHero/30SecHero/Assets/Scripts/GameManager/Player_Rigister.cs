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

    //玩家資料
    static bool PlayerInfoInitDataFinish = false;
    //強化
    static bool StrengthenInitDataFinish = false;
    //遊戲開始讀取玩家裝備資料，讀取完EquipInitDataFinish=true(false時玩家設定裝備不會寫入本地或雲端)
    static bool EquipInitDataFinish = false;
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
        if (PlayerInfoInitDataFinish)
        {
            if (SoundOn)
                PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 1);
            else
                PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 0);
        }
    }
    public static void SetMusic(bool _on)
    {
        MusicOn = _on;
        AudioPlayer.MuteMusic(!MusicOn);
        if (PlayerInfoInitDataFinish)
        {
            if (MusicOn)
                PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 1);
            else
                PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 0);
        }

    }
    public static void GetLocalData()
    {
        //PlayerPrefs.DeleteKey(LocoData.Init.ToString());
        //PlayerPrefs.DeleteKey(LocoData.Equip.ToString());
        //PlayerPrefs.DeleteKey(LocoData.Strengthen.ToString());
        //PlayerPrefs.DeleteKey(LocoData.SoundOn.ToString());
        //PlayerPrefs.DeleteKey(LocoData.MusicOn.ToString());
        //設定
        bool ClearAllLocoData = false;
        if (ClearAllLocoData)
        {
            PlayerPrefs.DeleteKey(LocoData.UseLanguage.ToString());
            PlayerPrefs.DeleteKey(LocoData.Init.ToString());
            PlayerPrefs.DeleteKey(LocoData.Equip.ToString());
            PlayerPrefs.DeleteKey(LocoData.Strengthen.ToString());
            PlayerPrefs.DeleteKey(LocoData.SoundOn.ToString());
            PlayerPrefs.DeleteKey(LocoData.MusicOn.ToString());
        }

        if (PlayerPrefs.GetInt(LocoData.Init.ToString()) == 0)
        {
            PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.Init.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), 0);
        }
        //Debug.Log((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));
        SetLanguage((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));
        if (PlayerPrefs.GetInt(LocoData.MusicOn.ToString()) == 1)
            SetMusic(true);
        else
            SetMusic(false);
        if (PlayerPrefs.GetInt(LocoData.SoundOn.ToString()) == 1)
            SetSound(true);
        else
            SetSound(false);
        //資源
        int gold = PlayerPrefs.GetInt(LocoData.Gold.ToString());

        if (gold != 0)
            SetGold(gold);
        int emerald = PlayerPrefs.GetInt(LocoData.Emerald.ToString());
        if (emerald != 0)
            SetEmerald(emerald);

        //目前所在關卡
        int curFloor = PlayerPrefs.GetInt(LocoData.CurFloor.ToString());
        SetCurFloor_Local(curFloor);
        //最高突破關卡
        int maxFloor = PlayerPrefs.GetInt(LocoData.MaxFloor.ToString());
        SetMaxFloor_Local(maxFloor);
        //最高怪物擊殺
        int maxEnemyKills = PlayerPrefs.GetInt(LocoData.MaxEnemyKills.ToString());
        SetMaxFloor_Local(maxEnemyKills);
        //擊敗BOSS清單
        string killBossStr = PlayerPrefs.GetString(LocoData.KillBossID.ToString());
        if (killBossStr != "")
        {
            string[] bossID = killBossStr.Split(',');
            for (int i = 0; i < bossID.Length; i++)
            {
                KillBossID.Add(int.Parse(bossID[i]));
            }
        }
        PlayerInfoInitDataFinish = true;
        //裝備
        string equipStr = PlayerPrefs.GetString(LocoData.Equip.ToString());

        if (equipStr != "")
        {
            string[] equipData = equipStr.Split('/');
            GetEquip_CB(equipData);
        }
        else
            EquipInitDataFinish = true;
        //強化
        string strengthenStr = PlayerPrefs.GetString(LocoData.Strengthen.ToString());

        if (strengthenStr != "")
        {
            string[] strengthenData = strengthenStr.Split('/');
            GetStrengthen_CB(strengthenData);
        }
        else
            StrengthenInitDataFinish = true;
        if (true)
        {
            Debug.Log("CurFloor=" + CurFloor);
            Debug.Log("MaxFloor=" + MaxFloor);
            Debug.Log("gold=" + gold);
            Debug.Log("emerald=" + emerald);
            Debug.Log("equipStr=" + equipStr);
            Debug.Log("strengthenStr=" + strengthenStr);
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
        SetCurFloor_Local(int.Parse(_data[3]));
        SetMaxFloor_Local(int.Parse(_data[4]));
        Debug.Log("CurFloor=" + int.Parse(_data[3]));
        Debug.Log("MaxFloor=" + int.Parse(_data[4]));
        ServerRequest.GetEquip();
        ServerRequest.GetStrengthen();
        PlayerInfoInitDataFinish = true;
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
            string propertiesStr = properties[6];//讀取本地資料要確定欄位數不然會炸掉，不能隨便追加資料，要追加要優化程式
            switch (type)
            {
                case EquipType.Weapon:
                    WeaponData w = WeaponData.GetNewWeapon(uid, jid, equipSlot, lv, quality, propertiesStr);
                    wlist.Add(uid, w);
                    break;
                case EquipType.Armor:
                    ArmorData a = ArmorData.GetNewArmor(uid, jid, equipSlot, lv, quality, propertiesStr);
                    alist.Add(uid, a);
                    break;
                case EquipType.Accessory:
                    AccessoryData ac = AccessoryData.GetNewAccessory(uid, jid, equipSlot, lv, quality, propertiesStr);
                    aclist.Add(uid, ac);
                    break;
            }
        }
        Itmes[EquipType.Weapon] = wlist;
        Itmes[EquipType.Armor] = alist;
        Itmes[EquipType.Accessory] = aclist;
        EquipInitDataFinish = true;
    }
    public static void GetStrengthen_CB(string[] _data)
    {
        for (int i = 0; i < _data.Length; i++)
        {
            string[] properties = _data[i].Split(',');
            int jid = int.Parse(properties[0]);
            int lv = int.Parse(properties[1]);
            if (StrengthenDic.ContainsKey(jid))
            {
                StrengthenDic[jid].InitSet(lv);
                GameSettingData.RolePropertyOperate(StrengthenPlus, StrengthenDic[jid].Properties, Operator.Plus);//加上升級後的值
            }
        }
        StrengthenInitDataFinish = true;
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
    public static void UpdateResource_CB(string[] _data)
    {
        Debug.Log("更新玩家資源成功");
    }
}
