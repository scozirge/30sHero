using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Player
{
    public static bool IsInit
    {
        get
        {
            if (PlayerInfoInitDataFinish && StrengthenInitDataFinish && EquipInitDataFinish)
                return true;
            else
                return false;
        }
    }
    public static int ID { get; private set; }
    public static int UserID_K { get; private set; }
    public static string Name_K { get; private set; }
    public static int Gold { get; private set; }
    public static int Emerald { get; private set; }
    public static int PayEmerald { get; private set; }
    public static int FreeEmerald { get; private set; }
    public static int TrueEmerald { get; private set; }
    public static string PayKredsLog { get; private set; }
    public static List<int> KillBossID = new List<int>();
    public static int CurFloor { get; private set; }
    public static int MaxFloor { get; private set; }
    public static int MaxEnemyKills { get; private set; }

    public static void Init()
    {
        if (IsInit)
            return;
        Itmes.Add(EquipType.Weapon, new Dictionary<long, EquipData>());
        Itmes.Add(EquipType.Armor, new Dictionary<long, EquipData>());
        Itmes.Add(EquipType.Accessory, new Dictionary<long, EquipData>());
        InitProperty();
    }

    public static void SetGold(int _gold)
    {
        Gold = _gold;
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Gold);
            }
        }
    }
    public static void SetEmeraldCB(int _emerald, int _trueEmerald, int _freeEmerald, int _payEmerald, string _payKredsLog)
    {
        FreeEmerald = _freeEmerald;
        PayEmerald = _payEmerald;
        Emerald = _emerald;
        if(!Player.LocalData)
        {
            if (TrueEmerald != _trueEmerald || PayKredsLog != _payKredsLog)
            {
                Emerald = TrueEmerald + FreeEmerald - PayEmerald;
                ServerRequest.UpdateResource();
            }
        }

        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Emerald);
                PlayerPrefs.SetInt(LocoData.FreeEmerald.ToString(), FreeEmerald);
                PlayerPrefs.SetInt(LocoData.PayEmerald.ToString(), PayEmerald);
            }
        }
    }
    public static void GainGold(int _gold)
    {
        if (_gold == 0)
            return;
        Gold += _gold;
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Gold);
            }
        }
    }
    public static void TradeEmeraldForGold(int _emerald, int _gold)
    {
        if (_gold == 0)
            return;
        Gold += _gold;
        Emerald -= _emerald;
        PayEmerald += Mathf.Abs(_emerald);
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Gold.ToString(), Gold);
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Emerald);
                PlayerPrefs.SetInt(LocoData.PayEmerald.ToString(), PayEmerald);
            }
        }
    }
    public static void GainEmerald(int _emerald, bool _trueEmerald)
    {
        if (_emerald == 0)
            return;
        Emerald += _emerald;
        if (_emerald > 0)
        {
            if (_trueEmerald)
                TrueEmerald += _emerald;
            else
                FreeEmerald += _emerald;
        }
        else
        {
            PayEmerald = Mathf.Abs(_emerald);
        }
        Main.UpdateResource();
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            if (!LocalData)
            {
                Debug.Log("更新server玩家資源");
                ServerRequest.UpdateResource();
            }
            else
            {
                Debug.Log("更新Loco玩家資源");
                PlayerPrefs.SetInt(LocoData.Emerald.ToString(), Emerald);
            }
        }
    }
    public static bool CanStrengthenTipChack()
    {
        List<int> keys = new List<int>(StrengthenDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (StrengthenDic[keys[i]].GetPrice() <= Player.Gold)
                return true;
        }
        return false;
    }
    public static bool CanEnchantTipCheck()
    {
        List<int> keys = new List<int>(EnchantDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (EnchantDic[keys[i]].MyEnchantType == EnchantType.Enchant && EnchantDic[keys[i]].LV > 0)
            {
                if (EnchantDic[keys[i]].GetPrice() <= Player.Emerald)
                    return true;
            }
            else
                continue;
        }
        return false;
    }
    public static void StrengthenUpgrade(StrengthenData _data)
    {
        //執行強化
        GainGold(-_data.GetPrice());
        if (StrengthenDic.ContainsKey(_data.ID))
        {
            GameSettingData.RolePropertyOperate(StrengthenPlus, StrengthenDic[_data.ID].Properties, Operator.Minus);//減去原本值
            StrengthenDic[_data.ID].LVUP();
            GameSettingData.RolePropertyOperate(StrengthenPlus, StrengthenDic[_data.ID].Properties, Operator.Plus);//加上升級後的值
        }
        //寫入資料
        if (StrengthenInitDataFinish)
        {
            if (LocalData)
            {
                Debug.Log("更新Loco玩家強化");
                //強化
                List<int> keys = new List<int>(StrengthenDic.Keys);
                string dataStr = "";
                for (int i = 0; i < keys.Count; i++)
                {
                    if (StrengthenDic[keys[i]].LV > 0)
                    {
                        if (dataStr != "")
                            dataStr += "/";
                        dataStr += StrengthenDic[keys[i]].ID + "," + StrengthenDic[keys[i]].LV;
                    }
                }
                PlayerPrefs.SetString(LocoData.Strengthen.ToString(), dataStr);
            }
            else
            {
                Debug.Log("更新server玩家強化");
                ServerRequest.StrengthenUpgrade(_data.ID, _data.LV, Gold);
            }
        }
    }
    public static void RefreshEquipEnchant()
    {
        List<int> keys = new List<int>(EnchantDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (EnchantDic[keys[i]].MyEnchantType != EnchantType.Enchant)
            {
                if (EnchantPlus.ContainsKey(EnchantDic[keys[i]].PropertyType))
                    EnchantPlus[EnchantDic[keys[i]].PropertyType] = 0;
            }
        }
        if (MyWeapon != null && MyWeapon.MyEnchant != null)
            EnchantPlus[MyWeapon.MyEnchant.PropertyType] = MyWeapon.MyEnchant.GetValue();
        if (MyArmor != null && MyArmor.MyEnchant != null)
            EnchantPlus[MyArmor.MyEnchant.PropertyType] = MyArmor.MyEnchant.GetValue();
        if (MyAccessorys[0] != null && MyAccessorys[0].MyEnchant != null)
            EnchantPlus[MyAccessorys[0].MyEnchant.PropertyType] = MyAccessorys[0].MyEnchant.GetValue();
        if (MyAccessorys[1] != null && MyAccessorys[1].MyEnchant != null)
            EnchantPlus[MyAccessorys[1].MyEnchant.PropertyType] = MyAccessorys[1].MyEnchant.GetValue();
    }
    public static void EnchantUpgrade(EnchantData _data, bool _needPay)
    {
        //執行附魔
        if (_needPay)
            GainEmerald(-_data.GetPrice(), false);
        GameSettingData.EnchantPropertyOperate(EnchantPlus, _data.Properties, Operator.Minus);//減去原本值
        _data.LVUP();
        GameSettingData.EnchantPropertyOperate(EnchantPlus, _data.Properties, Operator.Plus);//加上升級後的值
        //寫入資料
        if (EnchantInitDataFinish)
        {
            if (LocalData)
            {
                Debug.Log("更新Loco玩家附魔");
                //附魔
                List<int> keys = new List<int>(EnchantDic.Keys);
                string dataStr = "";
                for (int i = 0; i < keys.Count; i++)
                {
                    if (EnchantDic[keys[i]].LV > 0)
                    {
                        if (dataStr != "")
                            dataStr += "/";
                        dataStr += EnchantDic[keys[i]].ID + "," + EnchantDic[keys[i]].LV;
                    }
                }
                PlayerPrefs.SetString(LocoData.Enchant.ToString(), dataStr);
            }
            else
            {
                Debug.Log("更新server玩家附魔");
                ServerRequest.EnchantUpgrade(_data.ID, _data.LV, Emerald);
            }
        }
    }
    public static void SetCurFloor_Local(int _curFloor)
    {
        if (_curFloor == CurFloor)
            return;
        if (_curFloor < 1)
            _curFloor = 1;
        CurFloor = _curFloor;
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            Debug.Log("更新Loco玩家目前樓層");
            PlayerPrefs.SetInt(LocoData.CurFloor.ToString(), CurFloor);
        }
    }
    public static void SetMaxFloor_Local(int _maxFloor)
    {
        if (_maxFloor == MaxFloor)
            return;
        if (_maxFloor < 1)
            _maxFloor = 1;
        MaxFloor = _maxFloor;
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            Debug.Log("更新Loco玩家最高樓層");
            PlayerPrefs.SetInt(LocoData.MaxFloor.ToString(), MaxFloor);
        }
    }
    public static void SetMaxEnemyKills_Local(int _maxEnemyKills)
    {
        if (MaxEnemyKills == _maxEnemyKills)
            return;
        MaxEnemyKills = _maxEnemyKills;
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            Debug.Log("更新Loco玩家殺敵數");
            PlayerPrefs.SetInt(LocoData.MaxEnemyKills.ToString(), MaxEnemyKills);
        }
    }
    public static void KillNewBoss(int _id)
    {
        if (_id <= 0)
            return;
        //寫入資料
        if (PlayerInfoInitDataFinish)
        {
            Debug.Log("更新Loco玩家擊殺BOSS清單");
            KillBossID.Add(_id);
            string killBossStr = "";
            for (int i = 0; i < KillBossID.Count; i++)
            {
                if (killBossStr != "")
                    killBossStr += ",";
                killBossStr += KillBossID[i].ToString();
            }
            if (LocalData)
            {
                PlayerPrefs.SetString(LocoData.KillBossID.ToString(), killBossStr);
            }
            else
            {
                ServerRequest.KillNewBoss(killBossStr);
            }
        }
    }
    public static void GainEquip_Local(List<EquipData> _datas)
    {
        if (_datas == null || _datas.Count == 0)
            return;
        //寫入資料
        if (EquipInitDataFinish)
        {
            Debug.Log("更新Loco玩家裝備");
            for (int i = 0; i < _datas.Count; i++)
            {
                //Debug.Log("Type=" + _datas[i].Type + "  UID=" + _datas[i].UID);
                if (!Itmes[_datas[i].Type].ContainsKey(_datas[i].UID))
                    Itmes[_datas[i].Type].Add(_datas[i].UID, _datas[i]);
                else
                    Debug.LogWarning("重複裝備UID  Type=" + _datas[i].Type + "  UID=" + _datas[i].UID);
            }
            EquipSaveLocalData();
        }
    }

    static List<EquipData> CurGainEquipDatas;
    public static void Settlement(int _gold, int _emerald, int _freeEmerald, int _curFloor, int _maxFloor, List<EquipData> _equipDatas)
    {
        if (_equipDatas.Count != 0)
            CurGainEquipDatas = _equipDatas;
        else
            CurGainEquipDatas = null;
        Debug.Log("更新Server玩家結算");
        string addEquipStr = "";
        for (int i = 0; i < _equipDatas.Count; i++)
        {
            if (i != 0)
                addEquipStr += "/";
            //Debug.Log("equipProperty="+_equipDatas[i].PropertiesStr);
            addEquipStr += _equipDatas[i].ID + "," + (int)_equipDatas[i].Type + "," + _equipDatas[i].EquipSlot + "," + _equipDatas[i].LV + "," + _equipDatas[i].Quality + "," + _equipDatas[i].PropertiesStr + "," + ((_equipDatas[i].MyEnchant != null) ? _equipDatas[i].MyEnchant.ID : 0) + "," + ID;

        }
        //Debug.Log("gold=" + _gold + " emerald=" + _emerald + " maxFloor=" + _maxFloor + " addEquipStr=" + addEquipStr);
        ServerRequest.Settlement(_gold, _emerald, _freeEmerald, _curFloor, _maxFloor, addEquipStr);
    }
    public static void Settlement_CB(string[] _data)
    {
        //設定玩家資料
        Gold = int.Parse(_data[0]);
        Emerald = int.Parse(_data[1]);
        FreeEmerald = int.Parse(_data[2]);
        CurFloor = int.Parse(_data[3]);
        MaxFloor = int.Parse(_data[4]);
        //設定裝備
        if (CurGainEquipDatas != null && CurGainEquipDatas.Count != 0)
        {
            string[] equipUID = _data[5].Split(',');
            if (equipUID.Length != CurGainEquipDatas.Count)
            {
                Debug.LogWarning("結算server回傳資料錯誤");
                return;
            }
            for (int i = 0; i < CurGainEquipDatas.Count; i++)
            {
                if (!Itmes[CurGainEquipDatas[i].Type].ContainsKey(CurGainEquipDatas[i].UID))
                {
                    Itmes[CurGainEquipDatas[i].Type].Add(CurGainEquipDatas[i].UID, CurGainEquipDatas[i]);
                    CurGainEquipDatas[i].SetUID(int.Parse(equipUID[i]));
                }
                else
                {
                    Debug.LogWarning("重複裝備UID  Type=" + CurGainEquipDatas[i].Type + "  UID=" + CurGainEquipDatas[i].UID);
                }
            }
        }
        //顯示結果在結算
        if (BattleManage.BM)
            BattleManage.BM.StartCoroutine(BattleManage.BM.WaitToShowResult());
    }
}
