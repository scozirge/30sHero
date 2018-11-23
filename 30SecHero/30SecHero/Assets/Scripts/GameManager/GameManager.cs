using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{

    public static bool IsInit { get; protected set; }
    [SerializeField]
    Debugger DebuggerPrefab;
    [SerializeField]
    PopupUI PopUIPrefab;
    [SerializeField]
    Sprite[] QualityBotPrefabs;
    [SerializeField]
    Sprite[] EquipTypBotPrefab;
    [SerializeField]
    ServerRequest MyServer;


    [Tooltip("金幣圖")]
    [SerializeField]
    Sprite GoldSprite;
    [Tooltip("寶石圖")]
    [SerializeField]
    Sprite EmeraldSprite;
    [Tooltip("暈眩特效")]
    [SerializeField]
    public ParticleSystem StunPrefab;
    [Tooltip("冰凍特效")]
    [SerializeField]
    public ParticleSystem FreezePrefab;
    [Tooltip("燃燒特效")]
    [SerializeField]
    public ParticleSystem BurnPrefab;
    [Tooltip("詛咒特效")]
    [SerializeField]
    public ParticleSystem DamageDownPrefab;
    [Tooltip("無敵特效")]
    [SerializeField]
    public ParticleSystem ImmortalPrefab;
    [Tooltip("傷害上升特效")]
    [SerializeField]
    public ParticleSystem DamageUpPrefab;
    [Tooltip("隔檔特效")]
    [SerializeField]
    public ParticleSystem BlockPrefab;
    [Tooltip("速度上升特效")]
    [SerializeField]
    public ParticleSystem SpeedUpPrefab;
    [Tooltip("補血特效")]
    [SerializeField]
    public ParticleSystem HealPrefab;
    [Tooltip("玩家子彈特效")]
    [SerializeField]
    ParticleSystem PlayerAmmoParticle;
    [Tooltip("敵方子彈特效")]
    [SerializeField]
    ParticleSystem EnemyAmmoParticle;
    [Tooltip("肉搏反彈傷害特效")]
    [SerializeField]
    public ParticleSystem ReflectMeleeDamageParticle;
    [Tooltip("強化衝刺傷害特效")]
    [SerializeField]
    public ParticleSystem LethalDashParticle;
    [Tooltip("子彈反彈特效")]
    [SerializeField]
    public ParticleSystem AmmoReverseParticle;
    [Tooltip("淨化(消除負面)特效")]
    [SerializeField]
    public ParticleSystem PurifyParticle;
    [Tooltip("吃藥水加速特效")]
    [SerializeField]
    public ParticleSystem PotionSpeedUpParticle;
    [Tooltip("火焰刀特效")]
    [SerializeField]
    public ParticleSystem FireBladeParticle;
    [Tooltip("爪子特效")]
    [SerializeField]
    public ParticleSystem ClawParticle;



    [Tooltip("獲得金幣聲音")]
    [SerializeField]
    public AudioClip CoinSound;


    public static GameManager GM;
    static Sprite[] QualityBotSprites;
    static Sprite[] EquipTypBot;
    static Dictionary<RoleBuffer, ParticleSystem> BufferParticles = new Dictionary<RoleBuffer, ParticleSystem>();
    static Dictionary<Currency, Sprite> CurrencySpriteDic = new Dictionary<Currency, Sprite>();
    static Dictionary<string, ParticleSystem> OtherParticles = new Dictionary<string, ParticleSystem>();
    KongregateAPIBehaviour KG;

    public static Sprite GetItemQualityBotSprite(int _quality)
    {
        if (QualityBotSprites.Length >= _quality || _quality < 0)
            return QualityBotSprites[_quality];
        else if (QualityBotSprites[_quality] == null)
            return null;
        return null;
    }
    public static Sprite GetEquipTypeBotSprite(EquipType _type)
    {
        return EquipTypBot[(int)_type];
    }
    public static ParticleSystem GetBufferParticle(RoleBuffer _type)
    {
        if (!BufferParticles.ContainsKey(_type))
        {
            //Debug.LogWarning(string.Format("無此狀態特效:{0}", _type));
            return null;
        }
        return BufferParticles[_type];
    }
    public static Sprite GetCurrencySprite(Currency _type)
    {
        if (!CurrencySpriteDic.ContainsKey(_type))
            return null;
        return CurrencySpriteDic[_type];
    }
    void Init()
    {
        if (IsInit)
            return;
        GM = this;
        CurrencySpriteDic.Add(Currency.Gold, GoldSprite);
        CurrencySpriteDic.Add(Currency.Emerald, EmeraldSprite);
        QualityBotSprites = QualityBotPrefabs;
        EquipTypBot = EquipTypBotPrefab;
        BufferParticles.Add(RoleBuffer.Stun, StunPrefab);
        BufferParticles.Add(RoleBuffer.Freeze, FreezePrefab);
        BufferParticles.Add(RoleBuffer.Burn, BurnPrefab);
        BufferParticles.Add(RoleBuffer.DamageDown, DamageDownPrefab);
        BufferParticles.Add(RoleBuffer.Immortal, ImmortalPrefab);
        BufferParticles.Add(RoleBuffer.Block, BlockPrefab);
        BufferParticles.Add(RoleBuffer.DamageUp, DamageUpPrefab);
        BufferParticles.Add(RoleBuffer.SpeedUp, SpeedUpPrefab);
        OtherParticles.Add("Heal", HealPrefab);
        if (!Debugger.IsSpawn)
            DeployDebugger();
        if (!PopupUI.IsInit)
            DeployPopupUI();
        if (!GameDictionary.IsInit)
            GameDictionary.InitDic();
        MyServer.Init();
        KG = GetComponent<KongregateAPIBehaviour>();
        if (KG != null)
            KG.Init();
        DontDestroyOnLoad(gameObject);
        IsInit = true;
        Debug.Log("GameManager Inited");
    }
    void Awake()
    {
        Init();
    }
    public static void DeployGameManager()
    {
        GameManager gmPrefab = Resources.Load<GameManager>("Prefabs/GameManager");
        GameManager gm = Instantiate(gmPrefab, Vector3.zero, Quaternion.identity) as GameManager;
        gm.transform.position = Vector3.zero; ;
        gm.Init();
    }
    void DeployDebugger()
    {
        GameObject go = Instantiate(DebuggerPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.position = Vector3.zero;
    }
    void DeployPopupUI()
    {
        PopupUI ppui = Instantiate(PopUIPrefab, Vector3.zero, Quaternion.identity) as PopupUI;
        ppui.transform.position = Vector3.zero; ;
        ppui.Init();
    }
    public static ParticleSystem GetOtherParticle(string _key)
    {
        if (OtherParticles.ContainsKey(_key))
        {
            return OtherParticles[_key];
        }
        Debug.LogWarning("不存在的特效名稱:" + _key);
        return null;
    }
    public static ParticleSystem GetForceAmmoParticle(Force _force)
    {
        if (!GM)
            return null;
        switch (_force)
        {
            case Force.Player:
                return GM.PlayerAmmoParticle;
            case Force.Enemy:
                return GM.EnemyAmmoParticle;
            default:
                return GM.EnemyAmmoParticle;
        }
    }
}
