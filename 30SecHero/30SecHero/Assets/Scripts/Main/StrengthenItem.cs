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
    [SerializeField]
    MyText NumberText;
    [SerializeField]
    Animator MyAni;

    public StrengthenData MyData;
    Strengthen ParentUI;
    Toggle TheToggle;

    public void Set(StrengthenData _data, Strengthen _ui)
    {
        MyData = _data;
        ParentUI = _ui;
        if (Player.StrengthenDic.ContainsKey(MyData.ID))
        {
            MyData = Player.StrengthenDic[MyData.ID];
        }
        else
            return;
        Icon.sprite = MyData.GetICON();
        NumberText.text = MyData.LV.ToString();
        Icon.SetNativeSize();
        MyText.AddRefreshFunc(RefreshText);
        TheToggle = GetComponent<Toggle>();
        TheToggle.group = ParentUI.GetComponent<ToggleGroup>();
    }
    public override void RefreshText()
    {
        base.RefreshText();
        NumberText.text = MyData.LV.ToString();
    }
    void OnDestroy()
    {
        MyText.RemoveRefreshFunc(RefreshText);
    }
    public void UpdateUI()
    {
        Icon.sprite = MyData.GetICON();
        Icon.SetNativeSize();
        PlayAni("LevelUp");
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
    void PlayAni(string _trigger)
    {
        if (MyAni != null)
            MyAni.SetTrigger(_trigger);
    }
    public void ChangeLevelNumber()
    {
        NumberText.text = MyData.LV.ToString();
    }
}
