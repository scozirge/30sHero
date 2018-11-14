using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class EnchantItem : Item
{
    [SerializeField]
    Image Icon;
    [SerializeField]
    MyText LVText;

    public EnchantData MyData;
    Strengthen ParentUI;
    Toggle TheToggle;

    public void Set(EnchantData _data, Strengthen _ui)
    {
        MyData = _data;
        ParentUI = _ui;
        if (Player.EnchantDic.ContainsKey(MyData.ID))
        {
            MyData = Player.EnchantDic[MyData.ID];
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
