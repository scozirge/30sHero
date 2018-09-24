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
    Text LVText;

    new protected StrengthenData MyData;
    new protected Strengthen ParentUI;
    Toggle TheToggle;


    public override void Set(Data _data, MyUI _ui)
    {
        base.Set(_data, _ui);
        if (_data.GetType() == typeof(StrengthenData))
            MyData = ((StrengthenData)_data);
        else
            return;
        if (_ui.GetType() == typeof(Strengthen))
            ParentUI = ((Strengthen)_ui);
        LVText.text = MyData.GetLVString();
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
