using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Player
{
    public static Language UseLanguage { get; private set; }
    public static string AC { get; private set; }
    public static string ACPass { get; private set; }
    public static string Name { get; private set; }


    public static string FBID { get; private set; }

    public static void Init()
    {
        Player.SetLanguage(Language.EN);
        //PlayerPrefs.DeleteAll();//清除玩家資料

        //if (PlayerPrefs.GetString("AC") != "")
        //    AC = PlayerPrefs.GetString("AC");
        //if (PlayerPrefs.GetString("ACPass") != "")
        //    ACPass = PlayerPrefs.GetString("ACPass");
        //if (PlayerPrefs.GetString("Name") != "")
        //    Name = PlayerPrefs.GetString("Name");

    }
    public static void UpdateRecord(Dictionary<string, object> _data)
    {
    }
    public static void AutoLogin()
    {
    }
}
