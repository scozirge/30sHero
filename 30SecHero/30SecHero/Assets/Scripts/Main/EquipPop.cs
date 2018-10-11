using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipPop : MonoBehaviour
{
    [SerializeField]
    GameObject[] EquipObjs;
    [SerializeField]
    Image[] LeftEquipSprite;
    [SerializeField]
    Image[] RightEquipSprite;
    [SerializeField]
    Text[] LvText;
    [SerializeField]
    Image[] QualityBottom;
    [SerializeField]
    GameObject ArrowObj;
    [SerializeField]
    Text ExchangeText;
    [SerializeField]
    GameObject LeftSelectObj;
    [SerializeField]
    GameObject RightSelectObj;
    [SerializeField]
    ItemSpawner LeftSpanwer;
    [SerializeField]
    ItemSpawner RightSpanwer;

    public static EquipCondition Condition;
    List<PropertyItem> LeftPropertyItems = new List<PropertyItem>();
    List<PropertyItem> RightPropertyItems = new List<PropertyItem>();
    static EquipData LeftData;
    static EquipData RightData;

    public void SetPropertyItems(EquipData _data, List<PropertyItem> _itemList, bool _left)
    {
        List<PropertyText> textList;
        if (RightData != null && LeftData != null)
        {
            textList = _data.GetPropertyTextList(LeftData);
        }
        else
        {
            textList = _data.GetPropertyTextList();
        }


        if (textList.Count <= _itemList.Count)
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                if (i < textList.Count)
                {
                    if (textList[i].Comparison == Comparator.Equal)
                    {

                    }
                    _itemList[i].SetString(textList[i]);
                    _itemList[i].gameObject.SetActive(true);
                }
                else
                    _itemList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < textList.Count; i++)
            {
                if (i < _itemList.Count)
                {
                    _itemList[i].SetString(textList[i]);
                    _itemList[i].gameObject.SetActive(true);
                }
                else
                {
                    PropertyItem pi;
                    if (_left)
                        pi = (PropertyItem)LeftSpanwer.Spawn();
                    else
                        pi = (PropertyItem)RightSpanwer.Spawn();
                    pi.SetString(textList[i]);
                    _itemList.Add(pi);
                }
            }
        }
    }



    public void SetEquipData(EquipData _leftData, EquipData _rightData)
    {
        LeftData = _leftData;
        RightData = _rightData;
        if (LeftData == null && RightData != null)
        {
            ArrowObj.SetActive(false);
            RightSelectObj.SetActive(true);
            ExchangeText.text = StringData.GetString("Equip");
            Condition = EquipCondition.Equip;
            EquipObjs[0].SetActive(false);
            for (int i = 0; i < RightEquipSprite.Length; i++)
            {
                RightEquipSprite[i].sprite = RightData.Icons[i];
            }
            QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(RightData.Quality);
            LvText[1].text = RightData.GetLVString();
            SetPropertyItems(RightData, RightPropertyItems, false);
        }
        else if (LeftData != null && RightData == null)
        {
            ArrowObj.SetActive(false);
            RightSelectObj.SetActive(false);
            ExchangeText.text = StringData.GetString("TakeOff");
            Condition = EquipCondition.TakeOff;
            EquipObjs[0].SetActive(false);
            for (int i = 0; i < RightEquipSprite.Length; i++)
            {
                RightEquipSprite[i].sprite = LeftData.Icons[i];
            }
            QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(LeftData.Quality);
            LvText[1].text = LeftData.GetLVString();
            SetPropertyItems(LeftData, RightPropertyItems, false);
        }
        else
        {
            LeftSelectObj.SetActive((_leftData.Type == EquipType.Accessory));
            RightSelectObj.SetActive(true);
            EquipObjs[0].SetActive(true);
            ArrowObj.SetActive(true);
            ExchangeText.text = StringData.GetString("Exchange");
            Condition = EquipCondition.Exchange;
            for (int i = 0; i < RightEquipSprite.Length; i++)
            {
                LeftEquipSprite[i].sprite = LeftData.Icons[i];
            }
            QualityBottom[0].sprite = GameManager.GetItemQualityBotSprite(LeftData.Quality);
            LvText[0].text = LeftData.GetLVString();
            SetPropertyItems(LeftData, LeftPropertyItems, true);

            for (int i = 0; i < RightEquipSprite.Length; i++)
            {
                RightEquipSprite[i].sprite = RightData.Icons[i];
            }
            QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(RightData.Quality);
            LvText[1].text = RightData.GetLVString();
            SetPropertyItems(RightData, RightPropertyItems, false);
        }
        for (int j = 0; j < LeftEquipSprite.Length; j++)
        {
            LeftEquipSprite[j].SetNativeSize();
        }
        for (int j = 0; j < RightEquipSprite.Length; j++)
        {
            RightEquipSprite[j].SetNativeSize();
        }
    }
    public void UpdateRightEquipData(EquipData _rightData)
    {
        RightData = _rightData;
        for (int i = 0; i < RightEquipSprite.Length; i++)
        {
            RightEquipSprite[i].sprite = _rightData.Icons[i];
            RightEquipSprite[i].SetNativeSize();
        }
        QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(_rightData.Quality);
        LvText[1].text = _rightData.GetLVString();
        SetPropertyItems(RightData, RightPropertyItems, false);

    }
    public void UpdateLeftEquipData(EquipData _leftData)
    {
        LeftData = _leftData;
        for (int i = 0; i < RightEquipSprite.Length; i++)
        {
            LeftEquipSprite[i].sprite = _leftData.Icons[i];
            LeftEquipSprite[i].SetNativeSize();
        }
        QualityBottom[0].sprite = GameManager.GetItemQualityBotSprite(_leftData.Quality);
        LvText[0].text = _leftData.GetLVString();
        SetPropertyItems(LeftData, LeftPropertyItems, true);
        SetPropertyItems(RightData, RightPropertyItems, false);
    }
}
