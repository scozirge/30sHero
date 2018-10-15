using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour {

    public static bool IsInit { get; private set; }
    static PopupUI Myself;
    [SerializeField]
    GameObject LoadingGo;
    [SerializeField]
    Text Loading_Value;

    [SerializeField]
    GameObject ClickCancelGo;
    [SerializeField]
    Text ClickCancel_Value;

    [SerializeField]
    GameObject LoadingTitleGo;
    [SerializeField]
    Text LoadingTitle;
    [SerializeField]
    Text LoadingValue;

    [SerializeField]
    GameObject ClickCancelTitleGo;
    [SerializeField]
    Text ClickCancelTitle;
    [SerializeField]
    Text ClickCancelValue;

    [SerializeField]
    GameObject RestartGo;
    [SerializeField]
    Text Restart_Value;

    public void Init()
    {
        Myself = this;
        IsInit = true;
        DontDestroyOnLoad(gameObject);
    }
    public static void ShowLoading(string _text)
    {
        if (!Myself)
            return;
        Myself.LoadingGo.SetActive(true);
        Myself.Loading_Value.text = _text;
    }
    public static void ShowRestart(string _text)
    {
        if (!Myself)
            return;
        Myself.RestartGo.SetActive(true);
        Myself.Restart_Value.text = _text;
    }
    public void Restart()
    {
        Myself.RestartGo.SetActive(false);
        ChangeScene.RestartGame();
    }
    public static void HideLoading()
    {
        if (!Myself)
            return;
        Myself.LoadingGo.SetActive(false);
    }
    public static void ShowClickCancel( string _text)
    {
        if (!Myself)
            return;
        Myself.ClickCancelGo.SetActive(true);
        Myself.ClickCancel_Value.text = _text;
    }
    public static void HidewClickCancel()
    {
        if (!Myself)
            return;
        Myself.ClickCancelGo.SetActive(false);
    }
    public void HideCancel()
    {
        if (!Myself)
            return;
        ClickCancelGo.SetActive(false);
    }
    public static void ShowTitleLoading( string _title,string _value)
    {
        if (!Myself)
            return;
        Myself.LoadingTitleGo.SetActive(true);
        Myself.LoadingTitle.text = _title;
        Myself.LoadingValue.text = _value;
    }
    public static void HidewTitleLoading()
    {
        if (!Myself)
            return;
        Myself.LoadingTitleGo.SetActive(false);
    }
    public static void ShowTitleClickCancel(string _title, string _value)
    {
        if (!Myself)
            return;
        Myself.ClickCancelTitleGo.SetActive(true);
        Myself.ClickCancelTitle.text = _title;
        Myself.ClickCancelValue.text = _value;
    }
    public static void HideClickCancelTitle()
    {
        if (!Myself)
            return;
        Myself.ClickCancelTitleGo.SetActive(false);
    }
    public void HidekCancelTitle()
    {
        ClickCancelTitleGo.SetActive(false);
    }

}
