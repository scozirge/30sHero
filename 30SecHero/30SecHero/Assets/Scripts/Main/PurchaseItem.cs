using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PurchaseItem : Item
{
    [SerializeField]
    Image Icon;
    [SerializeField]
    Text CountText;
    public PurchaseData MyData;
    Purchase ParentUI;
    Toggle TheToggle;

    public void Set(PurchaseData _data, Purchase _ui)
    {
        MyData = _data;
        ParentUI = _ui;
        CountText.text = MyData.Gain.ToString();
        Icon.sprite = MyData.GetICON();
        TheToggle = GetComponent<Toggle>();
        TheToggle.group = ParentUI.GetComponent<ToggleGroup>();
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
