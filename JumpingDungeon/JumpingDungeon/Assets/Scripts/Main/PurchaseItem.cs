using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PurchaseItem : Item
{
    [SerializeField]
    Image Icon;
    new protected PurchaseData MyData;
    new protected Purchase ParentUI;
    Toggle TheToggle;

    public override void Set(Data _data, MyUI _ui)
    {
        base.Set(_data, _ui);
        if (_data.GetType() == typeof(PurchaseData))
            MyData = ((PurchaseData)_data);
        else
            return;
        if (_ui.GetType() == typeof(Purchase))
            ParentUI = ((Purchase)_ui);
        Icon.sprite = MyData.GetICON();
        TheToggle = GetComponent<Toggle>();
        TheToggle.group = ParentUI.GetComponent<ToggleGroup>();
    }
    public override void OnPress()
    {
        base.OnPress();
        if (!ParentUI)
            return;
        //TheToggle.isOn = true;
        ParentUI.ShowInfo(MyData);
    }
}
