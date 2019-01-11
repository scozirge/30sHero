using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongregateAPIBehaviour : MonoBehaviour
{
    private static KongregateAPIBehaviour instance;
    MyTimer InitTimer;
    public static bool KongregateLogin = false;
    public static bool EndLogin;
    float WaitInitTime = 10;
    static bool Test = true;

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
        }            
    }
    void Update()
    {
        InitTimer.RunTimer();
    }
    public void EndKongregateLogin()
    {
        if (EndLogin)
            return;
        EndLogin = true;
        Player.UseLocalData(!KongregateLogin);        
    }

    public void OnKongregateAPILoaded(string userInfoString)
    {
        Debug.Log("OnKongregateAPILoaded...");
        OnKongregateUserInfo(userInfoString);
    }

    public void OnKongregateUserInfo(string userInfoString)
    {
        InitTimer.StartRunTimer = false;
        var info = userInfoString.Split('|');
        var userId = System.Convert.ToInt32(info[0]);
        var username = info[1];
        //var gameAuthToken = info[2];
        Debug.Log("///////////////Kongregate User Info: " + username + ", userId: " + userId);
        if (userId != 0)
        {
            KongregateLogin = true;
            Player.GetKongregateUserData_CB(username, userId);
        }
        ShowUserItemList();
        if (Test)
            OnShowUserItemListCB("13356700,1,,1/13356696,2,,1/13356697,2,,1/13356698,2,,1/13345602,3,,/13355292,3,,/13356695,3,,1/13356699,3,,1");
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
