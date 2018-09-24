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

    static Sprite[] QualityBotSprites;
    static Sprite[] EquipTypBot;

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
    void Awake()
    {
        Screen.fullScreen = true;
        QualityBotSprites = QualityBotPrefabs;
        EquipTypBot = EquipTypBotPrefab;
    }
    void Start()
    {
        if (!Debugger.IsSpawn)
            DeployDebugger();
        if (!PopupUI.IsInit)
            DeployPopupUI();
        if (IsInit)
            return;
        if (!GameDictionary.IsInit)
            GameDictionary.InitDic();
        Player.Init();
        DontDestroyOnLoad(gameObject);
        IsInit = true;
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
}
