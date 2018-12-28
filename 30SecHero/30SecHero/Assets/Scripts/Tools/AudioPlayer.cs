using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static bool IsInit;
    public static bool IsSoundMute = false;
    public static bool IsMusicMute = false;
    static List<AudioSource> SoundList;
    static List<AudioSource> MusicList;
    static GameObject MySoundObject;
    static GameObject MyMusicObject;

    static Dictionary<string, AudioSource> LoopSoundDic;
    static Dictionary<string, AudioSource> LoopMusicDic;
    static AudioSource CurPlaySound;
    static AudioSource CurPlayMusic;

    void Awake()
    {
        if (!IsInit)
        {
            Init();
        }
    }
    static void Init()
    {

        LoopSoundDic = new Dictionary<string, AudioSource>();
        LoopMusicDic = new Dictionary<string, AudioSource>();
        SoundList = new List<AudioSource>();
        MusicList = new List<AudioSource>();
        MySoundObject = new GameObject("SoundPlayer");
        MyMusicObject = new GameObject("MusicPlayer");
        DontDestroyOnLoad(MySoundObject);
        DontDestroyOnLoad(MyMusicObject);
        AudioSource mySound = MySoundObject.AddComponent<AudioSource>();
        AudioSource myMusic = MyMusicObject.AddComponent<AudioSource>();
        SoundList.Add(mySound);
        MusicList.Add(myMusic);
        CurPlaySound = mySound;
        CurPlayMusic = myMusic;
        IsInit = true;
    }
    public static IEnumerator FadeOut(AudioClip _ac, string _key, float FadeTime)
    {
        AudioSource myAs = PlayLoopMusic_Static(_ac, _key);
        float startVolume = myAs.volume;
        while (myAs.volume > 0)
        {
            myAs.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        myAs.Stop();
    }
    public static IEnumerator FadeIn(AudioClip _ac, string _key, float FadeTime)
    {
        AudioSource myAs = PlayLoopMusic_Static(_ac, _key);
        myAs.volume = 0f;
        while (myAs.volume < 1)
        {
            myAs.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
    public static IEnumerator FadeOut(string _key, float FadeTime)
    {
        if (LoopMusicDic.ContainsKey(_key))
        {
            float startVolume = LoopMusicDic[_key].volume;
            while (LoopMusicDic[_key].volume > 0)
            {
                LoopMusicDic[_key].volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            LoopMusicDic[_key].Stop();
        }
    }
    public static IEnumerator FadeIn(string _key, float FadeTime)
    {
        if (LoopMusicDic.ContainsKey(_key))
        {
            LoopMusicDic[_key].Play();
            LoopMusicDic[_key].volume = 0f;
            while (LoopMusicDic[_key].volume < 1)
            {
                LoopMusicDic[_key].volume += Time.deltaTime / FadeTime;
                yield return null;
            }
        }
    }
    public static void MuteSound(bool _isMute)
    {
        IsSoundMute = _isMute;
    }
    public static void MuteMusic(bool _isMute)
    {
        IsMusicMute = _isMute;
    }
    public void PlaySoundByString(string _soundName)
    {
        if (IsSoundMute)
            return;

        if (IsSoundMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewSoundSource();
        }
        CurPlaySound.clip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", _soundName));
        CurPlaySound.loop = false;
        CurPlaySound.Play(0);
    }
    public static void PlaySound(AudioClip _ac)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return;
        }
        if (IsSoundMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewSoundSource();
        }
        CurPlaySound.clip = _ac;
        CurPlaySound.loop = false;
        CurPlaySound.Play(0);
    }
    public void PlaySoundByAudioClip(AudioClip _ac)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return;
        }
        if (IsSoundMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewSoundSource();
        }
        CurPlaySound.clip = _ac;
        CurPlaySound.loop = false;
        CurPlaySound.Play(0);
    }
    public void PlayMusicByString(string _MusicName)
    {
        if (IsMusicMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.loop = false;
        CurPlayMusic.clip = Resources.Load<AudioClip>(string.Format("Musics/{0}", _MusicName));
        CurPlayMusic.Play(0);
    }
    public void PlayMusicByAudioClip(AudioClip _ac)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return;
        }
        if (IsMusicMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.loop = false;
        CurPlayMusic.clip = _ac;
        CurPlayMusic.Play(0);
    }
    public static void PlayMusicByAudioClip_Static(AudioClip _ac)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return;
        }
        if (IsMusicMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.loop = false;
        CurPlayMusic.clip = _ac;
        CurPlayMusic.Play(0);
    }
    public void PlayLoopSound(AudioClip _ac, string _key)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return;
        }
        if (IsSoundMute)
            return;
        if (LoopSoundDic.ContainsKey(_key))
        {
            Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return;
        }
        if (IsSoundMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableSoundSource() == null)
        {
            GetNewSoundSource();
        }
        CurPlaySound.clip = _ac;
        CurPlaySound.loop = true;
        CurPlaySound.Play();
        LoopSoundDic.Add(_key, CurPlaySound);
    }
    public void StopLoopSound(string _key)
    {
        if (LoopSoundDic.ContainsKey(_key))
        {
            LoopSoundDic[_key].Stop();
            LoopSoundDic[_key].loop = false;
            LoopSoundDic.Remove(_key);
        }
        else
            Debug.LogWarning(string.Format("Key:{0}　不存在尋換播放音效清單中", _key));
    }
    public void PlayLoopMusic(AudioClip _ac, string _key)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return;
        }
        if (IsMusicMute)
            return;
        if (LoopMusicDic.ContainsKey(_key))
        {
            Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return;
        }
        if (IsMusicMute)
            return;
        if (!IsInit)
            Init();
        if (GetApplicableMusicSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.clip = _ac;
        CurPlayMusic.loop = true;
        CurPlayMusic.Play();
        LoopMusicDic.Add(_key, CurPlayMusic);
    }
    public static AudioSource PlayLoopMusic_Static(AudioClip _ac, string _key)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return null;
        }
        if (IsMusicMute)
            return null;
        if (LoopMusicDic.ContainsKey(_key))
        {
            Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return null;
        }
        if (IsMusicMute)
            return null;
        if (!IsInit)
            Init();
        if (GetApplicableMusicSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.clip = _ac;
        CurPlayMusic.loop = true;
        CurPlayMusic.Play();
        LoopMusicDic.Add(_key, CurPlayMusic);
        return LoopMusicDic[_key];
    }

    public void StopLoopMusic(string _key)
    {
        if (LoopMusicDic.ContainsKey(_key))
        {
            LoopMusicDic[_key].Stop();
            LoopMusicDic[_key].loop = false;
            LoopMusicDic.Remove(_key);
        }
        //else
        //Debug.LogWarning(string.Format("Key:{0}　不存在尋換播放音效清單中", _key));
    }
    public static void StopAllMusic()
    {
        List<string> keys = new List<string>(LoopMusicDic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            LoopMusicDic[keys[i]].Stop();
            LoopMusicDic[keys[i]].loop = false;
        }
        LoopMusicDic = new Dictionary<string, AudioSource>();
    }
    static AudioSource GetApplicableSoundSource()
    {
        CurPlaySound = null;
        for (int i = 0; i < SoundList.Count; i++)
        {
            if (!SoundList[i].isPlaying)
            {
                CurPlaySound = SoundList[i];
                return CurPlaySound;
            }
        }
        return CurPlaySound;
    }
    static AudioSource GetNewSoundSource()
    {
        CurPlaySound = MySoundObject.AddComponent<AudioSource>();
        SoundList.Add(CurPlaySound);
        return CurPlaySound;
    }
    static AudioSource GetApplicableMusicSource()
    {
        CurPlayMusic = null;
        for (int i = 0; i < MusicList.Count; i++)
        {
            if (!MusicList[i].isPlaying)
            {
                CurPlayMusic = MusicList[i];
                return CurPlayMusic;
            }
        }
        return CurPlayMusic;
    }
    static AudioSource GetNewMusicSource()
    {
        CurPlayMusic = MyMusicObject.AddComponent<AudioSource>();
        MusicList.Add(CurPlayMusic);
        return CurPlayMusic;
    }
}
