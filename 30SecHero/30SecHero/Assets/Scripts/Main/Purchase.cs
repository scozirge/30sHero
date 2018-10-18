using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purchase : MyUI
{

    [SerializeField]
    ItemSpawner MySpanwer;
    [SerializeField]
    MyText NameText;
    [SerializeField]
    MyText DescriptionText;
    [SerializeField]
    MyText PriceText;
    [SerializeField]
    Image Icon;
    [SerializeField]
    Text CountText;
    [SerializeField]
    GameObject RightPanel;

    List<PurchaseItem> ItemList = new List<PurchaseItem>();

    public static Purchase MySelf;
    public static PurchaseData CurSelectedData;
    public static PurchaseItem CurSelectedItem;

    public override void OnEnable()
    {
        base.OnEnable();
        if (!MySelf)
            MySelf = this;
        for (int i = 0; i < ItemList.Count; i++)
        {
            Destroy(ItemList[i].gameObject);
        }
        ItemList = new List<PurchaseItem>();
        ShowInfo(null);
        KongregateAPIBehaviour.ShowItemList();
        //ShowItemListCB("1,test,test,1/2,test2,test2,5");
    }
    public void ShowItemListCB(string _datas)
    {
        Debug.Log("////////////////OnItemListCB-" + _datas);
        string[] items = _datas.Split('/');
        for (int i = 0; i < items.Length; i++)
        {
            string[] itemData = items[i].Split(',');
            int id = int.Parse(itemData[0]);
            int price = int.Parse(itemData[3]);
            //Debug.Log(string.Format("iden={0},namte={1},desc={2},price={3}", itemData[0], itemData[1], itemData[2], itemData[3]));
            if (GameDictionary.PurchaseDic.ContainsKey(id))
            {
                GameDictionary.PurchaseDic[id].SetPurchasePrice(price);
                PurchaseItem pi = (PurchaseItem)MySpanwer.Spawn();
                pi.Set(GameDictionary.PurchaseDic[id], this);
                ItemList.Add(pi);
            }
        }
        if (ItemList[0] != null)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].GetComponent<Toggle>().isOn = false;
            }
            ItemList[0].OnPress();//OnPress會呼叫ShowInfo
            ItemList[0].GetComponent<Toggle>().isOn = true;
        }
        else
            ShowInfo(null);
    }
    public void ShowInfo(PurchaseItem _item)
    {
        if (_item)
        {
            CurSelectedData = _item.MyData;
            CurSelectedItem = _item;
            RefreshText();
        }
        RightPanel.SetActive(_item);
    }
    public override void RefreshText()
    {
        base.RefreshText();
        NameText.text = CurSelectedData.Name;
        DescriptionText.text = CurSelectedData.Description;
        PriceText.text = CurSelectedData.Price.ToString();
        Icon.sprite = CurSelectedData.GetICON();
        CountText.text = CurSelectedData.EmeraldCount.ToString();
    }
    static PurchaseData CurPurchaseData;
    public void ToPurchase()
    {
        CurPurchaseData = CurSelectedData;
        KongregateAPIBehaviour.PurchaseItem(CurPurchaseData.ID);
    }
    public static void ToPurchaseCB(bool _result)
    {
        if (_result)
        {
            Player.GainEmerald(CurPurchaseData.EmeraldCount);
            PopupUI.ShowClickCancel(string.Format(StringData.GetString("PurchaseEmeraldSuccess"), CurPurchaseData.EmeraldCount));
            ServerRequest.PurchaseEmerald(Player.Emerald);
        }
        else
        {
            PopupUI.ShowClickCancel("PurchaseEmeraldFail");
        }
    }
}
