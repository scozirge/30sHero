using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MyText : Text
{
    public string UIString;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!Application.isPlaying)
            return;
        if (UIString == "" || !GameDictionary.IsInit)
            return;
        if (GameDictionary.String_UIDic.ContainsKey(UIString))
        {
            text = GameDictionary.String_UIDic[UIString].GetString(Player.UseLanguage);
        }
        else
        {
            text = "undefined";
        }
    }

}