using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Main : MonoBehaviour
{
    bool Isinit = false;
    [SerializeField]
    Text GoldText;
    [SerializeField]
    Text EmeraldText;
    [SerializeField]
    Strengthen MyStrengthen;
    [SerializeField]
    Equip MyEquip;
    [SerializeField]
    Purchase MyPurchase;
    [SerializeField]
    Set MySet;
    [SerializeField]
    GameObject KGLoginBtn;
    [SerializeField]
    MainUI CurUI;
    [SerializeField]
    GameObject EquipBtnTip;
    [SerializeField]
    GameObject StrengthenTagTip;
    [SerializeField]
    GameObject EnchantTagTip;
    [SerializeField]
    GameObject WeaponTagTip;
    [SerializeField]
    GameObject ArmorTagTip;
    [SerializeField]
    GameObject AccessoryTagTip;



    public static bool ShowEquipBtnTip;
    public static bool ShowStrengthenTagTip;
    public static bool ShowEnchantTagTip;
    public static bool ShowWeaponTagTip;
    public static bool ShowArmorTagTip;
    public static bool ShowAccessoryTagTip;

    static Text MyGoldText;
    static Text MyEmeraldText;

    Dictionary<MainUI, MyUI> UIDic = new Dictionary<MainUI, MyUI>();

    // Use this for initialization
    void Awake()
    {
        if (!GameManager.IsInit)
            GameManager.DeployGameManager();
    }
    void Init()
    {
        if (Isinit)
            return;
        UIDic.Add(MainUI.Strengthen, MyStrengthen);
        UIDic.Add(MainUI.Purchase, MyPurchase);
        UIDic.Add(MainUI.Equip, MyEquip);
        List<MainUI> keys = new List<MainUI>(UIDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] != CurUI)
                UIDic[keys[i]].SetActive(false);
            else
                UIDic[keys[i]].SetActive(true);
        }
        MyGoldText = GoldText;
        MyEmeraldText = EmeraldText;
        GoldText.text = Player.Gold.ToString();
        EmeraldText.text = Player.Emerald.ToString();
        Isinit = true;
        /*測試送server
        List<EquipData> datas=new List<EquipData>();
        datas.Add(EquipData.GetRandomNewEquip(3,5));
        datas.Add(EquipData.GetRandomNewEquip(2,1));
        Player.Settlement(2000, 500, 3, datas);
         */
    }
    void OnEnable()
    {
        UpdateTips();
        if (Player.Name_K==null || Player.Name_K == "")
            KGLoginBtn.SetActive(true);
        else
            KGLoginBtn.SetActive(false);
        if (!Isinit)
            return;
        GoldText.text = Player.Gold.ToString();
        EmeraldText.text = Player.Emerald.ToString();
    }
    public void KGLogin()
    {
        KongregateAPIBehaviour.KGLogin();
    }
    void Update()
    {
        if (Player.IsInit)
            Init();
    }
    public static void UpdateResource()
    {
        if (MyGoldText == null)
            return;
        MyGoldText.text = Player.Gold.ToString();
        MyEmeraldText.text = Player.Emerald.ToString();
    }
    public void ChangeUI(int _enumID)
    {
        MainUI type = (MainUI)_enumID;
        ChangeUI(type);
    }
    public void ChangeUI(MainUI _ui)
    {
        if (_ui == CurUI)
            return;
        if (!UIDic.ContainsKey(_ui))
            return;
        if (_ui == MainUI.Equip)//關閉tip
            SetTip(TipType.EquipBtnTip, false);
        UIDic[CurUI].SetActive(false);
        UIDic[_ui].SetActive(true);
        CurUI = _ui;
    }
    public void Set(bool _bool)
    {
        MySet.SetActive(_bool);
    }
    public void Battle()
    {
        if (Player.ItemCout > GameSettingData.MaxItemCount)
        {
            PopupUI.ShowClickCancel(StringData.GetString("EquipLimit"));
        }
        else
        {
            PopupUI.CallCutScene("Battle");
            Player.UpToDateCurMaxEquipUID();
            BattleManage.NewGetEnchatIDs.Clear();
        }

        //SceneManager.LoadScene("Battle");
    }
    public void TestBtn()
    {
        SceneManager.LoadScene("Main");
    }
    public void TestSceneBtn()
    {
        SceneManager.LoadScene("test");
    }
    public static void ResetTipBool()
    {
        ShowEquipBtnTip = false;
        ShowStrengthenTagTip = false;
        ShowEnchantTagTip = false;
        ShowWeaponTagTip = false;
        ShowArmorTagTip = false;
        ShowAccessoryTagTip = false;
    }
    public void UpdateTips()
    {
        SetTip(TipType.EquipBtnTip, ShowEquipBtnTip);
        SetTip(TipType.StrengthenTagTip, ShowStrengthenTagTip);
        SetTip(TipType.EnchantTagTip, ShowEnchantTagTip);
        SetTip(TipType.WeaponTagTip, ShowWeaponTagTip);
        SetTip(TipType.ArmorTagTip, ShowArmorTagTip);
        SetTip(TipType.AccessoryTagTip, ShowAccessoryTagTip);
    }
    public void SetTip(TipType _type, bool _on)
    {
        switch (_type)
        {
            case TipType.EquipBtnTip:
                EquipBtnTip.SetActive(_on);
                ShowEquipBtnTip = _on;
                break;
            case TipType.StrengthenTagTip:
                StrengthenTagTip.SetActive(_on);
                ShowStrengthenTagTip = _on;
                break;
            case TipType.EnchantTagTip:
                EnchantTagTip.SetActive(_on);
                ShowEnchantTagTip = _on;
                break;
            case TipType.WeaponTagTip:
                WeaponTagTip.SetActive(_on);
                ShowWeaponTagTip = _on;
                break;
            case TipType.ArmorTagTip:
                ArmorTagTip.SetActive(_on);
                ShowArmorTagTip = _on;
                break;
            case TipType.AccessoryTagTip:
                AccessoryTagTip.SetActive(_on);
                ShowAccessoryTagTip = _on;
                break;
        }
    }
}
public enum TipType
{
    EquipBtnTip,
    StrengthenTagTip,
    EnchantTagTip,
    WeaponTagTip,
    ArmorTagTip,
    AccessoryTagTip
}
