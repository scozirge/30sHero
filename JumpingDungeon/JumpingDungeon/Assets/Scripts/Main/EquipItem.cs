using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : Item
{
    [SerializeField]
    Image Icon;
    [SerializeField]
    Image QualityBottom;
    [SerializeField]
    Text LVText;
    [SerializeField]
    GameObject SoldCoverObj;
    [SerializeField]
    GameObject SoldCheckObj;
    public EquipType MyType;
    new public EquipData MyData;
    new protected Equip ParentUI;
    public bool IsSoldCheck;

    public override void Set(Data _data, MyUI _ui)
    {
        base.Set(_data, _ui);
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

        LVText.text = MyData.GetLVString();
        Icon.sprite = MyData.GetICON();
        QualityBottom.sprite = GameManager.GetItemQualityBotSprite(MyData.Quality);
        SetSoldMode(false);
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
            ParentUI.ShowInfo(MyData);
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
