using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongregateAPIBehaviour : MonoBehaviour
{
    private static KongregateAPIBehaviour instance;

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
        //Player.SetKongregateUserData_CB("scozirge", 1);
    }

    public void OnKongregateAPILoaded(string userInfoString)
    {
        Debug.Log("OnKongregateAPILoaded...");
        OnKongregateUserInfo(userInfoString);
    }

    public void OnKongregateUserInfo(string userInfoString)
    {
        var info = userInfoString.Split('|');
        var userId = System.Convert.ToInt32(info[0]);
        var username = info[1];
        //var gameAuthToken = info[2];
        Debug.Log("///////////////Kongregate User Info: " + username + ", userId: " + userId);
        Player.SetKongregateUserData_CB(username, userId);
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
        }
        else
        {
            Purchase.ToPurchaseCB(false);
        }
    }
}
