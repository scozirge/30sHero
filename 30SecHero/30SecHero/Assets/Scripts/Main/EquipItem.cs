using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : Item
{
    [SerializeField]
    Image[] Icons;
    [SerializeField]
    Image QualityBottom;
    [SerializeField]
    MyText LVText;
    [SerializeField]
    GameObject SoldCoverObj;
    [SerializeField]
    GameObject SoldCheckObj;
    public EquipType MyType;
    public EquipData MyData;
    Equip ParentUI;
    public bool IsSoldCheck;

    public void Set(EquipData _data, Equip _ui)
    {
        if (_data.GetType() == typeof(WeaponData))
        {
            MyType = EquipType.Weapon;
            MyData = ((WeaponData)_data);
        }
        else if (_data.GetType() == typeof(ArmorData))
        {
            MyType = EquipType.Armor;
            MyData = ((ArmorData)_data);
        }
        else if (_data.GetType() == typeof(AccessoryData))
        {
            MyType = EquipType.Accessory;
            MyData = ((AccessoryData)_data);
        }
        else
            return;
        if (_ui.GetType() == typeof(Equip))
            ParentUI = ((Equip)_ui);
        RefreshText();
        MyText.AddRefreshFunc(RefreshText);
        for (int i = 0; i < Icons.Length;i++ )
        {
            Icons[i].sprite = MyData.Icons[i];
            Icons[i].SetNativeSize();
        }
        QualityBottom.sprite = GameManager.GetItemQualityBotSprite(MyData.Quality);
        SetSoldMode(false);
    }
    public override void RefreshText()
    {
        base.RefreshText();
        LVText.text = MyData.GetLVString();
    }
    public void SetSoldMode(bool _bool)
    {
        SoldCoverObj.SetActive(_bool);
        if (!_bool)
            IsSoldCheck = false;
        SoldCheckObj.SetActive(IsSoldCheck);
    }
    public override void OnPress()
    {
        base.OnPress();
        if (!ParentUI)
            return;
        if (!Equip.SoldMode)
        {
            ParentUI.ToEquip(MyData);
        }
        else
        {
            IsSoldCheck = !IsSoldCheck;
            SoldCheckObj.SetActive(IsSoldCheck);
        }
    }
    public override void Filter(EquipType _type)
    {
        base.Filter(_type);
    }
}
