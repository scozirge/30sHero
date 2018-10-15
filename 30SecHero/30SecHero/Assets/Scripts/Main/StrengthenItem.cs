using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class StrengthenItem : Item
{
    [SerializeField]
    Image Icon;
    [SerializeField]
    MyText LVText;

    public StrengthenData MyData;
    Strengthen ParentUI;
    Toggle TheToggle;
    public StrengthenType MyType;

    public void Set(StrengthenData _data, Strengthen _ui,StrengthenType _type)
    {
        MyData = _data;
        ParentUI = _ui;
        MyType = _type;
        if (Player.StrengthenDic.ContainsKey(MyData.ID))
        {
            MyData = Player.StrengthenDic[MyData.ID];
        }
        else
            return;
        UpdateUI();
        MyText.AddRefreshFunc(RefreshText);
        TheToggle = GetComponent<Toggle>();
        TheToggle.group = ParentUI.GetComponent<ToggleGroup>();
    }
    public override void RefreshText()
    {
        base.RefreshText();
        LVText.text = MyData.GetLVString(0);
    }
    public void UpdateUI()
    {
        RefreshText();
        Icon.sprite = MyData.GetICON();
        Icon.SetNativeSize();
    }
    public override void OnPress()
    {
        if (!TheToggle.isOn)
            return;
        base.OnPress();
        if (!ParentUI)
            return;
        ParentUI.ShowInfo(this);

    }
}
