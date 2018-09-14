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
    void Awake()
    {
        Screen.fullScreen = true;
    }
    void Start()
    {
        if (!Debugger.IsSpawn)
            DeployDebugger();
        if (!PopupUI.IsInit)
            DeployPopupUI();
        if (!GameDictionary.IsInit)
            GameDictionary.InitDic();
        if (IsInit)
            return;
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
