using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Myself;
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
    static Dictionary<string, Coroutine> MusicFadeDic;
    void Awake()
    {
        if (!IsInit)
        {
            Myself = this;
            Init();
        }
    }
    static void Init()
    {
        LoopSoundDic = new Dictionary<string, AudioSource>();
        LoopMusicDic = new Dictionary<string, AudioSource>();
        MusicFadeDic = new Dictionary<string, Coroutine>();
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
    public static void FadeOutMusic(string _key, float _fadeTime)
    {
        if (!Myself)
            return;
        if (!LoopMusicDic.ContainsKey(_key))
        {
            return;
        }
        if(MusicFadeDic.ContainsKey(_key))
        {
            Myself.StopCoroutine(MusicFadeDic[_key]);
            MusicFadeDic.Remove(_key);
        }
        Coroutine c= Myself.StartCoroutine(Myself.FadeOut(_key, _fadeTime));
        MusicFadeDic.Add(_key, c);
    }
    public static void FadeInMusic(AudioClip _ac, string _key, float _fadeTime)
    {
        if (!Myself)
            return;
        if (LoopMusicDic.ContainsKey(_key))
        {
            Myself.StopLoopMusic(_key);
        }
        if (MusicFadeDic.ContainsKey(_key))
        {
            Myself.StopCoroutine(MusicFadeDic[_key]);
            MusicFadeDic.Remove(_key);
        }
        Coroutine c = Myself.StartCoroutine(Myself.FadeIn(_ac, _key, _fadeTime));
        MusicFadeDic.Add(_key, c);
    }
    IEnumerator FadeOut(string _key, float _fadeTime)
    {
        if (LoopMusicDic.ContainsKey(_key))
        {
            float startVolume = LoopMusicDic[_key].volume;
            while (LoopMusicDic.ContainsKey(_key) && LoopMusicDic[_key].volume > 0)
            {
                LoopMusicDic[_key].volume -= startVolume * Time.deltaTime / _fadeTime;
                if (LoopMusicDic[_key].volume <= 0)
                {
                    StopLoopMusic(_key);
                    break;
                }
                yield return null;
            }
        }
    }
    IEnumerator FadeIn(AudioClip _ac, string _key, float _fadeTime)
    {
        if (PlayLoopMusic_Static(_ac, _key) != null)
        {
            LoopMusicDic[_key].Play();
            LoopMusicDic[_key].volume = 0f;
            while (LoopMusicDic.ContainsKey(_key) && LoopMusicDic[_key].volume < 1)
            {
                LoopMusicDic[_key].volume += Time.deltaTime / _fadeTime;
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
        if (IsMusicMute)
        {
            List<string> keys = new List<string>(LoopMusicDic.Keys);
            foreach (string key in keys)
            {
                LoopMusicDic[key].volume = 0;
            }
        }
        else
        {
            List<string> keys = new List<string>(LoopMusicDic.Keys);
            foreach (string key in keys)
            {
                LoopMusicDic[key].volume = 1;
            }
        }
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
        CurPlaySound.volume = 1;
        CurPlaySound.Play();
        LoopSoundDic.Add(_key, CurPlaySound);
        if (IsMusicMute)
        {
            LoopSoundDic[_key].volume = 0;
        }
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
        if (LoopMusicDic.ContainsKey(_key))
        {
            Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return;
        }
        if (!IsInit)
            Init();
        if (GetApplicableMusicSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.clip = _ac;
        CurPlayMusic.loop = true;
        CurPlayMusic.volume = 1;
        CurPlayMusic.Play();
        LoopMusicDic.Add(_key, CurPlayMusic);
        if (IsMusicMute)
        {
            LoopSoundDic[_key].volume = 0;
        }
    }
    public static AudioSource PlayLoopMusic_Static(AudioClip _ac, string _key)
    {
        if (_ac == null)
        {
            Debug.LogWarning("要播放的音檔為null");
            return null;
        }
        if (LoopMusicDic.ContainsKey(_key))
        {
            Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return null;
        }
        if (!IsInit)
            Init();
        if (GetApplicableMusicSource() == null)
        {
            GetNewMusicSource();
        }
        CurPlayMusic.clip = _ac;
        CurPlayMusic.loop = true;
        CurPlayMusic.volume = 1;
        CurPlayMusic.Play();
        LoopMusicDic.Add(_key, CurPlayMusic);
        if (IsMusicMute)
        {
            LoopSoundDic[_key].volume = 0;
        }
        return LoopMusicDic[_key];
    }

    public void StopLoopMusic(string _key)
    {
        if (LoopMusicDic.ContainsKey(_key))
        {
            LoopMusicDic[_key].volume = 1;
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
