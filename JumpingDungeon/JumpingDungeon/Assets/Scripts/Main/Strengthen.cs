using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Strengthen : MyUI
{
    [SerializeField]
    ItemSpawner MySpanwer;
    [SerializeField]
    Text NameText;
    [SerializeField]
    Text DescriptionText;
    [SerializeField]
    Text PriceText;

    List<Item> ItemList = new List<Item>();

    void Start()
    {
        List<int> keys = new List<int>(GameDictionary.StrengthenDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ItemList.Add(MySpanwer.Spawn(GameDictionary.StrengthenDic[keys[i]], this));
        }
        if (ItemList[0] != null)
        {
            ItemList[0].OnPress();
            ItemList[0].GetComponent<Toggle>().isOn = true;
        }
    }
    public override void ShowInfo(Data _data)
    {
        if (_data.GetType() == typeof(StrengthenData))
        {
            StrengthenData data = (StrengthenData)_data;
            NameText.text = data.Name;
            DescriptionText.text = data.Description;
            PriceText.text = data.GetPrice().ToString();
        }
    }
}
