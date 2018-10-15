using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class CaseTableData
{
    public int CaseID { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool SendGALog { get; private set; }
    public PopType MyPopType { get; private set; }
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, CaseTableData> _dic)
    {
        string jsonStr = Resources.Load<TextAsset>("Json/CaseTable").ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData CaseTableItems = jd["CaseTable"];
        for (int i = 0; i < CaseTableItems.Count; i++)
        {
            CaseTableData caseTableData = new CaseTableData(CaseTableItems[i]);
            int id = caseTableData.CaseID;
            _dic.Add(id, caseTableData);
        }
    }
    CaseTableData(JsonData _item)
    {
        try
        {
            JsonData item = _item;
            CaseID = int.Parse(item["CaseID"].ToString());
            MyPopType = (PopType)Enum.Parse(typeof(PopType), item["PopType"].ToString());
            SendGALog = bool.Parse(item["SendGALog"].ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public static void ShowPopLog(int _caseID)
    {
        if (!GameDictionary.CaseTableDic.ContainsKey(_caseID))
            return;
        CaseTableData ct = GameDictionary.CaseTableDic[_caseID];
        switch (ct.MyPopType)
        {
            case PopType.Loading:
                PopupUI.ShowLoading(GameDictionary.String_CaseTableDic[ct.CaseID.ToString()].GetString(1, Player.UseLanguage));
                break;
            case PopType.ClickCancel:
                PopupUI.ShowClickCancel(GameDictionary.String_CaseTableDic[ct.CaseID.ToString()].GetString(1, Player.UseLanguage));
                break;
            case PopType.LoadingTitle:
                PopupUI.ShowTitleLoading(GameDictionary.String_CaseTableDic[ct.CaseID.ToString()].GetString(0, Player.UseLanguage), GameDictionary.String_CaseTableDic[ct.CaseID.ToString()].GetString(1, Player.UseLanguage));
                break;
            case PopType.ClickCancelTitle:
                PopupUI.ShowTitleClickCancel(GameDictionary.String_CaseTableDic[ct.CaseID.ToString()].GetString(0, Player.UseLanguage), GameDictionary.String_CaseTableDic[ct.CaseID.ToString()].GetString(1, Player.UseLanguage));
                break;
        }
    }
    public static void HidePopLog(int _caseID)
    {
        if (!GameDictionary.CaseTableDic.ContainsKey(_caseID))
            return;
        CaseTableData ct = GameDictionary.CaseTableDic[_caseID];
        switch (ct.MyPopType)
        {
            case PopType.Loading:
                PopupUI.HideLoading();
                break;
            case PopType.ClickCancel:
                PopupUI.HidewClickCancel();
                break;
            case PopType.LoadingTitle:
                PopupUI.HidewTitleLoading();
                break;
            case PopType.ClickCancelTitle:
                PopupUI.HideClickCancelTitle();
                break;
        }

    }
}