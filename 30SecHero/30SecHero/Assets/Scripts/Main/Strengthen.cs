using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Strengthen : MyUI
{
    [SerializeField]
    public Main MainPanel;
    [SerializeField]
    ItemSpawner StrengthenSpanwer;
    [SerializeField]
    ItemSpawner EnchantSpanwer;
    [SerializeField]
    MyText NameText;
    [SerializeField]
    MyText DescriptionText;
    [SerializeField]
    Button UpgradeButton;
    [SerializeField]
    MyText PriceText;
    [SerializeField]
    Image PriceImage;
    [SerializeField]
    Toggle[] TagToggles;
    [SerializeField]
    ScrollRect MyScrollRect;
    [SerializeField]
    RectTransform StrengthenContent;
    [SerializeField]
    RectTransform EnchantContent;

    bool IsInit;
    public static StrengthenType CurFilterType;
    public static StrengthenData CurSelectedSData;
    public static StrengthenItem CurSelectedSItem;
    public static EnchantData CurSelectedEData;
    public static EnchantItem CurSelectedEItem;
    public static List<StrengthenItem> StrengthenItemList = new List<StrengthenItem>();
    static List<EnchantItem> EnchantItemList = new List<EnchantItem>();

    void Start()
    {
        if (IsInit)
            return;
        StrengthenItemList = new List<StrengthenItem>();
        EnchantItemList = new List<EnchantItem>();
        //Strengthen
        List<int> keys = new List<int>(GameDictionary.StrengthenDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            StrengthenItem si = (StrengthenItem)StrengthenSpanwer.Spawn();
            si.Set(GameDictionary.StrengthenDic[keys[i]], this);
            StrengthenItemList.Add(si);
        }
        //Enchant
        keys = new List<int>(Player.EnchantDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (Player.EnchantDic[keys[i]].MyEnchantType == EnchantType.Enchant)
            {
                if(Player.EnchantDic[keys[i]].LV>0)
                {
                    EnchantItem ei = (EnchantItem)EnchantSpanwer.Spawn();
                    ei.Set(Player.EnchantDic[keys[i]], this);
                    EnchantItemList.Add(ei);
                }
            }
        }
        if(!EnchantData.CheckGetAllEnchant())//如果所以附魔都解索完就不跳未解鎖問號標誌
        {
            EnchantItem unknown = (EnchantItem)EnchantSpanwer.Spawn();
            unknown.Set(null, this);
            EnchantItemList.Add(unknown);
        }

        //預設強化分頁並選擇第一個item
        ToFilter((int)StrengthenType.Strengthen);
        StrengthenContent.gameObject.SetActive(true);
        EnchantContent.gameObject.SetActive(false);
        MyScrollRect.content = StrengthenContent;
        if (StrengthenItemList[0] != null)
        {
            StrengthenItemList[0].OnPress();
            StrengthenItemList[0].GetComponent<Toggle>().isOn = true;
            MyText.AddRefreshFunc(RefreshText);
        }
        IsInit = true;
    }
    void OnDestroy()
    {
        MyText.RemoveRefreshFunc(RefreshText);
    }
    public override void OnEnable()
    {
        if (!IsInit)
            return;
        base.OnEnable();
        ToFilter((int)StrengthenType.Strengthen);
    }
    public void ShowInfo(StrengthenItem _item)
    {
        CurSelectedSData = _item.MyData;
        CurSelectedSItem = _item;
        RefreshText();
        UpgradeButton.gameObject.SetActive(true);
        if (Player.Gold < CurSelectedSData.GetPrice() || !CurSelectedSData.CanUpgrade())
        {
            UpgradeButton.interactable = false;
        }
        else
            UpgradeButton.interactable = true;
    }
    public void ShowInfo(EnchantItem _item)
    {
        if(_item.MyData!=null)
        {
            CurSelectedEData = _item.MyData;
            CurSelectedEItem = _item;
            RefreshText();
            UpgradeButton.gameObject.SetActive(true);
            if (Player.Emerald < CurSelectedEData.GetPrice() || !CurSelectedEData.CanUpgrade())
            {
                UpgradeButton.interactable = false;
            }
            else
                UpgradeButton.interactable = true;
        }
        else
        {
            CurSelectedEData = null;
            CurSelectedEItem = null;
            RefreshText();
            UpgradeButton.gameObject.SetActive(false);
        }
    }
    public override void RefreshText()
    {
        base.RefreshText();
        if (CurFilterType == StrengthenType.Strengthen)
        {
            NameText.text = CurSelectedSData.Name;
            if (!CurSelectedSData.CanUpgrade())
            {
                DescriptionText.text = CurSelectedSData.Description(-1);
                PriceText.text = StringData.GetString("MaxLevel");

            }
            else if(Player.Gold < CurSelectedSData.GetPrice())
            {
                DescriptionText.text = CurSelectedSData.Description(0);
                PriceText.text = CurSelectedSData.GetPrice().ToString(); //PriceText.text = StringData.GetString("Unaffordable");
            }
            else
            {
                DescriptionText.text = CurSelectedSData.Description(0);
                PriceText.text = CurSelectedSData.GetPrice().ToString();
            }
        }
        else if (CurFilterType == StrengthenType.Enchant)
        {
            if (CurSelectedEData!=null)
            {
                NameText.text = CurSelectedEData.Name;
                if (!CurSelectedEData.CanUpgrade())
                {
                    DescriptionText.text = CurSelectedEData.Description(-1);
                    PriceText.text = StringData.GetString("MaxLevel");

                }
                else if(Player.Emerald < CurSelectedEData.GetPrice())
                {
                    DescriptionText.text = CurSelectedEData.Description(0);
                    PriceText.text = CurSelectedEData.GetPrice().ToString(); //PriceText.text = StringData.GetString("Unaffordable");
                }
                else
                {
                    DescriptionText.text = CurSelectedEData.Description(0);
                    PriceText.text = CurSelectedEData.GetPrice().ToString();
                }
            }
            else
            {
                NameText.text = StringData.GetString("UnknownEnchant");
                DescriptionText.text = StringData.GetString("UnknownEnchantInfo");
            }
        }
    }
    public void ToFilter(int _typeID)
    {
        if ((int)CurFilterType == _typeID)
            return;
        StrengthenType type = (StrengthenType)_typeID;
        CurFilterType = type;
        Filter();
    }
    void Filter()
    {
        switch (CurFilterType)
        {
            case StrengthenType.Strengthen:
                MainPanel.SetTip(TipType.StrengthenTagTip, false);
                PriceImage.sprite = GameManager.GetCurrencySprite(Currency.Gold);
                StrengthenContent.gameObject.SetActive(true);
                EnchantContent.gameObject.SetActive(false);
                MyScrollRect.content = StrengthenContent;
                //預設強化分頁並選擇第一個item
                if (StrengthenItemList[0] != null)
                {
                    StrengthenItemList[0].OnPress();
                    StrengthenItemList[0].GetComponent<Toggle>().isOn = true;
                    MyText.AddRefreshFunc(RefreshText);
                }
                break;
            case StrengthenType.Enchant:
                MainPanel.SetTip(TipType.EnchantTagTip, false);
                PriceImage.sprite = GameManager.GetCurrencySprite(Currency.Emerald);
                StrengthenContent.gameObject.SetActive(false);
                EnchantContent.gameObject.SetActive(true);
                MyScrollRect.content = EnchantContent;
                //預設選擇第一個item
                if (EnchantItemList[0] != null)
                {
                    EnchantItemList[0].OnPress();
                    EnchantItemList[0].GetComponent<Toggle>().isOn = true;
                    MyText.AddRefreshFunc(RefreshText);
                }
                break;
        }
    }
    public void ToStrengthen()
    {

        if (CurFilterType == StrengthenType.Strengthen)
        {
            MainPanel.SetTip(TipType.StrengthenTagTip, false);
            Player.StrengthenUpgrade(CurSelectedSData);
            CurSelectedSItem.UpdateUI();
            ShowInfo(CurSelectedSItem);
        }
        else if (CurFilterType == StrengthenType.Enchant)
        {
            Player.EnchantUpgrade(CurSelectedEData);
            CurSelectedEItem.UpdateUI();
            ShowInfo(CurSelectedEItem);
        }

    }
}
