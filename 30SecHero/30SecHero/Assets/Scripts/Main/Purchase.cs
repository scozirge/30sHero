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
    Image BtnIcon;
    [SerializeField]
    Text ItemCountText;
    [SerializeField]
    GameObject RightPanel;
    [SerializeField]
    Button UpgradeButton;

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
        ShowItemList();
        KongregateAPIBehaviour.ShowItemList();
        //ShowItemListCB("1,test,test,1/2,test2,test2,5");
    }
    void ShowItemList()
    {
        List<int> keys = new List<int>(GameDictionary.PurchaseDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            PurchaseItem pi = (PurchaseItem)MySpanwer.Spawn();
            pi.Set(GameDictionary.PurchaseDic[keys[i]], this);
            ItemList.Add(pi);
        }
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
            for (int j = 0; j < ItemList.Count; j++)
            {
                if (ItemList[j].MyData.ID == id)
                {
                    ItemList[j].MyData.SetPurchasePrice(price);
                    break;
                }
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
        Icon.sprite = CurSelectedData.GetICON();
        Icon.SetNativeSize();
        ItemCountText.text = CurSelectedData.Gain.ToString();
        if (CurSelectedData.MyType == PurchaseType.TradeGold)
            ItemCountText.color = GameManager.GM.GoldColor;
        else
        {
            ItemCountText.color = GameManager.GM.EmeraldColor;
        }
        UpgradeButton.interactable = true;
        switch (CurSelectedData.MyType)
        {
            case PurchaseType.BuyEmerald:
                BtnIcon.sprite = GameManager.GetCurrencySprite(Currency.Kred);
                PriceText.text = CurSelectedData.PayKreds.ToString();
                break;
            case PurchaseType.TradeGold:
                if (Player.Emerald < CurSelectedData.PayEmerald)
                {
                    UpgradeButton.interactable = false;
                }
                BtnIcon.sprite = GameManager.GetCurrencySprite(Currency.Emerald);
                PriceText.text = CurSelectedData.PayEmerald.ToString();
                break;
            case PurchaseType.WatchingAD:
                BtnIcon.sprite = GameManager.GetCurrencySprite(Currency.AD);
                break;
        }
    }
    static PurchaseData CurPurchaseData;
    public void ToPurchase()
    {
        CurPurchaseData = CurSelectedData;
        switch (CurPurchaseData.MyType)
        {
            case PurchaseType.BuyEmerald:
                KongregateAPIBehaviour.PurchaseItem(CurPurchaseData.ID);
                break;
            case PurchaseType.TradeGold:
                Player.TradeEmeraldForGold(CurSelectedData.PayEmerald, CurSelectedData.Gain);
                break;
            case PurchaseType.WatchingAD:
                break;
        }
    }
    public static void ToPurchaseCB(bool _result)
    {
        if (_result)
        {
            Player.GainEmerald(CurPurchaseData.Gain);
            PopupUI.ShowClickCancel(string.Format(StringData.GetString("PurchaseEmeraldSuccess"), CurPurchaseData.Gain));
            ServerRequest.PurchaseEmerald(Player.Emerald);
        }
        else
        {
            PopupUI.ShowClickCancel("PurchaseEmeraldFail");
        }
    }
}
