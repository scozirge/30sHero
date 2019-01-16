using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class StrengthenItem : Item
{
    [SerializeField]
    GameObject TipObj;
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
        //NumberText.text = MyData.LV.ToString();
        Icon.SetNativeSize();
        MyText.AddRefreshFunc(RefreshText);
        TheToggle = GetComponent<Toggle>();
        TheToggle.group = ParentUI.GetComponent<ToggleGroup>();
        UpdateUI();
    }
    public void SetTip(bool _show)
    {
        TipObj.SetActive(_show);
    }
    public override void RefreshText()
    {
        base.RefreshText();
        LVText.text = MyData.GetLVString(0);
        //NumberText.text = MyData.LV.ToString();
    }
    void OnDestroy()
    {
        MyText.RemoveRefreshFunc(RefreshText);
    }
    public void UpdateUI()
    {
        RefreshText();
        Icon.sprite = MyData.GetICON();
        Icon.SetNativeSize();
        //PlayAni("LevelUp");
    }
    public override void OnPress()
    {
        if (!TheToggle.isOn)
            return;
        base.OnPress();
        if (!ParentUI)
            return;
        ParentUI.ShowInfo(this);
        if(this!=Strengthen.StrengthenItemList[0])
            ParentUI.MainPanel.SetTip(TipType.StrengthenTagTip, false);
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
