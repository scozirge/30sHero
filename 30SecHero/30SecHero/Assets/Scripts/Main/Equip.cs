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
    Image[] WeaponIcon;
    [SerializeField]
    Image WeaponQuality;
    [SerializeField]
    Image[] ArmorIcon;
    [SerializeField]
    Image ArmorQuality;
    [SerializeField]
    Image[] Accessory1Icon;
    [SerializeField]
    Image[] Accessory2Icon;
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
    [SerializeField]
    GameObject SellModeBtnObj;
    [SerializeField]
    GameObject NormalModeBtnObj;
    [SerializeField]
    Text ItemCoutText;
    [SerializeField]
    MyToggle SortTypeToggle;
    [SerializeField]
    MyToggle SortWayToggle;
    [SerializeField]
    Toggle[] TagToggles;


    EquipData SelectedEquip;
    public EquipType CurFilterType;
    EquipType TakeOffType;
    List<EquipItem> ItemList = new List<EquipItem>();
    Dictionary<EquipType, List<EquipItem>> EquipDic = new Dictionary<EquipType, List<EquipItem>>();
    List<EquipItem> WeaponList = new List<EquipItem>();
    List<EquipItem> ArmorList = new List<EquipItem>();
    List<EquipItem> AccessoryList = new List<EquipItem>();
    static int CurItemIndex;
    static int CurEquipAccessoryIndex;
    public static bool SoldMode;

    void Start()
    {
        if (Player.Itmes.ContainsKey(EquipType.Weapon))
        {
            List<long> keys = new List<long>(Player.Itmes[EquipType.Weapon].Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                EquipItem ei = (EquipItem)MySpanwer.Spawn();
                ei.Set(Player.Itmes[EquipType.Weapon][keys[i]], this);
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
                EquipItem ei = (EquipItem)MySpanwer.Spawn();
                ei.Set(Player.Itmes[EquipType.Armor][keys[i]], this);
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
                EquipItem ei = (EquipItem)MySpanwer.Spawn();
                ei.Set(Player.Itmes[EquipType.Accessory][keys[i]], this);
                ItemList.Add(ei);
                if (!ei.MyData.IsEquiped)
                    AccessoryList.Add(ei);
            }
            EquipDic.Add(EquipType.Accessory, AccessoryList);
        }
        ItemCoutText.text = string.Format("{0}/{1}", ItemList.Count, GameSettingData.MaxItemCount);
        Filter();
        Sort();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        ToFilter(0);
        Sort();
        UpdateRoleInfo();
        SetSoldMode(false);
        if (TagToggles != null)
        {
            for (int i = 0; i < TagToggles.Length; i++)
            {
                TagToggles[i].isOn = false;
            }
            TagToggles[0].isOn = true;
        }
    }
    public void SetSoldMode(bool _bool)
    {
        SoldMode = _bool;
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetSoldMode(_bool);
        }
        SellModeBtnObj.SetActive(_bool);
        NormalModeBtnObj.SetActive(!_bool);
    }
    public void Sell()
    {
        List<EquipData> list = new List<EquipData>();
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].IsSoldCheck)
            {
                list.Add(ItemList[i].MyData);
                EquipDic[ItemList[i].MyData.Type].Remove(ItemList[i]);
                ItemList[i].SelfDestroy();
                ItemList[i] = null;
            }
        }
        ItemList.RemoveAll(item => item == null);
        Player.SellEquips(list);
        ItemCoutText.text = string.Format("{0}/{1}", ItemList.Count, GameSettingData.MaxItemCount);
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
                if (Player.MyAccessorys[0] != null && Player.MyAccessorys[1] == null)
                {
                    CurEquipAccessoryIndex = 1;
                }
                else
                {
                    CurEquipAccessoryIndex = 0;
                }
                EquipPop.SetEquipData(Player.MyAccessorys[CurEquipAccessoryIndex], SelectedEquip);
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

    public void NextAccessory()
    {
        CurEquipAccessoryIndex++;
        if (CurEquipAccessoryIndex >= Player.MyAccessorys.Length)
            CurEquipAccessoryIndex = 0;
        EquipPop.UpdateLeftEquipData(Player.MyAccessorys[CurEquipAccessoryIndex]);
    }
    public void PreviousAccessory()
    {
        CurEquipAccessoryIndex--;
        if (CurEquipAccessoryIndex < 0)
            CurEquipAccessoryIndex = Player.MyAccessorys.Length - 1;
        EquipPop.UpdateLeftEquipData(Player.MyAccessorys[CurEquipAccessoryIndex]);
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
        CurEquipAccessoryIndex = _index;
        if (Player.MyAccessorys[CurEquipAccessoryIndex] != null)
        {
            EquipPop.SetEquipData(Player.MyAccessorys[CurEquipAccessoryIndex], null);
            EquipPop.gameObject.SetActive(true);
        }
    }
    public void CancelEquip()
    {
        EquipPop.gameObject.SetActive(false);
    }
    public void Sort()
    {
        if (!SortTypeToggle.isOn)
        {
            ItemList.Sort(SortByQuality);
        }
        else
        {
            ItemList.Sort(SortByLV);
        }
        if (SortWayToggle.isOn)
        {
            ItemList.Reverse();
        }
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].transform.SetSiblingIndex(i);
        }
    }
    static int SortByLV(EquipItem _e1, EquipItem _e2)
    {
        return _e1.MyData.LV.CompareTo(_e2.MyData.LV);
    }
    static int SortByQuality(EquipItem _e1, EquipItem _e2)
    {
        return _e1.MyData.Quality.CompareTo(_e2.MyData.Quality);
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
                        if (Player.MyAccessorys[CurEquipAccessoryIndex] != null)
                        {
                            int index = GetIndexFromTotalItemList(Player.MyAccessorys[CurEquipAccessoryIndex].UID);
                            EquipDic[CurFilterType].Add(ItemList[index]);
                        }
                        Player.Equip((AccessoryData)SelectedEquip, CurEquipAccessoryIndex);
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
                    Player.TakeOff(TakeOffType, 0);
                    break;
                case EquipType.Armor:
                    itemIndex = GetIndexFromTotalItemList(Player.MyArmor.UID);
                    EquipDic[TakeOffType].Add(ItemList[itemIndex]);
                    Player.TakeOff(TakeOffType, 0);
                    break;
                case EquipType.Accessory:
                    if (Player.MyAccessorys.Length > 0)
                    {
                        itemIndex = GetIndexFromTotalItemList(Player.MyAccessorys[CurEquipAccessoryIndex].UID);
                        EquipDic[TakeOffType].Add(ItemList[itemIndex]);
                        Player.TakeOff(TakeOffType, 0);
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
            for (int i = 0; i < WeaponIcon.Length; i++)
            {
                WeaponIcon[i].sprite = Player.MyWeapon.Icons[i];
                WeaponIcon[i].enabled = true;
                WeaponIcon[i].SetNativeSize();
            }
            WeaponQuality.sprite = GameManager.GetItemQualityBotSprite(Player.MyWeapon.Quality);
        }
        else
        {
            for (int i = 0; i < WeaponIcon.Length; i++)
            {
                if (i == 0)
                    WeaponIcon[i].sprite = GameManager.GetEquipTypeBotSprite(EquipType.Weapon);
                else
                    WeaponIcon[i].enabled = false;
                WeaponIcon[i].SetNativeSize();
            }
            WeaponQuality.sprite = GameManager.GetItemQualityBotSprite(0);
        }
        if (Player.MyArmor != null)
        {
            for (int i = 0; i < ArmorIcon.Length; i++)
            {
                ArmorIcon[i].sprite = Player.MyArmor.Icons[i];
                ArmorIcon[i].enabled = true;
                ArmorIcon[i].SetNativeSize();
            }
            ArmorQuality.sprite = GameManager.GetItemQualityBotSprite(Player.MyArmor.Quality);
        }
        else
        {
            for (int i = 0; i < ArmorIcon.Length; i++)
            {
                if (i == 0)
                    ArmorIcon[i].sprite = GameManager.GetEquipTypeBotSprite(EquipType.Armor);
                else
                    ArmorIcon[i].enabled = false;
                ArmorIcon[i].SetNativeSize();
            }
            ArmorQuality.sprite = GameManager.GetItemQualityBotSprite(0);
        }
        //Accessory1
        if (Player.MyAccessorys[0] != null)
        {
            for (int j = 0; j < Accessory1Icon.Length; j++)
            {
                Accessory1Icon[j].sprite = Player.MyAccessorys[0].Icons[j];
                Accessory1Icon[j].enabled = true;
                Accessory1Icon[j].SetNativeSize();
            }
            AccessoryQuality[0].sprite = GameManager.GetItemQualityBotSprite(Player.MyAccessorys[0].Quality);
        }
        else
        {
            for (int j = 0; j < Accessory1Icon.Length; j++)
            {
                if (j == 0)
                    Accessory1Icon[j].sprite = GameManager.GetEquipTypeBotSprite(EquipType.Accessory);
                else
                    Accessory1Icon[j].enabled = false;
                Accessory1Icon[j].SetNativeSize();
            }
            AccessoryQuality[0].sprite = GameManager.GetItemQualityBotSprite(0);
        }
        //Accessory2
        if (Player.MyAccessorys[1] != null)
        {
            for (int j = 0; j < Accessory2Icon.Length; j++)
            {
                Accessory2Icon[j].sprite = Player.MyAccessorys[1].Icons[j];
                Accessory2Icon[j].enabled = true;
                Accessory2Icon[j].SetNativeSize();
            }
            AccessoryQuality[1].sprite = GameManager.GetItemQualityBotSprite(Player.MyAccessorys[1].Quality);
        }
        else
        {
            for (int j = 0; j < Accessory2Icon.Length; j++)
            {
                if (j == 0)

                    Accessory2Icon[j].sprite = GameManager.GetEquipTypeBotSprite(EquipType.Accessory);
                else
                    Accessory2Icon[j].enabled = false;
                Accessory2Icon[j].SetNativeSize();
            }
            AccessoryQuality[1].sprite = GameManager.GetItemQualityBotSprite(0);
        }
        StrengthText.text = Player.GetProperties(RoleProperty.Strength).ToString();
        HealthText.text = Player.GetProperties(RoleProperty.Health).ToString();
        ShieldText.text = Player.GetProperties(RoleProperty.Shield).ToString();
        ShieldRecoveryText.text = string.Format("{0}%/{1}", Player.GetProperties(RoleProperty.ShieldRecovery)*100, StringData.GetString("Second"));
        MoveSpeedText.text = string.Format("{0}/{1}", Player.GetProperties(RoleProperty.MoveSpeed), StringData.GetString("Second"));
        MaxMoveText.text = string.Format("{0}/{1}", Player.GetProperties(RoleProperty.MaxMoveSpeed)+Player.GetProperties(RoleProperty.MoveSpeed), StringData.GetString("Second"));
        AvatarTimeText.text = string.Format("{0}{1}", Player.GetProperties(RoleProperty.AvatarPotionBuff), StringData.GetString("Second"));
        SkillTimeText.text = string.Format("{0}{1}", Player.GetProperties(RoleProperty.SkillTimeBuff), StringData.GetString("Second"));
    }
}
