using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : MyUI
{
    [SerializeField]
    MyToggle MusicToggle;
    [SerializeField]
    MyToggle SoundToggle;
    [SerializeField]
    MyToggle LanguageToggle;

    public void SetMusic()
    {
        AudioPlayer.MuteMusic(!MusicToggle.isOn);
    }
    public void SetSound()
    {
        AudioPlayer.MuteSound(!SoundToggle.isOn);
    }
    public void SetLanguage()
    {
        if (LanguageToggle.isOn)
        {
            Player.SetLanguage(Language.EN);
        }
        else
        {
            Player.SetLanguage(Language.ZH_TW);
        }
    }
}
