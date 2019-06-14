using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public partial class ServerRequest : MonoBehaviour
{
    public static bool WaitCB_QuickSignUp { get; private set; }
    //註冊需求重送要求給Server次數
    static byte ReSendQuestTimes_QuickSignUp { get; set; }
    //每次註冊需求最大重送次數，註冊預設是3次，以免太多垃圾帳戶
    const byte MaxReSendQuestTimes_QuickSignUp = 3;
    /// <summary>
    /// 註冊，傳入帳密
    /// </summary>
    public static void QuickSignUp()
    {
        Debug.Log("QuickSignUp");
        ReSendQuestTimes_QuickSignUp = MaxReSendQuestTimes_QuickSignUp;//重置重送要求給Server的次數
        SendSignUpQuest();
    }
    static void SendSignUpQuest()
    {
        if (Conn == null)
            return;
        WWWForm form = new WWWForm();
        //string requestTime = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");//命令時間，格式2015-11-25 15:39:36
        form.AddField("ac_K", Player.Name_K);
        form.AddField("userID_K", Player.UserID_K);
        NewKGAccountSaveLocoDataToDB(ref form);
        WWW w = new WWW(string.Format("{0}{1}", GetServerURL(), "QuickSignUp.php"), form);
        //設定為正等待伺服器回傳
        WaitCB_QuickSignUp = true;
        Conn.StartCoroutine(Coroutine_QuickSignUpCB(w));
        Conn.StartCoroutine(SignUpTimeOutHandle(2f, 0.5f, 12));
    }
    /// <summary>
    /// 註冊回傳
    /// </summary>
    static IEnumerator Coroutine_QuickSignUpCB(WWW w)
    {
        if (ReSendQuestTimes_QuickSignUp == MaxReSendQuestTimes_QuickSignUp)
            if (ShowLoading) CaseTableData.ShowPopLog(1003);//帳號建立中
        yield return w;
        if (ShowCBLog)
            Debug.LogWarning(w.text);
        if (WaitCB_QuickSignUp)
        {
            WaitCB_QuickSignUp = false;
            if (w.error == null)
            {
                try
                {
                    string[] result = w.text.Split(':');
                    //////////////////成功////////////////
                    if (result[0] == ServerCBCode.Success.ToString())
                    {
                        string[] data = result[1].Split(',');
                        Player.SignIn_CB(data);
                        PopupUI.HideLoading();//隱藏Loading
                    }
                    //////////////////失敗///////////////
                    else if (result[0] == ServerCBCode.Fail.ToString())
                    {
                        int caseID = int.Parse(result[1]);
                        if (ShowLoading) CaseTableData.ShowPopLog(caseID);
                        PopupUI.HideLoading();//隱藏Loading
                    }
                    else
                    {
                        if (ShowLoading) CaseTableData.ShowPopLog(6);//錯誤的命令
                        PopupUI.HideLoading();//隱藏Loading
                    }
                }
                //////////////////例外//////////////////
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    if (ShowLoading) CaseTableData.ShowPopLog(6);//錯誤的命令
                    PopupUI.HideLoading();//隱藏Loading
                }
            }
            //////////////////回傳null////////////////
            else
            {
                Debug.LogWarning(w.error);
                if (ShowLoading) CaseTableData.ShowPopLog(2);//連線不到server
                PopupUI.HideLoading();//隱藏Loading
            }
        }
    }
    static IEnumerator SignUpTimeOutHandle(float _firstWaitTime, float _perWaitTime, byte _checkTimes)
    {
        yield return new WaitForSeconds(_firstWaitTime);
        byte checkTimes = _checkTimes;
        //經過_fristWaitTime時間後，每_perWaitTime檢查一次資料是否回傳了，若檢查checkTimes次數後還是沒回傳就重送資料
        while (WaitCB_QuickSignUp && checkTimes > 0)
        {
            checkTimes--;
            yield return new WaitForSeconds(_perWaitTime);
        }
        if (WaitCB_QuickSignUp)//如果還沒接收到CB就重送需求
        {
            //若重送要求的次數達到上限次數則代表連線有嚴重問題，直接報錯
            if (ReSendQuestTimes_QuickSignUp > 0)
            {
                ReSendQuestTimes_QuickSignUp--;
                if (ShowLoading) CaseTableData.ShowPopLog(1002);//連線逾時，嘗試重複連線請玩家稍待
                //向Server重送要求
                SendSignUpQuest();
            }
            else
            {
                WaitCB_QuickSignUp = false;//設定為false代表不接受回傳了
                if (ShowLoading) CaseTableData.ShowPopLog(7); ;//連線逾時，請檢查網路是否正常
                PopupUI.HideLoading();//隱藏Loading
            }
        }
    }
    /// <summary>
    /// 如果server端判斷資料庫中沒此kg帳戶的玩家就新增kg帳戶並把本地資料上傳到對應此kg帳戶的資料中
    /// </summary>
    static void NewKGAccountSaveLocoDataToDB(ref WWWForm form)
    {
        int gold = PlayerPrefs.GetInt(LocoData.Gold.ToString());
        int emerald = PlayerPrefs.GetInt(LocoData.Emerald.ToString());
        int freeEmerald = PlayerPrefs.GetInt(LocoData.FreeEmerald.ToString());
        int payEmerald = PlayerPrefs.GetInt(LocoData.PayEmerald.ToString());
        int curFloor = PlayerPrefs.GetInt(LocoData.CurFloor.ToString());
        int maxFloor = PlayerPrefs.GetInt(LocoData.MaxFloor.ToString());
        int maxEnemyKills = PlayerPrefs.GetInt(LocoData.MaxEnemyKills.ToString());
        string killBossStr = PlayerPrefs.GetString(LocoData.KillBossID.ToString());
        form.AddField("gold", gold);
        form.AddField("emerald", emerald);
        form.AddField("freeEmerald", freeEmerald);
        form.AddField("payEmerald", payEmerald);
        form.AddField("curFloor", curFloor);
        form.AddField("maxFloor", maxFloor);
        form.AddField("killBoss", killBossStr);
        //裝備資料
        string equipStr = PlayerPrefs.GetString(LocoData.Equip.ToString());
        form.AddField("equipStr", equipStr);
        /*
        string equipStr = "";
        if (Player.Items[EquipType.Weapon] != null && Player.Items[EquipType.Weapon].Count != 0)
        {
            List<long> keys = new List<long>(Player.Items[EquipType.Weapon].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (equipStr != "")
                    equipStr += "/";
                equipStr += Player.Items[EquipType.Weapon][keys[i]].ID + "," + (int)Player.Items[EquipType.Weapon][keys[i]].Type + "," + Player.Items[EquipType.Weapon][keys[i]].EquipSlot + "," + Player.Items[EquipType.Weapon][keys[i]].LV + "," + Player.Items[EquipType.Weapon][keys[i]].Quality + "," + Player.Items[EquipType.Weapon][keys[i]].PropertiesStr + "," + ((Player.Items[EquipType.Weapon][keys[i]].MyEnchant != null) ? Player.Items[EquipType.Weapon][keys[i]].MyEnchant.ID : 0);
            }
        }
        if (Player.Items[EquipType.Armor] != null && Player.Items[EquipType.Armor].Count != 0)
        {
            List<long> keys = new List<long>(Player.Items[EquipType.Armor].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (equipStr != "")
                    equipStr += "/";
                equipStr += Player.Items[EquipType.Armor][keys[i]].ID + "," + (int)Player.Items[EquipType.Armor][keys[i]].Type + "," + Player.Items[EquipType.Armor][keys[i]].EquipSlot + "," + Player.Items[EquipType.Armor][keys[i]].LV + "," + Player.Items[EquipType.Armor][keys[i]].Quality + "," + Player.Items[EquipType.Armor][keys[i]].PropertiesStr + "," + ((Player.Items[EquipType.Armor][keys[i]].MyEnchant != null) ? Player.Items[EquipType.Armor][keys[i]].MyEnchant.ID : 0);
            }
        }
        if (Player.Items[EquipType.Accessory] != null && Player.Items[EquipType.Accessory].Count != 0)
        {
            List<long> keys = new List<long>(Player.Items[EquipType.Accessory].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                if (equipStr != "")
                    equipStr += "/";
                equipStr += Player.Items[EquipType.Accessory][keys[i]].ID + "," + (int)Player.Items[EquipType.Accessory][keys[i]].Type + "," + Player.Items[EquipType.Accessory][keys[i]].EquipSlot + "," + Player.Items[EquipType.Accessory][keys[i]].LV + "," + Player.Items[EquipType.Accessory][keys[i]].Quality + "," + Player.Items[EquipType.Accessory][keys[i]].PropertiesStr + "," + ((Player.Items[EquipType.Accessory][keys[i]].MyEnchant != null) ? Player.Items[EquipType.Accessory][keys[i]].MyEnchant.ID : 0);
            }
        }
        */

        //強化資料
        string strengthenStr = PlayerPrefs.GetString(LocoData.Strengthen.ToString());
        form.AddField("strengthenStr", strengthenStr);
        /*
        string strengthenStr = "";
        List<int> skeys = new List<int>(Player.StrengthenDic.Keys);
        for (int i = 0; i < skeys.Count; i++)
        {
            if (Player.StrengthenDic[skeys[i]] == null || Player.StrengthenDic[skeys[i]].LV == 0)
                continue;
            if (strengthenStr != "")
                strengthenStr += "/";
            strengthenStr += Player.StrengthenDic[skeys[i]].ID + "," + Player.StrengthenDic[skeys[i]].LV;
        }
        */
        //附魔資料
        string enchantStr = PlayerPrefs.GetString(LocoData.Enchant.ToString());
        form.AddField("enchantStr", enchantStr);
        /*
        string enchantStr = "";
        List<int> ekeys = new List<int>(Player.EnchantDic.Keys);
        for (int i = 0; i < ekeys.Count; i++)
        {
            if (Player.EnchantDic[ekeys[i]] == null || Player.EnchantDic[ekeys[i]].LV == 0)
                continue;
            if (enchantStr != "")
                enchantStr += "/";
            enchantStr += Player.EnchantDic[ekeys[i]].ID + "," + Player.EnchantDic[ekeys[i]].LV;
        }
        */

    }
}
