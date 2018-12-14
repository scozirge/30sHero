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
            PlayerPrefs.DeleteKey(LocoData.CurFloor.ToString());
            PlayerPrefs.DeleteKey(LocoData.MaxFloor.ToString());
            PlayerPrefs.DeleteKey(LocoData.MaxEnemyKills.ToString());
            PlayerPrefs.DeleteKey(LocoData.KillBossID.ToString());
            PlayerPrefs.DeleteKey(LocoData.Gold.ToString());
            PlayerPrefs.DeleteKey(LocoData.Emerald.ToString());
            PlayerPrefs.DeleteKey(LocoData.Enchant.ToString());
        }

        if (PlayerPrefs.GetInt(LocoData.Init.ToString()) == 0)
        {
            PlayerPrefs.SetInt(LocoData.SoundOn.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.MusicOn.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.Init.ToString(), 1);
            PlayerPrefs.SetInt(LocoData.UseLanguage.ToString(), 2);
        }
        //PlayerPrefs.SetInt(LocoData.Emerald.ToString(), 1000);
        //PlayerPrefs.SetInt(LocoData.Gold.ToString(), 10000);
        //Debug.Log((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));
        SetLanguage((Language)PlayerPrefs.GetInt(LocoData.UseLanguage.ToString()));
        //PlayerPrefs.SetString(LocoData.Strengthen.ToString(), "4,19/6,2/8,1/9,2");
        //PlayerPrefs.SetString(LocoData.Equip.ToString(), "2,294,0,0,1,2,0=12&3=0.05/65,269,0,0,3,2,0=20&5=60/73,72,0,0,3,2,0=20&4=30/78,5,0,0,3,1,0=20/80,203,0,0,3,1,0=20/85,79,0,0,3,3,0=20&4=30&13=0.02/87,49,0,0,3,1,0=20/89,57,0,0,3,1,0=20/91,264,0,0,3,2,0=20&5=60/94,200,0,0,3,1,0=20/106,78,0,0,4,1,0=24/120,131,0,0,4,1,0=24/123,242,0,0,4,1,0=24/126,159,0,0,4,1,0=24/127,184,0,0,4,2,0=24&8=1/130,249,0,0,4,1,0=24/135,8,0,0,4,2,0=24&3=0.05/136,51,0,0,4,2,0=24&6=0.5/138,142,0,0,4,1,0=24/144,111,0,0,4,1,0=24/145,156,0,0,4,1,0=24/149,201,0,0,4,2,0=24&10=0.05/151,133,0,0,4,1,0=24/150,212,0,0,4,2,0=24&1=40/154,183,0,0,4,3,0=24&1=40&13=0.02/156,118,0,0,4,1,0=24/239,150,0,0,5,1,0=28/241,6,0,0,5,1,0=28/242,31,0,0,5,1,0=28/343,9,0,0,6,2,0=32&4=30/348,218,0,0,6,2,0=35/353,115,0,0,6,1,0=32/359,155,0,0,6,4,0=32&3=0.05&7=10/360,293,0,0,6,1,0=32/361,249,0,0,6,1,0=32/364,38,0,0,6,3,0=32&9=1&12=3/370,176,0,0,6,2,0=32&7=5/372,52,0,0,6,1,0=32/371,293,0,0,6,2,0=32&7=5/379,230,0,0,6,1,0=32/381,194,0,0,6,1,0=32/384,292,0,0,6,2,0=32&15=0.3/385,134,0,0,6,1,0=32/423,92,0,0,7,1,0=36/426,38,0,0,7,1,0=36/429,139,0,0,7,1,0=36/430,7,0,0,7,3,0=36&3=0.05&6=0.5/478,182,0,0,8,2,0=40&14=0.3/479,140,0,0,8,1,0=40/480,231,0,0,8,1,0=40/483,288,0,0,8,1,0=40/485,130,0,0,8,1,0=40/487,70,0,0,8,1,0=40/489,52,0,0,8,2,0=40&1=80/494,233,0,0,8,1,0=40/496,74,0,0,8,2,0=40&12=4/536,255,0,0,9,1,0=44/539,117,0,0,9,1,0=44/540,221,0,0,9,1,0=44/541,265,0,0,9,2,0=44&1=90/551,40,0,0,9,5,0=48.5&3=0.05&14=0.6,52/560,22,0,0,10,1,0=48/566,263,0,0,10,1,0=48/568,62,0,0,10,1,0=48/578,37,0,0,10,2,0=48&14=0.3/583,232,0,0,10,2,0=48&1=100/582,163,0,1,10,5,0=53&2=16&3=0.05,66/588,224,0,0,10,4,0=48&1=200&15=0.3/591,82,0,0,10,2,0=48&12=5/592,47,0,0,10,1,0=48/610,162,0,0,10,1,0=48/608,150,0,0,10,2,0=48&14=0.3/60,53,1,0,3,1,1=200/67,30,1,0,3,1,1=200/9,121,1,0,2,3,0=1&1=160&7=5/72,166,1,0,3,1,1=200/71,92,1,0,3,1,1=200/74,68,1,0,3,1,1=200/76,112,1,0,3,3,1=200&12=1.5&13=0.02/77,204,1,0,3,1,1=200/81,21,1,0,3,2,1=230/82,77,1,0,3,2,1=200&3=0.05/86,204,1,0,3,4,0=1.5&1=200&5=60&6=0.5/110,176,1,0,4,1,1=240/113,40,1,0,4,2,1=240&12=2/117,218,1,0,4,1,1=240/116,144,1,0,4,2,1=280/122,96,1,0,4,1,1=240/124,124,1,0,4,2,0=2&1=240/129,8,1,0,4,1,1=240/133,187,1,0,4,2,1=240&15=0.3/134,186,1,0,4,2,1=240&2=3.2/137,232,1,0,4,1,1=240/139,164,1,0,4,3,1=240&4=30&13=0.02/152,46,1,0,4,1,1=240/157,22,1,0,4,1,1=240/244,131,1,0,5,1,1=280/248,207,1,0,5,1,1=280/252,211,1,0,5,3,1=280&6=0.5&14=0.3/368,37,1,0,6,1,1=320/373,139,1,0,6,2,1=320&13=0.02/380,76,1,0,6,1,1=320/424,99,1,0,7,1,1=360/472,211,1,0,8,3,1=400&10=0.05&14=0.3/477,1,1,2,8,5,1=560&8=1&13=0.02,48/484,5,1,0,8,3,1=400&3=0.05&4=30/493,98,1,0,8,2,1=400&15=0.3/495,57,1,0,8,2,1=400&15=0.3/499,209,1,0,8,1,1=400/501,8,1,0,8,1,1=400/500,63,1,0,8,1,1=400/532,62,1,0,9,1,1=440/533,95,1,0,9,2,0=4.5&1=440/535,107,1,0,9,3,1=440&9=1&10=0.05/543,135,1,0,9,2,0=4.5&1=440/544,222,1,0,9,1,1=440/547,85,1,0,9,1,1=440/553,30,1,0,9,1,1=440/555,83,1,0,9,1,1=440/558,203,1,0,10,1,1=480/565,154,1,0,10,3,1=480&7=5&10=0.05/575,51,1,0,10,1,1=480/576,55,1,0,10,1,1=480/577,244,1,0,10,1,1=480/581,164,1,0,10,1,1=480/586,181,1,0,10,2,1=480&15=0.3/599,195,1,0,10,2,1=480&15=0.3/602,26,1,0,10,1,1=480/603,114,1,0,10,1,1=480/607,86,1,0,10,1,1=480/606,29,1,0,10,1,1=480/609,2,1,0,10,1,1=480/611,176,1,0,10,1,1=480/59,37,2,0,3,1,2=25/61,97,2,0,3,2,2=25&7=5/5,73,2,0,1,3,2=15&8=1&11=5/63,9,2,0,3,1,2=25/62,139,2,0,3,3,2=25&3=0.05&5=60/11,119,2,0,2,3,1=20&2=20&14=0.3/64,232,2,0,3,1,2=25/66,4,2,0,3,1,2=25/68,81,2,0,3,2,0=1.5&2=25/69,134,2,0,3,2,2=25&12=1.5/70,114,2,0,3,4,0=1.5&1=30&2=27.4/75,144,2,0,3,1,2=25/79,210,2,0,3,1,2=25/83,140,2,0,3,1,2=25/84,101,2,0,3,1,2=25/88,156,2,0,3,1,2=25/90,162,2,0,3,1,2=25/92,44,2,0,3,3,2=25&6=0.5&10=0.05/93,11,2,0,3,1,2=25/107,70,2,0,4,2,0=2&2=30/108,122,2,0,4,3,1=40&2=30&11=5/109,29,2,0,4,1,2=30/111,108,2,0,4,4,1=40&2=30&6=0.5&14=0.3/112,9,2,0,4,3,2=30&7=5&13=0.02/114,89,2,0,4,5,0=2&2=30&9=2&10=0.05/115,141,2,0,4,1,2=30/118,201,2,0,4,1,2=30/119,57,2,0,4,1,2=30/121,217,2,0,4,1,2=30/125,56,2,0,4,2,2=33.2/128,79,2,0,4,3,1=40&2=30&13=0.02/131,203,2,0,4,1,2=30/132,4,2,0,4,4,0=2&2=30&5=60&8=1/140,133,2,0,4,1,2=30/141,12,2,0,4,1,2=30/142,24,2,0,4,2,0=2&2=30/143,162,2,0,4,1,2=30/146,149,2,0,4,1,2=30/147,244,2,0,4,1,2=30/148,229,2,0,4,2,2=30&13=0.02/153,154,2,0,4,2,2=33.2/155,238,2,0,4,2,2=30&3=0.05/158,41,2,0,4,2,2=30&14=0.3/240,238,2,0,5,4,1=50&2=35&10=0.05&14=0.3/243,65,2,0,5,1,2=35/245,167,2,0,5,1,2=35/246,11,2,0,5,3,2=39&15=0.3/247,220,2,0,5,1,2=35/249,174,2,0,5,2,2=35&13=0.02/250,158,2,0,5,2,2=35&3=0.05/251,27,2,0,5,1,2=35/254,228,2,0,5,1,2=35/253,72,2,0,5,3,2=35&11=5&15=0.3/255,27,2,0,5,3,2=35&9=1&14=0.3/344,231,2,0,6,1,2=40/345,29,2,0,6,1,2=40/346,22,2,0,6,1,2=40/350,26,2,0,6,1,2=40/351,231,2,0,6,1,2=40/349,56,2,0,6,2,2=40&9=1/347,113,2,0,6,1,2=40/352,70,2,0,6,1,2=40/354,83,2,0,6,2,2=40&7=5/355,159,2,0,6,1,2=40/357,239,2,0,6,1,2=40/356,221,2,0,6,1,2=40/358,11,2,0,6,1,2=40/362,186,2,0,6,1,2=40/363,68,2,0,6,1,2=40/365,103,2,3,6,5,1=60&2=40&6=0.5&8=1&11=5/366,158,2,0,6,2,2=40&15=0.3/367,10,2,0,6,1,2=40/369,71,2,0,6,2,2=40&14=0.3/374,49,2,0,6,2,2=40&5=60/375,169,2,0,6,2,2=40&8=1/376,217,2,0,6,1,2=40/377,55,2,0,6,2,2=44.8/378,218,2,0,6,1,2=40/383,11,2,0,6,2,2=40&9=1/382,142,2,0,6,1,2=40/422,141,2,0,7,2,2=45&5=60/425,70,2,0,7,2,2=45&8=1/427,204,2,0,7,2,0=3.5&2=45/428,168,2,4,7,3,1=70&2=50.6/431,203,2,0,7,1,2=45/432,70,2,0,7,2,2=50.6/473,135,2,0,8,2,2=50&8=1/474,106,2,0,8,2,2=50&13=0.02/475,174,2,0,8,1,2=50/476,239,2,0,8,1,2=50/481,25,2,0,8,1,2=50/482,208,2,0,8,3,2=50&8=1&12=4/486,106,2,0,8,1,2=50/488,16,2,0,8,5,2=50&5=60&8=1&11=5&14=0.3/490,193,2,0,8,1,2=50/491,167,2,0,8,1,2=50/492,198,2,0,8,1,2=50/498,141,2,0,8,1,2=50/497,226,2,0,8,1,2=50/503,44,2,0,8,1,2=50/534,10,2,0,9,3,2=55&9=1&11=5/537,240,2,0,9,2,2=55&13=0.02/538,233,2,0,9,1,2=55/542,47,2,0,9,4,2=55&3=0.05&5=60&8=1/545,160,2,0,9,2,2=62.2/546,2,2,0,9,2,2=55&10=0.05/548,142,2,0,9,2,2=62.2/549,233,2,0,9,1,2=55/550,118,2,0,9,4,2=55&5=60&7=5&12=4.5/552,182,2,0,9,1,2=55/554,224,2,0,9,2,2=55&13=0.02/556,131,2,0,9,1,2=55/559,89,2,0,10,1,2=60/561,205,2,0,10,1,2=60/562,54,2,0,10,2,2=60&6=0.5/563,133,2,0,10,2,0=5&2=60/564,237,2,0,10,3,2=60&7=5&14=0.3/567,116,2,0,10,3,2=60&8=1&13=0.02/569,139,2,0,10,2,0=5&2=60/570,150,2,0,10,1,2=60/571,68,2,0,10,1,2=60/572,166,2,0,10,1,2=60/573,63,2,0,10,1,2=60/574,1,2,0,10,1,2=60/579,188,2,0,10,2,2=60&6=0.5/580,199,2,0,10,3,2=60&12=5&15=0.3/584,188,2,0,10,1,2=60/585,194,2,0,10,3,2=60&3=0.05&12=5/587,3,2,0,10,2,2=60&15=0.3/589,45,2,0,10,1,2=60/590,69,2,0,10,2,2=60&12=5/593,23,2,0,10,3,2=60&11=5&15=0.3/595,74,2,0,10,3,0=5&2=60&11=5/594,62,2,0,10,1,2=60/597,203,2,0,10,1,2=60/596,146,2,0,10,2,2=60&12=5/598,167,2,0,10,1,2=60/601,119,2,0,10,2,1=100&2=60/600,89,2,0,10,2,2=60&3=0.05/604,16,2,0,10,1,2=60/605,233,2,0,10,3,2=60&6=0.5&14=0.3");
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
        //附魔
        string enchantStr = PlayerPrefs.GetString(LocoData.Enchant.ToString());

        if (enchantStr != "")
        {
            string[] enchantData = enchantStr.Split('/');
            GetEnchant_CB(enchantData);
        }
        else
            EnchantInitDataFinish = true;
        if (true)
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
        SetEmerald(int.Parse(_data[2]));
        SetCurFloor_Local(int.Parse(_data[3]));
        SetMaxFloor_Local(int.Parse(_data[4]));
        Debug.Log("CurFloor=" + int.Parse(_data[3]));
        Debug.Log("MaxFloor=" + int.Parse(_data[4]));
        ServerRequest.GetEquip();
        ServerRequest.GetStrengthen();
        ServerRequest.GetEnchant();
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
        Itmes[EquipType.Weapon] = wlist;
        Itmes[EquipType.Armor] = alist;
        Itmes[EquipType.Accessory] = aclist;
        EquipInitDataFinish = true;
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
}
