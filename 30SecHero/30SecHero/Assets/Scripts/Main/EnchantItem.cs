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
        ParentUI = _ui;
        MyData = _data;
        if (MyData != null)
        {
            if (Player.EnchantDic.ContainsKey(MyData.ID))
            {
                MyData = Player.EnchantDic[MyData.ID];
            }
            else
                return;
        }
        UpdateUI();
        MyText.AddRefreshFunc(RefreshText);
        TheToggle = GetComponent<Toggle>();
        TheToggle.group = ParentUI.GetComponent<ToggleGroup>();
    }
    void OnDestroy()
    {
        MyText.RemoveRefreshFunc(RefreshText);
    }
    public override void RefreshText()
    {
        base.RefreshText();
        if (MyData != null)
        {
            LVText.text = MyData.GetLVString(0);
            LVText.enabled = true;
        }
        else
            LVText.enabled = false;
    }
    public void UpdateUI()
    {
        RefreshText();
        if(MyData!=null)
        {
            Icon.sprite = MyData.GetICON();
            Icon.SetNativeSize();
        }
        else
        {
            Icon.sprite = GameManager.GM.UnknownIcon;
            Icon.SetNativeSize();
        }
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
