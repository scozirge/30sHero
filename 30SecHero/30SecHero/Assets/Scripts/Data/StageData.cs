using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
public class StageData
{
    public int ID;
    public string Name;
    public int DebutFloor;
    public static string DataName;
    /// <summary>
    /// 將字典傳入，依json表設定資料
    /// </summary>
    public static void SetData(Dictionary<int, StageData> _dic, string _dataName)
    {
        DataName = _dataName;
        string jsonStr = Resources.Load<TextAsset>(string.Format("Json/{0}", DataName)).ToString();
        JsonData jd = JsonMapper.ToObject(jsonStr);
        JsonData items = jd[DataName];
        for (int i = 0; i < items.Count; i++)
        {
            StageData data = new StageData(items[i]);
            int id = int.Parse(items[i]["ID"].ToString());
            _dic.Add(id, data);
        }
    }
    StageData(JsonData _item)
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
    public static List<Stage> GetAvailableStages(int _floor)
    {
        List<Stage> stages = new List<Stage>();
        List<int> keys = new List<int>(GameDictionary.StageDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (GameDictionary.StageDic[keys[i]].DebutFloor == 1 || GameDictionary.StageDic[keys[i]].DebutFloor <= _floor)
            {
                Stage stage = Resources.Load<Stage>(string.Format("Prefabs/Battle/Stages/{0}",GameDictionary.StageDic[keys[i]].Name)) as Stage;
                if (stage == null)
                {
                    Debug.LogWarning(string.Format("找不到StageID:{0} 名稱為{1}的prefab", GameDictionary.StageDic[keys[i]].ID, GameDictionary.StageDic[keys[i]].Name));
                    continue;
                }

                stages.Add(stage);
            }
        }
        return stages;
    }
}
