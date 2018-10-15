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
    public override void OnEnable()
    {
        base.OnEnable();
        if (Player.UseLanguage == Language.EN)
            LanguageToggle.isOn = true;
        else
            LanguageToggle.isOn = false;
        if (Player.MusicOn)
            MusicToggle.isOn = true;
        else
            MusicToggle.isOn = false;
        if (Player.SoundOn)
            SoundToggle.isOn = true;
        else
            SoundToggle.isOn = false;
    }

    public void SetMusic()
    {
        Player.SetMusic(MusicToggle.isOn);
    }
    public void SetSound()
    {
        Player.SetSound(SoundToggle.isOn);
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
