using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public partial class Player
{
    public static Language UseLanguage { get; private set; }
    public static bool LocalData = true;
    public static bool MusicOn { get; private set; }
    public static bool SoundOn { get; private set; }
    public static bool IsRigister { get; private set; }
    public static bool LVSort { get; private set; }
    public static bool Tutorial { get; private set; }

    //玩家資料
    static bool PlayerInfoInitDataFinish = false;
    //強化
    static bool StrengthenInitDataFinish = false;
    //強化
    static bool EnchantInitDataFinish = false;
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

        //教學
        if (PlayerPrefs.GetInt(LocoData.Tutorial.ToString()) == 0)
            SetTutorial(true);
        else
            SetTutorial(false);
        //初始化
        if (PlayerPrefs.GetInt(LocoData.Init.ToString()) == 0)
        {
            PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.Init.ToString(), 1);
        }        
        if (PlayerPrefs.GetInt(LocoData.MusicOn.ToString()) == 1)
            SetMusic(true);
        else
            SetMusic(false);
        if (PlayerPrefs.GetInt(LocoData.SoundOn.ToString()) == 1)
            SetSound(true);
        else
            SetSound(false);

        /*如要藥第一次進遊戲時顯示語言選擇，就取消此段註解，改註解下一段
if (PlayerPrefs.GetInt(LocoData.InitSetLanguage.ToString()) == 0)
    InitSetLanguage = true;
   */
        if (PlayerPrefs.GetInt(LocoData.InitSetLanguage.ToString()) == 0)
        {
            PlayerPrefs.SetInt(LocoData.InitSetLanguage.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), (int)Language.EN);
            Player.SetLanguage((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));
        }
        else
            SetLanguage((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));

        if (LocalData)
            GetLocalData();
        else
            ServerRequest.QuickSignUp();
    }
    public static void SetTutorial(bool _bool)
    {
        Tutorial = _bool;
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
    public static void ClearLocoData()
    {
        foreach (int i in Enum.GetValues(typeof(LocoData)))
        {
            PlayerPrefs.DeleteKey(((LocoData)i).ToString());
        }
        Debug.Log("清空玩家本基資料");
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
            ClearLocoData();
        }


        //PlayerPrefs.SetInt(LocoData.Emerald.ToString(), 1000);
        //PlayerPrefs.SetInt(LocoData.Gold.ToString(), 10000);
        //Debug.Log((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));
        
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
        int freeEmerald = PlayerPrefs.GetInt(LocoData.FreeEmerald.ToString());
        int payEmerald = PlayerPrefs.GetInt(LocoData.PayEmerald.ToString());
        if (emerald != 0)
            SetEmeraldCB(emerald, 0, freeEmerald, payEmerald, "");

        //目前所在關卡
        int curFloor = PlayerPrefs.GetInt(LocoData.CurFloor.ToString());
        SetCurFloor_Local(curFloor);
        //最高突破關卡
        int maxFloor = PlayerPrefs.GetInt(LocoData.MaxFloor.ToString());
        SetMaxFloor_Local(maxFloor);
        //最高怪物擊殺
        int maxEnemyKills = PlayerPrefs.GetInt(LocoData.MaxEnemyKills.ToString());
        SetMaxEnemyKills_Local(maxEnemyKills);
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
        //附魔
        string enchantStr = PlayerPrefs.GetString(LocoData.Enchant.ToString());
        if (enchantStr != "")
        {
            string[] enchantData = enchantStr.Split('/');
            GetEnchant_CB(enchantData);
        }
        else
            EnchantInitDataFinish = true;
        if (false)
        {
            Debug.Log("CurFloor=" + CurFloor + "  MaxFloor=" + MaxFloor + "  gold=" + gold + "  emerald=" + emerald);
            Debug.Log("equipStr=" + equipStr);
            Debug.Log("strengthenStr=" + strengthenStr);
            Debug.Log("enchantStr=" + enchantStr);
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
        for(int i=0;i< _data.Length;i++)
        {
            if (_data[i] == null)
                _data[i] = "";
        }
        SetEmeraldCB(int.Parse(_data[2]), int.Parse(_data[3]), int.Parse(_data[4]), int.Parse(_data[5]), _data[6]);
        SetCurFloor_Local(int.Parse(_data[7]));
        SetMaxFloor_Local(int.Parse(_data[8]));
        //擊敗BOSS清單
        string killBossStr = _data[9];
        if (killBossStr != "")
        {
            string[] bossID = killBossStr.Split(',');
            for (int i = 0; i < bossID.Length; i++)
            {
                KillBossID.Add(int.Parse(bossID[i]));
            }
        }
        ServerRequest.GetEquip();
        ServerRequest.GetStrengthen();
        ServerRequest.GetEnchant();
        Debug.Log("PlayerInfoInitDataFinish OK!");
        PlayerInfoInitDataFinish = true;
    }
    public static void GetEquip_CB(string[] _data)
    {
        if (_data != null)
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
                string propertiesStr = "";
                if (properties.Length > 6)
                    propertiesStr = properties[6];//讀取本地資料要確定欄位數不然會炸掉，不能隨便追加資料，要追加要優化程式
                int enchantID = 0;
                if (properties.Length > 7)
                    enchantID = int.Parse(properties[7]);//讀取本地資料要確定欄位數不然會炸掉，不能隨便追加資料，要追加要優化程式
                switch (type)
                {
                    case EquipType.Weapon:
                        WeaponData w = WeaponData.GetNewWeapon(uid, jid, equipSlot, lv, quality, propertiesStr, enchantID);
                        wlist.Add(uid, w);
                        break;
                    case EquipType.Armor:
                        ArmorData a = ArmorData.GetNewArmor(uid, jid, equipSlot, lv, quality, propertiesStr, enchantID);
                        alist.Add(uid, a);
                        break;
                    case EquipType.Accessory:
                        AccessoryData ac = AccessoryData.GetNewAccessory(uid, jid, equipSlot, lv, quality, propertiesStr, enchantID);
                        aclist.Add(uid, ac);
                        break;
                }
            }
            Items[EquipType.Weapon] = wlist;
            Items[EquipType.Armor] = alist;
            Items[EquipType.Accessory] = aclist;
            Player.UpToDateCurMaxEquipUID();
        }
        EquipInitDataFinish = true;
        Debug.Log("EquipInitDataFinish OK!");
    }
    public static void GetStrengthen_CB(string[] _data)
    {
        if (_data != null)
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
        }
        StrengthenInitDataFinish = true;
        Debug.Log("StrengthenInitDataFinish OK!");
    }
    public static void GetEnchant_CB(string[] _data)
    {
        if (_data != null)
        {
            for (int i = 0; i < _data.Length; i++)
            {
                string[] properties = _data[i].Split(',');
                int jid = int.Parse(properties[0]);
                int lv = int.Parse(properties[1]);
                if (EnchantDic.ContainsKey(jid))
                {
                    EnchantDic[jid].InitSet(lv);
                    GameSettingData.EnchantPropertyOperate(EnchantPlus, EnchantDic[jid].Properties, Operator.Plus);//加上升級後的值
                }
            }
        }
        EnchantInitDataFinish = true;
        Debug.Log("EnchantInitDataFinish OK!");
        //送kongregate統計資料
        KongregateAPIBehaviour.SendKGStatistics("Great Adventurer", MaxFloor);
        KongregateAPIBehaviour.SendKGStatistics("Boss Hunter", Player.KillBossID.Count);
        KongregateAPIBehaviour.SendKGStatistics("Happy Partner", Player.GetPartnerEnchantCount());
        Debug.Log("Player.KillBossID.Count=" + Player.KillBossID.Count);
        Debug.Log("Player.GetPartnerEnchantCount=" + Player.GetPartnerEnchantCount());
    }
    public static void StrengthenUpgrade_CB(string[] _data)
    {
        Debug.Log("強化成功");
        //int jid = int.Parse(_data[0]);
        //StrengthenUpgrade(jid);
    }
    public static void EnchantUpgrade_CB(string[] _data)
    {
        Debug.Log("附魔成功");
        //int jid = int.Parse(_data[0]);
        //EnchantUpgrade(jid);
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
    public static void KillNewBoss_CB(string[] _data)
    {
        Debug.Log("更新玩家擊殺BOSS清單成功");
    }
    public static void ShowUserItemListCB(string _dataStr)
    {
        TrueEmerald = 0;
        PayKredsLog = "";
        if (_dataStr != "")
        {
            //163697,1,,/163698,2,,/164105,3,,
            //13356700,1,,1/13609218,1,,1/13356696,2,,1/13356697,2,,1/13356698,2,,1/13358938,2,,1/13345602,3,,/13355292,3,,/13356695,3,,1/13356699,3,,1/13357027,3,,1/13357029,3,,1/13357047,3,,1/13357066,3,,1/13358576,3,,1/13358579,3,,1/13582902,3,,1
            //1,1/1,1/2,1/2,1/2,1/2,1/3,/3,/3,1/3,1/3,1/3,1/3,1/3,1/3,1/3,1/3,1
            string[] items = _dataStr.Split('/');
            Dictionary<int, int> payTypeCount = new Dictionary<int, int>();
            for (int i = 0; i < items.Length; i++)
            {
                string[] itemData = items[i].Split(',');
                int id = int.Parse(itemData[0]);
                int count = 0;
                int.TryParse(itemData[1], out count);
                if (count != 0)
                {
                    if (GameDictionary.PurchaseDic[id].MyType == PurchaseType.BuyEmerald)
                    {
                        TrueEmerald += GameDictionary.PurchaseDic[id].Gain * count;
                        if (payTypeCount.ContainsKey(GameDictionary.PurchaseDic[id].ID))
                            payTypeCount[id] += count;
                        else
                            payTypeCount.Add(id, count);
                    }
                }
            }
            List<int> keys = new List<int>(payTypeCount.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (i != 0)
                    PayKredsLog += "/";
                PayKredsLog += string.Format("{0}={1}", keys[i], payTypeCount[keys[i]]);
            }
        }
        Debug.Log("UpdatePayKredsLog=" + PayKredsLog);
    }
}
