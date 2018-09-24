using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    [SerializeField]
    Text GoldText;
    [SerializeField]
    Text EmeraldText;
    [SerializeField]
    Strengthen MyStrengthen;
    [SerializeField]
    Equip MyEquip;
    [SerializeField]
    Purchase MyPurchase;
    [SerializeField]
    Set MySet;
    [SerializeField]
    MainUI CurUI;


    Dictionary<MainUI, MyUI> UIDic = new Dictionary<MainUI, MyUI>();

    // Use this for initialization
    void Start()
    {
        UIDic.Add(MainUI.Strengthen, MyStrengthen);
        UIDic.Add(MainUI.Purchase, MyPurchase);
        UIDic.Add(MainUI.Equip, MyEquip);
        List<MainUI> keys = new List<MainUI>(UIDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] != CurUI)
                UIDic[keys[i]].SetActive(false);
            else
                UIDic[keys[i]].SetActive(true);
        }
    }
    void OnEnable()
    {
        GoldText.text = Player.Gold.ToString();
        EmeraldText.text = Player.Emerald.ToString();
    }
    public void ChangeUI(int _enumID)
    {
        MainUI type = (MainUI)_enumID;
        ChangeUI(type);
    }
    public void ChangeUI(MainUI _ui)
    {
        if (_ui == CurUI)
            return;
        if (!UIDic.ContainsKey(_ui))
            return;
        UIDic[CurUI].SetActive(false);
        UIDic[_ui].SetActive(true);
        CurUI = _ui;
    }
    public void Set(bool _bool)
    {
        MySet.SetActive(_bool);
    }
    public void Battle()
    {

    }

}
