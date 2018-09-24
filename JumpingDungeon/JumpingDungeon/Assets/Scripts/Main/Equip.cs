using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MyUI
{
    [SerializeField]
    ItemSpawner MySpanwer;
    [SerializeField]
    EquipPop EquipPop;
    [SerializeField]
    EquipData SelectedEquip;
    [SerializeField]
    Image WeaponIcon;
    [SerializeField]
    Image WeaponQuality;
    [SerializeField]
    Image ArmorIcon;
    [SerializeField]
    Image ArmorQuality;
    [SerializeField]
    Image[] AccessoryIcon;
    [SerializeField]
    Image[] AccessoryQuality;
    [SerializeField]
    Text StrengthText;
    [SerializeField]
    Text HealthText;
    [SerializeField]
    Text ShieldText;
    [SerializeField]
    Text ShieldRecoveryText;
    [SerializeField]
    Text MoveSpeedText;
    [SerializeField]
    Text MaxMoveText;
    [SerializeField]
    Text AvatarTimeText;
    [SerializeField]
    Text SkillTimeText;




    public EquipType CurFilterType;
    EquipType TakeOffType;
    List<EquipItem> ItemList = new List<EquipItem>();
    Dictionary<EquipType, List<EquipItem>> EquipDic = new Dictionary<EquipType, List<EquipItem>>();
    List<EquipItem> WeaponList = new List<EquipItem>();
    List<EquipItem> ArmorList = new List<EquipItem>();
    List<EquipItem> AccessoryList = new List<EquipItem>();
    static int CurItemIndex;

    void Start()
    {
        if (Player.Itmes.ContainsKey(EquipType.Weapon))
        {
            List<long> keys = new List<long>(Player.Itmes[EquipType.Weapon].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EquipItem ei = (EquipItem)MySpanwer.Spawn(Player.Itmes[EquipType.Weapon][keys[i]], this);
                ItemList.Add(ei);
                if (!ei.MyData.IsEquiped)
                    WeaponList.Add(ei);
            }
            EquipDic.Add(EquipType.Weapon, WeaponList);
        }
        if (Player.Itmes.ContainsKey(EquipType.Armor))
        {
            List<long> keys = new List<long>(Player.Itmes[EquipType.Armor].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EquipItem ei = (EquipItem)MySpanwer.Spawn(Player.Itmes[EquipType.Armor][keys[i]], this);
                ItemList.Add(ei);
                if (!ei.MyData.IsEquiped)
                    ArmorList.Add(ei);
            }
            EquipDic.Add(EquipType.Armor, ArmorList);
        }
        if (Player.Itmes.ContainsKey(EquipType.Accessory))
        {
            List<long> keys = new List<long>(Player.Itmes[EquipType.Accessory].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EquipItem ei = (EquipItem)MySpanwer.Spawn(Player.Itmes[EquipType.Accessory][keys[i]], this);
                ItemList.Add(ei);
                if (!ei.MyData.IsEquiped)
                    AccessoryList.Add(ei);
            }
            EquipDic.Add(EquipType.Accessory, AccessoryList);
        }

        Filter();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        UpdateRoleInfo();
    }
    public override void ShowInfo(Data _data)
    {
        base.ShowInfo(_data);
    }
    public void ToFilter(int _typeID)
    {
        EquipType type = (EquipType)_typeID;
        if (CurFilterType == type)
            return;
        CurFilterType = type;
        Filter();
    }
    void Filter()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (!ItemList[i].MyData.IsEquiped)
                if (ItemList[i].MyType == CurFilterType)
                    ItemList[i].gameObject.SetActive(true);
                else
                    ItemList[i].gameObject.SetActive(false);
            else
                ItemList[i].gameObject.SetActive(false);
        }
    }
    public void ToEquip(EquipData _data)
    {
        SelectedEquip = _data;
        EquipPop.gameObject.SetActive(true);
        switch (CurFilterType)
        {
            case EquipType.Weapon:
                EquipPop.SetEquipData(Player.MyWeapon, SelectedEquip);
                break;
            case EquipType.Armor:
                EquipPop.SetEquipData(Player.MyArmor, SelectedEquip);
                break;
            case EquipType.Accessory:
                if (Player.MyAccessorys.Length > 0)
                    EquipPop.SetEquipData(Player.MyAccessorys[0], SelectedEquip);
                break;
        }
        CurItemIndex = GetIndex(_data.UID);
    }
    int GetIndex(long _equipUID)
    {
        int index = 0;
        for (int i = 0; i < EquipDic[CurFilterType].Count; i++)
        {
            if (_equipUID == EquipDic[CurFilterType][i].MyData.UID)
                return i;
        }
        Debug.LogWarning(string.Format("Don't find equipUID from list. Type:{0}", CurFilterType));
        return index;
    }
    int GetIndexFromTotalItemList(long _equipUID)
    {
        int index = 0;
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (_equipUID == ItemList[i].MyData.UID)
                return i;
        }
        Debug.LogWarning("Don't find equipUID from ItemList");
        return index;
    }
    public void NextItem()
    {
        CurItemIndex++;
        if (CurItemIndex >= EquipDic[CurFilterType].Count)
            CurItemIndex = 0;
        SelectedEquip = EquipDic[CurFilterType][CurItemIndex].MyData;
        EquipPop.UpdateRightEquipData(SelectedEquip);
    }
    public void PreviousItem()
    {
        CurItemIndex--;
        if (CurItemIndex < 0)
            CurItemIndex = EquipDic[CurFilterType].Count - 1;
        SelectedEquip = EquipDic[CurFilterType][CurItemIndex].MyData;
        EquipPop.UpdateRightEquipData(SelectedEquip);

    }

    public void ToTakeOff(int _equipTypeID)
    {
        TakeOffType = (EquipType)_equipTypeID;
        switch (TakeOffType)
        {
            case EquipType.Weapon:
                if (Player.MyWeapon == null)
                    return;
                EquipPop.SetEquipData(Player.MyWeapon, null);
                break;
            case EquipType.Armor:
                if (Player.MyArmor == null)
                    return;
                EquipPop.SetEquipData(Player.MyArmor, null);
                break;
        }
        EquipPop.gameObject.SetActive(true);
    }
    public void ToTakeOffAccessory(int _index)
    {
        if (_index > 1 || _index < 0)
            _index = 0;
        TakeOffType = EquipType.Accessory;
        if (Player.MyAccessorys.Length > 0)
        {
            if (Player.MyAccessorys[_index] == null)
                return;
            EquipPop.SetEquipData(Player.MyAccessorys[_index], null);
            EquipPop.gameObject.SetActive(true);
        }
    }
    public void CancelEquip()
    {
        EquipPop.gameObject.SetActive(false);
    }
    public void ExecuteEquip()
    {
        EquipPop.gameObject.SetActive(false);
        if (EquipPop.Condition == EquipCondition.Equip || EquipPop.Condition == EquipCondition.Exchange)
        {
            switch (SelectedEquip.Type)
            {
                case EquipType.Weapon:
                    if (Player.MyWeapon != null)
                    {
                        int index = GetIndexFromTotalItemList(Player.MyWeapon.UID);
                        EquipDic[CurFilterType].Add(ItemList[index]);
                    }
                    Player.Equip((WeaponData)SelectedEquip);
                    break;
                case EquipType.Armor:
                    if (Player.MyArmor != null)
                    {
                        int index = GetIndexFromTotalItemList(Player.MyArmor.UID);
                        EquipDic[CurFilterType].Add(ItemList[index]);
                    }
                    Player.Equip((ArmorData)SelectedEquip);
                    break;
                case EquipType.Accessory:
                    if (Player.MyAccessorys.Length > 0)
                    {
                        if (Player.MyAccessorys[0] != null)
                        {
                            int index = GetIndexFromTotalItemList(Player.MyAccessorys[0].UID);
                            EquipDic[CurFilterType].Add(ItemList[index]);
                        }
                        Player.Equip((AccessoryData)SelectedEquip, 0);
                    }
                    break;
            }
            EquipDic[CurFilterType].RemoveAt(CurItemIndex);
        }
        else
        {
            int itemIndex = 0;
            switch (TakeOffType)
            {
                case EquipType.Weapon:
                    itemIndex = GetIndexFromTotalItemList(Player.MyWeapon.UID);
                    EquipDic[TakeOffType].Add(ItemList[itemIndex]);
                    Player.TakeOff(Player.MyWeapon);
                    break;
                case EquipType.Armor:
                    itemIndex = GetIndexFromTotalItemList(Player.MyArmor.UID);
                    EquipDic[TakeOffType].Add(ItemList[itemIndex]);
                    Player.TakeOff(Player.MyArmor);
                    break;
                case EquipType.Accessory:
                    if (Player.MyAccessorys.Length > 0)
                    {
                        itemIndex = GetIndexFromTotalItemList(Player.MyAccessorys[0].UID);
                        EquipDic[TakeOffType].Add(ItemList[itemIndex]);
                        Player.TakeOff((AccessoryData)SelectedEquip, 0);
                    }
                    break;
            }
        }
        Filter();
        UpdateRoleInfo();
    }
    public void UpdateRoleInfo()
    {
        if (Player.MyWeapon != null)
        {
            WeaponIcon.sprite = Player.MyWeapon.GetICON();
            WeaponQuality.sprite = GameManager.GetItemQualityBotSprite(Player.MyWeapon.Quality);
        }
        else
        {
            WeaponIcon.sprite = GameManager.GetEquipTypeBotSprite(EquipType.Weapon);
            WeaponQuality.sprite = GameManager.GetItemQualityBotSprite(0);
        }
        if (Player.MyArmor != null)
        {
            ArmorIcon.sprite = Player.MyArmor.GetICON();
            ArmorQuality.sprite = GameManager.GetItemQualityBotSprite(Player.MyArmor.Quality);
        }
        else
        {
            ArmorIcon.sprite = GameManager.GetEquipTypeBotSprite(EquipType.Armor);
            ArmorQuality.sprite = GameManager.GetItemQualityBotSprite(0);
        }
        for (int i = 0; i < 2; i++)
        {
            if (Player.MyAccessorys[i] != null)
            {
                AccessoryIcon[i].sprite = Player.MyAccessorys[i].GetICON();
                AccessoryQuality[i].sprite = GameManager.GetItemQualityBotSprite(Player.MyAccessorys[i].Quality);
            }
            else
            {
                AccessoryIcon[i].sprite = GameManager.GetEquipTypeBotSprite(EquipType.Accessory);
                AccessoryQuality[i].sprite = GameManager.GetItemQualityBotSprite(0);
            }
        }
        StrengthText.text = Player.Strength.ToString();
        HealthText.text = Player.Health.ToString();
        ShieldText.text = Player.Shield.ToString();
        ShieldRecoveryText.text = string.Format("{0}/{1}", Player.Shield, StringData.GetString("Second"));
        MoveSpeedText.text = string.Format("{0}/{1}", Player.MoveSpeed, StringData.GetString("Second"));
        MaxMoveText.text = string.Format("{0}/{1}", Player.MaxMoveSpeed, StringData.GetString("Second"));
        AvatarTimeText.text = string.Format("{0}{1}", Player.AvatarTime, StringData.GetString("Second"));
        SkillTimeText.text = string.Format("{0}{1}", Player.SkillTime, StringData.GetString("Second"));
    }
}
