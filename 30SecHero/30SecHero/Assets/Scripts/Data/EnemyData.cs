using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class EnemyData
{
    public int ID;
    public string Name;
    public int DebutFloor;
    public EnemyType Type;
    public static string DataName;

    static List<EnemyRole> Roles;
    static Dictionary<int, int> DemogorgonDic = new Dictionary<int, int>();
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
            if (data.Type == EnemyType.Demogorgon)
                DemogorgonDic.Add(data.DebutFloor, data.ID);
            _dic.Add(id, data);
        }
        Roles = GetEnemys();
        DemogorgonDic = MySort.GetSortDicByKey(DemogorgonDic);
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
            er = er.GetMemberwiseClone();
            er.SetEnemyData(GameDictionary.EnemyDic[keys[i]]);
            roles.Add(er);
        }
        return roles;
    }
    static EnemyRole GetEnemy(int _id)
    {
        for(int i=0;i<Roles.Count;i++)
        {
            if (Roles[i].ID == _id)
                return Roles[i];
        }
        return null;
    }
    public static List<EnemyRole> GetAvailableMillions(int _floor)
    {
        List<EnemyRole> roles = new List<EnemyRole>();
        for (int i = 0; i < Roles.Count; i++)
        {
            if (Roles[i].Type != EnemyType.Minion)
                continue;
            if (Roles[i].DebutFloor == 1 || Roles[i].DebutFloor <= _floor)
            {
                roles.Add(Roles[i]);
            }
        }
        return roles;
    }
    public static EnemyRole GetNextDemogorgon(int _curfloor, out int _nextBossFloor)
    {
        _nextBossFloor = 0;
        foreach(int key in DemogorgonDic.Keys)
        {
            if (key >= _curfloor)
            {
                _nextBossFloor = key;
                return GetEnemy(DemogorgonDic[key]);
            }
        }
        return null;
    }
    public static EnemyRole GetPreviousDemogorgon(int _curfloor, out int _previousBossFloor)
    {
        _previousBossFloor = 0;
        Dictionary<int, int> reverseDemogorgonDic = MySort.GetReverseDic(DemogorgonDic);
        foreach (int key in reverseDemogorgonDic.Keys)
        {
            if (key < _curfloor)
            {
                _previousBossFloor = key;
                return GetEnemy(DemogorgonDic[key]);
            }
        }
        return null;
    }
}
