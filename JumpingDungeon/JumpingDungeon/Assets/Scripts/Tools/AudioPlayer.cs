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
    AudioSource CurPlaySound;
    AudioSource CurPlayMusic;

    void Awake()
    {
        if (!IsInit)
        {
            Init();
        }
    }
    void Init()
    {
        if (PlayerPrefs.GetInt("IsMute") == 0)
            MuteSound(false);
        else
            AudioPlayer.MuteSound(true);

        LoopSoundDic = new Dictionary<string, AudioSource>();
        LoopMusicDic = new Dictionary<string, AudioSource>();
        SoundList = new List<AudioSource>();
        MusicList = new List<AudioSource>();
        MySoundObject = new GameObject("SoundPlayer");
        MyMusicObject = new GameObject("MusicPlayer");
        DontDestroyOnLoad(MySoundObject);
        AudioSource mySound = MySoundObject.AddComponent<AudioSource>();
        AudioSource myMusic = MyMusicObject.AddComponent<AudioSource>();
        SoundList.Add(mySound);
        MusicList.Add(myMusic);
        CurPlaySound = mySound;
        CurPlayMusic = myMusic;
        IsInit = true;
    }
    public static void MuteSound(bool _isMute)
    {
        if (_isMute == IsSoundMute)
            return;
        IsSoundMute = _isMute;
    }
    public static void MuteMusic(bool _isMute)
    {
        if (_isMute == IsMusicMute)
            return;
        IsMusicMute = _isMute;
    }
    public void PlaySoundByString(string _soundName)
    {
        if (IsSoundMute)
            return;
        if (IsInit)
        {
            if (GetApplicableSoundSource() != null)
            {
                CurPlaySound.loop = false;
                CurPlaySound.clip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", _soundName));
                CurPlaySound.Play(0);
            }
            else
            {
                GetNewSoundSource();
                CurPlaySound.loop = false;
                CurPlaySound.clip = Resources.Load<AudioClip>(string.Format("Sounds/{0}", _soundName));
                CurPlaySound.Play(0);
            }
        }
        else
        {
            Init();
            CurPlaySound.loop = false;
            CurPlaySound.Play(0);
        }
    }
    public void PlaySoundByAudioClip(AudioClip _ac)
    {
        if (IsSoundMute)
            return;
        if (IsInit)
        {
            if (GetApplicableSoundSource() != null)
            {
                CurPlaySound.clip = _ac;
                CurPlaySound.loop = false;
                CurPlaySound.Play(0);
            }
            else
            {
                GetNewSoundSource();
                CurPlaySound.loop = false;
                CurPlaySound.clip = _ac;
                CurPlaySound.Play(0);
            }
        }
        else
        {
            Init();
            CurPlaySound.loop = false;
            CurPlaySound.Play(0);
        }
    }
    public void PlayMusicByString(string _MusicName)
    {
        if (IsMusicMute)
            return;
        if (IsInit)
        {
            if (GetApplicableMusicSource() != null)
            {
                CurPlayMusic.loop = false;
                CurPlayMusic.clip = Resources.Load<AudioClip>(string.Format("Musics/{0}", _MusicName));
                CurPlayMusic.Play(0);
            }
            else
            {
                GetNewMusicSource();
                CurPlayMusic.loop = false;
                CurPlayMusic.clip = Resources.Load<AudioClip>(string.Format("Musics/{0}", _MusicName));
                CurPlayMusic.Play(0);
            }
        }
        else
        {
            Init();
            CurPlayMusic.loop = false;
            CurPlayMusic.Play(0);
        }
    }
    public void PlayMusicByAudioClip(AudioClip _ac)
    {
        if (IsMusicMute)
            return;
        if (IsInit)
        {
            if (GetApplicableMusicSource() != null)
            {
                CurPlayMusic.clip = _ac;
                CurPlayMusic.loop = false;
                CurPlayMusic.Play(0);
            }
            else
            {
                GetNewMusicSource();
                CurPlayMusic.loop = false;
                CurPlayMusic.clip = _ac;
                CurPlayMusic.Play(0);
            }
        }
        else
        {
            Init();
            CurPlayMusic.loop = false;
            CurPlayMusic.Play(0);
        }
    }

    public void PlayLoopSound(AudioClip _ac, string _key)
    {
        if (IsSoundMute)
            return;
        if (LoopSoundDic.ContainsKey(_key))
        {
            //Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return;
        }
        if (IsInit)
        {
            if (GetApplicableSoundSource() != null)
            {
                CurPlaySound.loop = true;
                CurPlaySound.clip = _ac;
                CurPlaySound.Play();
            }
            else
            {
                GetNewSoundSource();
                CurPlaySound.loop = true;
                CurPlaySound.clip = _ac;
                CurPlaySound.Play();
            }
        }
        else
        {
            Init();
            CurPlaySound.loop = true;
            CurPlaySound.Play();
        }
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
        //else
        //Debug.LogWarning(string.Format("Key:{0}　不存在尋換播放音效清單中", _key));
    }
    public void PlayLoopMusic(AudioClip _ac, string _key)
    {
        if (IsMusicMute)
            return;
        if (LoopMusicDic.ContainsKey(_key))
        {
            //Debug.LogWarning(string.Format("Key:{0} 循環播放音效索引重複", _key));
            return;
        }
        if (IsInit)
        {
            if (GetApplicableMusicSource() != null)
            {
                CurPlayMusic.loop = true;
                CurPlayMusic.clip = _ac;
                CurPlayMusic.Play();
            }
            else
            {
                GetNewMusicSource();
                CurPlayMusic.loop = true;
                CurPlayMusic.clip = _ac;
                CurPlayMusic.Play();
            }
        }
        else
        {
            Init();
            CurPlayMusic.loop = true;
            CurPlayMusic.Play();
        }
        LoopMusicDic.Add(_key, CurPlayMusic);
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
    AudioSource GetApplicableSoundSource()
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
    AudioSource GetNewSoundSource()
    {
        CurPlaySound = MySoundObject.AddComponent<AudioSource>();
        SoundList.Add(CurPlaySound);
        return CurPlaySound;
    }
    AudioSource GetApplicableMusicSource()
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
    AudioSource GetNewMusicSource()
    {
        CurPlayMusic = MyMusicObject.AddComponent<AudioSource>();
        MusicList.Add(CurPlayMusic);
        return CurPlayMusic;
    }
}
