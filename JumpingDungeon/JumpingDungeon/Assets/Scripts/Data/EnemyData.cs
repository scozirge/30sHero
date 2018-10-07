﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class EnemyData
{
    public int ID;
    public string Name;
    public int DebutFloor;
    public static string DataName;
    public EnemyType Type;
    static List<EnemyRole> Roles;
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, EnemyData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            EnemyData data = new EnemyData(items[i]);
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }
        Roles = GetEnemys();
    }
    EnemyData(JsonData _item)
    {
        try
        {
            JsonData item = _item;
            foreach (string key in item.Keys)
            {
                switch (key)
                {
                    case "ID":
                        ID = int.Parse(item[key].ToString());
                        break;
                    case "Name":
                        Name = item[key].ToString();
                        break;
                    case "DebutFloor":
                        DebutFloor = int.Parse(item[key].ToString());
                        break;
                    case "Type":
                        Type = (EnemyType)Enum.Parse(typeof(EnemyType), item[key].ToString());
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
    static List<EnemyRole> GetEnemys()
    {
        List<EnemyRole> roles = new List<EnemyRole>();
        List<int> keys = new List<int>(GameDictionary.EnemyDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            EnemyRole er = Resources.Load<EnemyRole>(string.Format("Prefabs/Battle/Role/{0}", GameDictionary.EnemyDic[keys[i]].Name));
            if (!er)
            {
                Debug.LogWarning(string.Format("名稱為{0}的怪物Prefab不存在", GameDictionary.EnemyDic[keys[i]].Name));
                continue;
            }
            er.SetEnemyData(GameDictionary.EnemyDic[keys[i]]);
            roles.Add(er);
        }
        return roles;
    }
    public static List<EnemyRole> GetAvailableMillions(int _floor)
    {
        List<EnemyRole> roles = new List<EnemyRole>();
        for (int i = 0; i < Roles.Count; i++)
        {
            if (Roles[i].RelyData.Type != EnemyType.Minion)
                continue;
            if (Roles[i].RelyData.DebutFloor == 1 || Roles[i].RelyData.DebutFloor <= _floor)
            {
                roles.Add(Roles[i]);
            }
        }
        return roles;
    }
    public static List<EnemyRole> GetNextDemogorgon(int _curfloor, out int _nextBossFloor)
    {
        _nextBossFloor = 0;
        List<EnemyRole> roles = new List<EnemyRole>();
        for (int i = 0; i < Roles.Count; i++)
        {
            if (Roles[i].RelyData.Type == EnemyType.Demogorgon)
            {
                if (_curfloor <= Roles[i].RelyData.DebutFloor)
                {
                    if (_nextBossFloor == 0)
                    {
                        _nextBossFloor = Roles[i].RelyData.DebutFloor;
                        roles.Add(Roles[i]);
                    }
                    else
                    {
                        if (Roles[i].RelyData.DebutFloor == _nextBossFloor)
                            roles.Add(Roles[i]);
                    }
                }
            }
        }
        return roles;
    }
}