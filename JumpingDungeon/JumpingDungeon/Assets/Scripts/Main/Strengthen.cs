using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Strengthen : MyUI
{
    [SerializeField]
    ItemSpawner MySpanwer;
    [SerializeField]
    MyText NameText;
    [SerializeField]
    MyText DescriptionText;
    [SerializeField]
    Button UpgradeButton;
    [SerializeField]
    MyText PriceText;
    [SerializeField]
    Toggle[] TagToggles;

    bool IsInit;
    public static StrengthenType CurFilterType;
    public static StrengthenData CurSelectedData;
    public static StrengthenItem CurSelectedItem;
    static List<StrengthenItem> ItemList = new List<StrengthenItem>();

    void Start()
    {
        if (IsInit)
            return;
        List<int> keys = new List<int>(GameDictionary.StrengthenDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            StrengthenItem si = (StrengthenItem)MySpanwer.Spawn();
            si.Set(GameDictionary.StrengthenDic[keys[i]], this, StrengthenType.Strengthen);
            ItemList.Add(si);
        }
        if (ItemList[0] != null)
        {
            ItemList[0].OnPress();
            ItemList[0].GetComponent<Toggle>().isOn = true;
            MyText.AddRefreshFunc(RefreshText);
        }
        IsInit = true;
    }
    public override void OnEnable()
    {
        if (!IsInit)
            return;
        base.OnEnable();
        if (ItemList[0] != null)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].GetComponent<Toggle>().isOn = false;
            }
            ItemList[0].OnPress();
            ItemList[0].GetComponent<Toggle>().isOn = true;
        }
        if (TagToggles != null)
        {
            for (int i = 0; i < TagToggles.Length; i++)
            {
                TagToggles[i].isOn = false;
            }
            TagToggles[0].isOn = true;
        }
    }
    public void ShowInfo(StrengthenItem _item)
    {
        CurSelectedData = _item.MyData;
        CurSelectedItem = _item;
        RefreshText();
        UpgradeButton.interactable = !(Player.Gold < CurSelectedData.GetPrice());
    }
    public override void RefreshText()
    {
        base.RefreshText();
        NameText.text = CurSelectedData.Name;
        DescriptionText.text = CurSelectedData.Description;
        PriceText.text = CurSelectedData.GetPrice().ToString();
    }
    public void ToFilter(int _typeID)
    {
        StrengthenType type = (StrengthenType)_typeID;
        if (CurFilterType == type)
            return;
        CurFilterType = type;
        Filter();
    }
    void Filter()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].MyType == CurFilterType)
                ItemList[i].gameObject.SetActive(true);
            else
                ItemList[i].gameObject.SetActive(false);
        }
    }
    public void ToStrengthen()
    {
        Player.UpgradeStrengthen(CurSelectedData);
        CurSelectedItem.UpdateUI();
        ShowInfo(CurSelectedItem);
    }
}
