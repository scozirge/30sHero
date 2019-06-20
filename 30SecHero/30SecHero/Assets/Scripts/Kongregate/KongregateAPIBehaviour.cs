using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongregateAPIBehaviour : MonoBehaviour
{
    private static KongregateAPIBehaviour instance;
    MyTimer InitTimer;
    public static bool KongregateLogin = false;
    public static bool EndLogin;
    static float WaitInitTime = 10;
    static bool Test = false;

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
      };"
        );
        if (!Application.isEditor)
            InitTimer = new MyTimer(WaitInitTime, EndKongregateLogin, true, false);
        else
        {
            InitTimer = new MyTimer(0.1f, EndKongregateLogin, true, false);
        }
        if (Test)
        {
            //OnKongregateUserInfo("1|scozirge");
            OnKongregateUserInfo("41605611|starbrogamemaker");
            //OnKongregateUserInfo("100|t12");
        }
    }
    static MyTimer WaitSignInCheck;
    public static bool Relogin;//是否為遊戲進行到一半時登入
    public static void KGLogin()
    {
        EndLogin = false;
        Relogin = true;
        Debug.Log("Kongregate Login");
        Application.ExternalEval(@"
          kongregate.services.showRegistrationBox();");
        CaseTableData.ShowPopLog(1001);
        Application.ExternalEval(
            @"if(typeof(kongregateUnitySupport) != 'undefined'){
        kongregate.services.addEventListener('login', 'OnKongregateInPageLogin');
      };"
            );
        WaitSignInCheck = new MyTimer(WaitInitTime, EndKongregateLogin, true, false);
    }
    public static void OnKongregateInPageLogin(string _userInfoString)
    {
        Debug.Log("OnKongregateInPageLogin.................");
        Debug.Log("_userInfoString=" + _userInfoString);
        Application.ExternalEval(
@"if(typeof(kongregateUnitySupport) != 'undefined'){
        kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');
      };"
);
        /*
         kongregate.services.addEventListener("login", onKongregateInPageLogin);

        function onKongregateInPageLogin() {
          var user_id = kongregate.services.getUserId();
          var username = kongregate.services.getUsername();
          var token = kongregate.services.getGameAuthToken();
        }
         */
    }




    void Update()
    {
        if (InitTimer != null)
            InitTimer.RunTimer();
        if (WaitSignInCheck != null)
            WaitSignInCheck.RunTimer();
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
        OnKongregateUserInfo(userInfoString);
    }

    public void OnKongregateUserInfo(string userInfoString)
    {
        if (InitTimer!=null)
            InitTimer.StartRunTimer = false;
        if (WaitSignInCheck!=null)
            WaitSignInCheck.StartRunTimer = false;
        var info = userInfoString.Split('|');
        var userId = System.Convert.ToInt32(info[0]);
        var username = info[1];
        //var gameAuthToken = info[2];
        Debug.Log("///////////////Kongregate User Info: " + username + ", userId: " + userId);
        if (userId != 0)
        {
            KongregateLogin = true;
            if (!Relogin)
            {
                Debug.Log("KongregateLogin=" + KongregateLogin);
                Player.GetKongregateUserData_CB(username, userId);
                ShowUserItemList();
            }
            else
            {
                EndKongregateLogin();
            }
        }
        else
        {
            if (!Relogin)
            {
                EndKongregateLogin();
                Debug.Log("Guest account");
            }
            else
            {
                Relogin = false;
                CaseTableData.HidePopLog(1001);
            }
        }
        if (Test)
            OnShowUserItemListCB("13356700,1,,1/13356696,2,,1/13356697,2,,1/13356698,2,,1/13345602,3,,/13355292,3,,/13356695,3,,1");
    }
    public static void ShowItemList()
    {
        Debug.Log("////////////////Send ShowItemList");
        Application.ExternalEval(@"
          kongregate.mtx.requestItemList([], function(result) {
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
            Purchase.MySelf.ShowItemListCB(_datas);
        else
        {
            CaseTableData.ShowPopLog(8);
        }
    }
    public static void PurchaseItem(int _id)
    {
        Debug.Log("////////////////Send PurchaseIte ID:" + _id);
        Application.ExternalEval(@"
          kongregate.mtx.purchaseItems(['" + _id + @"'], function(result) {
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
          kongregate.mtx.requestUserItemList(null, function(result) {
            var unityObject = kongregateUnitySupport.getUnityObject();
            if(result.success) {
                var datas = [];
                for(var i = 0; i < result.data.length; i++) 
                {
                    var item = result.data[i];
                    if(i!=0)
                        datas+='/';
                    datas+=[item.id, item.identifier, item.data , item.remaining_uses ].join(',');
                }       
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
        Debug.Log("///////////////Kongregate UserItem Info: " + _datas);
        if (_datas != "Fail")
        {
            Player.ShowUserItemListCB(_datas);
            EndKongregateLogin();
        }
        else
        {
            Debug.Log("Fail to ShowUserItemListCB");
            CaseTableData.ShowPopLog(8);
        }
    }

    public static void GetUserItemList()
    {
        Debug.Log("////////////////Send GetUserItemList");
        Application.ExternalEval(@"
          kongregate.mtx.requestUserItemList(null, function(result) {
            var unityObject = kongregateUnitySupport.getUnityObject();
            if(result.success) {
                var datas = [];
                for(var i = 0; i < result.data.length; i++) 
                {
                    var item = result.data[i];
                    if(i!=0)
                        datas+='/';
                    datas+=[item.id, item.identifier, item.data , item.remaining_uses ].join(',');
                }       
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
        Debug.Log("///////////////Kongregate UserItem Info: " + _datas);
        if (_datas != "Fail")
        {
            Player.ShowUserItemListCB(_datas);
            ServerRequest.UpdateResource();
        }
        else
        {
            Debug.Log("Fail to GetUserItemListCB");
        }
    }
}
