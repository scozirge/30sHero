using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipPop : MonoBehaviour
{
    [SerializeField]
    GameObject[] EquipObjs;
    [SerializeField]
    Image[] IconSprite;
    [SerializeField]
    Text[] LvText;
    [SerializeField]
    Image[] QualityBottom;
    [SerializeField]
    Text[] Content;
    [SerializeField]
    GameObject ArrowObj;
    [SerializeField]
    Text ExchangeText;
    [SerializeField]
    GameObject LeftSelectObj;
    [SerializeField]
    GameObject RightSelectObj;

    public static EquipCondition Condition;



    public void SetEquipData(EquipData _leftData, EquipData _rightData)
    {
        if (_leftData == null && _rightData != null)
        {
            ArrowObj.SetActive(false);
            RightSelectObj.SetActive(true);
            ExchangeText.text = StringData.GetString("Equip");
            Condition = EquipCondition.Equip;
            EquipObjs[0].SetActive(false);
            IconSprite[1].sprite = _rightData.GetICON();
            QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(_rightData.Quality);
            LvText[1].text = _rightData.GetLVString();
            Content[1].text = "說明";
        }
        else if (_leftData != null && _rightData == null)
        {
            ArrowObj.SetActive(false);
            RightSelectObj.SetActive(false);
            ExchangeText.text = StringData.GetString("TakeOff");
            Condition = EquipCondition.TakeOff;
            EquipObjs[0].SetActive(false);
            IconSprite[1].sprite = _leftData.GetICON();
            QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(_leftData.Quality);
            LvText[1].text = _leftData.GetLVString();
            Content[1].text = "說明";
        }
        else
        {
            RightSelectObj.SetActive(true);
            EquipObjs[0].SetActive(true);
            ArrowObj.SetActive(true);
            ExchangeText.text = StringData.GetString("Exchange");
            Condition = EquipCondition.Exchange;
            IconSprite[0].sprite = _leftData.GetICON();
            QualityBottom[0].sprite = GameManager.GetItemQualityBotSprite(_leftData.Quality);
            LvText[0].text = _leftData.GetLVString();
            Content[0].text = "說明";

            IconSprite[1].sprite = _rightData.GetICON();
            QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(_rightData.Quality);
            LvText[1].text = _rightData.GetLVString();
            Content[1].text = "說明";
        }
    }
    public void UpdateRightEquipData(EquipData _rightData)
    {
        IconSprite[1].sprite = _rightData.GetICON();
        QualityBottom[1].sprite = GameManager.GetItemQualityBotSprite(_rightData.Quality);
        LvText[1].text = _rightData.GetLVString();
        Content[1].text = "說明";
    }
}
