using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KongregateAPIBehaviour : MonoBehaviour
{
    private static KongregateAPIBehaviour instance;
    MyTimer InitTimer;
    static bool KGIsInit;
    public static bool KongregateLogin = false;
    public static bool EndLogin;
    static float WaitInitTime = 10;
    static bool Test = false;

    public static void ResetData()
    {
        EndLogin = false;
        KongregateLogin = false;
    }

    public static void SendKGStatistics(string _name,int _value)
    {
        if (_name == null)
            return;
        if (_value < 0)
            return;
        Application.ExternalEval(@"
        parent.kongregate.stats.submit('" + _name + @"'," + _value + @");");
    }

    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("StartInitKongregateAPI...");
        Object.DontDestroyOnLoad(gameObject);
        gameObject.name = "KongregateAPI";

        Application.ExternalEval(
@"if(typeof(kongregateUnitySupport) != 'undefined'){
            kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');
          };");




        if (!Application.isEditor)
            InitTimer = new MyTimer(WaitInitTime, EndKongregateLogin, true, false);
        else
        {
            InitTimer = new MyTimer(0.1f, EndKongregateLogin, true, false);
        }
        if (Test)
        {
            OnKongregateUserInfo("1|scozirge");
            //OnKongregateUserInfo("41605611|starbrogamemaker");
            //OnKongregateUserInfo("100|t12");
        }
    }
    //static MyTimer WaitSignInCheck;
    public static bool Relogin;//是否為遊戲進行到一半時登入
    public static void KGLogin()
    {
        EndLogin = false;
        Relogin = true;
        Debug.Log("Kongregate Login");
        Application.ExternalEval(@"
          parent.kongregate.services.showRegistrationBox();");
    }
    public static void OnKongregateInPageLogin(string _userInfoString)
    {
        Debug.Log("OnKongregateInPageLogin.................");
        Debug.Log("_userInfoString=" + _userInfoString);
    }




    void Update()
    {
        if (InitTimer != null)
            InitTimer.RunTimer();
        //if (WaitSignInCheck != null)
        //WaitSignInCheck.RunTimer();
    }
    public static void EndKongregateLogin()
    {
        if (EndLogin)
            return;
        if (!KongregateLogin && Relogin)
        {
            Debug.Log("Fail To InPageLogin...");
            CaseTableData.HidePopLog(1001);
            CaseTableData.ShowPopLog(9);
            Relogin = false;
            return;
        }
        Player.UseLocalData(!KongregateLogin);
        EndLogin = true;
    }

    public void OnKongregateAPILoaded(string userInfoString)
    {
        Debug.Log("OnKongregateAPILoaded...");
        if(!KGIsInit)
        {
            Application.ExternalEval(@"
        parent.kongregate.stats.submit('EnterGame', 1);");
        }
        KGIsInit = true;
        //偵聽是否有登入kongregate
        if (!Relogin)
        {
            Debug.Log("建立kongregate登入偵聽");
            Application.ExternalEval(@"
      parent.kongregate.services.addEventListener('login', function(){
        var unityObject = kongregateUnitySupport.getUnityObject();
        var services = parent.kongregate.services;
        var params=[services.getUserId(), services.getUsername(), 
                    services.getGameAuthToken()].join('|');

        unityObject.SendMessage('KongregateAPI', 'OnKongregateUserInfo', params);
    });"
);
        }


        //設定玩家資料
        OnKongregateUserInfo(userInfoString);
    }

    public void OnKongregateUserInfo(string userInfoString)
    {
        //重置玩家資料
        ResetData();
        Player.ResetData();

        if (InitTimer != null)
            InitTimer.StartRunTimer = false;
        //if (WaitSignInCheck!=null)
        //WaitSignInCheck.StartRunTimer = false;
        var info = userInfoString.Split('|');
        var userId = System.Convert.ToInt32(info[0]);
        var username = info[1];
        //var gameAuthToken = info[2];
        //Debug.Log("///////////////Kongregate User Info: " + username + ", userId: " + userId);
        if (userId != 0)//帳號ID不是0代表登入Kongregate帳號了
        {
            KongregateLogin = true;
            if (!Relogin)//一開始進遊戲登入kongregate成功
            {
                Debug.Log("KongregateLogin=" + KongregateLogin);
                Player.GetKongregateUserData_CB(username, userId);
                ShowUserItemList();
            }
            else//商品頁面跳登入視窗時後登入成功
            {
                Relogin = false;
                Init();
                PopupUI.CallCutScene("Init");
            }
        }
        else//帳號ID是0代表沒登入Kongregate
        {
            if (!Relogin)//一開始遊戲登入失敗
            {
                EndKongregateLogin();
                Debug.Log("Guest account");
            }
            else//商品頁面跳登入視窗時，登入失敗
            {
                Relogin = false;
                CaseTableData.HidePopLog(1001);
                CaseTableData.ShowPopLog(9);
            }
        }
        if (Test)
            OnShowUserItemListCB("13356700,1,,1/13356696,2,,1/13356697,2,,1/13356698,2,,1/13345602,3,,/13355292,3,,/13356695,3,,1");
    }
    public static void ShowItemList()
    {
        Debug.Log("////////////////Send ShowItemList");
        Application.ExternalEval(@"
          parent.kongregate.mtx.requestItemList([], function(result) {
            var unityObject = kongregateUnitySupport.getUnityObject();
            if(result.success) {
                var datas = [];
                for(var i = 0; i < result.data.length; i++) 
                {
                    var item = result.data[i];
                    if(i!=0)
                        datas+='/';
                    datas+=[item.identifier, item.name, item.description , item.price ].join(',');
                }
                unityObject.SendMessage('KongregateAPI', 'OnItemListCB', datas);     
            }
            else
            {
                unityObject.SendMessage('KongregateAPI', 'OnItemListCB', 'Fail'); 
            }
          });
        ");
    }
    public void OnItemListCB(string _datas)
    {
        if (_datas != "Fail")
        {
            Purchase.MySelf.ShowItemListCB(_datas);
        }
        else
        {
            CaseTableData.ShowPopLog(8);
        }
    }
    public static void PurchaseItem(int _id)
    {
        Debug.Log("////////////////Send PurchaseIte ID:" + _id);
        Application.ExternalEval(@"
          parent.kongregate.mtx.purchaseItems(['" + _id + @"'], function(result) {
            var unityObject = kongregateUnitySupport.getUnityObject();
            var success = String(result.success);
            unityObject.SendMessage('KongregateAPI', 'OnPurchaseResult', success);
          });");
    }
    public void OnPurchaseResult(string _result)
    {
        Debug.Log("OnPurchaseResult" + _result);
        if (_result == "true")
        {
            Purchase.ToPurchaseCB(true);
            GetUserItemList();
        }
        else
        {
            Purchase.ToPurchaseCB(false);
        }
    }
    public static void ShowUserItemList()
    {
        Debug.Log("////////////////Send ShowUserItemList");
        Application.ExternalEval(@"
          parent.kongregate.mtx.requestUserItemList(null, function(result) {
            var unityObject = kongregateUnitySupport.getUnityObject();
            if(result.success) {
                var datas = '';
                for(var i = 0; i < result.data.length; i++) 
                {
                    var item = result.data[i];
                    if(i!=0)
                        datas+='/';
                    datas+=[item.id, item.identifier, item.data , item.remaining_uses ].join(',');
                }
                if(datas=='')
                    datas='Empty';
                unityObject.SendMessage('KongregateAPI', 'OnShowUserItemListCB', datas);     
            }
            else
            {
                unityObject.SendMessage('KongregateAPI', 'OnShowUserItemListCB', 'Fail'); 
            }
          });
        ");
    }

    public void OnShowUserItemListCB(string _datas)
    {
        //Debug.Log("///////////////Kongregate UserItem Info: " + _datas);
        switch(_datas)
        {
            case "Fail":
                Debug.Log("Fail to ShowUserItemListCB");
                CaseTableData.ShowPopLog(8);
                break;
            case "Empty":
                Debug.Log("PayItem is Empty");
                EndKongregateLogin();
                break;
            default:
                Player.ShowUserItemListCB(_datas);
                EndKongregateLogin();
                break;
        }
    }

    public static void GetUserItemList()
    {
        Debug.Log("////////////////Send GetUserItemList");
        Application.ExternalEval(@"
          parent.kongregate.mtx.requestUserItemList(null, function(result) {
            var unityObject = kongregateUnitySupport.getUnityObject();
            if(result.success) {
                var datas = '';
                for(var i = 0; i < result.data.length; i++) 
                {
                    var item = result.data[i];
                    if(i!=0)
                        datas+='/';
                    datas+=[item.id, item.identifier, item.data , item.remaining_uses ].join(',');
                }
                if(datas=='')
                    datas='Empty';
                unityObject.SendMessage('KongregateAPI', 'OnGetUserItemListCB', datas);     
            }
            else
            {
                unityObject.SendMessage('KongregateAPI', 'OnGetUserItemListCB', 'Fail'); 
            }
          });
        ");
    }
    public void OnGetUserItemListCB(string _datas)
    {
        //Debug.Log("///////////////Kongregate UserItem Info: " + _datas);
        switch (_datas)
        {
            case "Fail":
                Debug.Log("Fail to GetUserItemListCB");
                break;
            case "Empty":
                Debug.Log("PayItem is Empty");
                ServerRequest.UpdateResource();
                break;
            default:
                Player.ShowUserItemListCB(_datas);
                ServerRequest.UpdateResource();
                break;
        }
    }
}
