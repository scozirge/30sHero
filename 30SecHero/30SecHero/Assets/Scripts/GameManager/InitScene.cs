using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InitScene : MonoBehaviour
{
    [SerializeField]
    GameObject StartBtn;
    [SerializeField]
    GameObject SetUI;
    bool InitSetLanguage;
    void Start()
    {
        if (PlayerPrefs.GetInt(LocoData.InitSetLanguage.ToString()) == 0)
            InitSetLanguage = true;
        StartBtn.SetActive(false);
        SetUI.SetActive(false);
    }
    public static void GoToScene(MyScene _scene)
    {
        SceneManager.LoadScene(_scene.ToString());
    }
    public static void GoToScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
    public void SetNextScene(string _scene)
    {
        NextSceneName = _scene;
    }
    public static void RestartGame()
    {
        GoToScene(MyScene.Init);
    }
    string NextSceneName = "";
    void Update()
    {
        if (NextSceneName == "")
            return;
        //SceneManager.LoadScene(NextSceneName);
        PopupUI.CallCutScene(NextSceneName);
        NextSceneName = "";
    }
    public void PlaySceneMusic()
    {
        if (!InitSetLanguage)
            StartBtn.SetActive(true);
        if (SceneManager.GetActiveScene().name == MyScene.Battle.ToString())
        {
            int rndNum = Random.Range(0, 2);
            if (rndNum == 0)
                AudioPlayer.PlayLoopMusic_Static(GameManager.GM.FightMusic1, "Battle");
            else
                AudioPlayer.PlayLoopMusic_Static(GameManager.GM.FightMusic2, "Battle");
        }
        else if (SceneManager.GetActiveScene().name == MyScene.Main.ToString())
        {
            AudioPlayer.PlayLoopMusic_Static(GameManager.GM.MainMusic, "Main");
        }
        else if (SceneManager.GetActiveScene().name == MyScene.Init.ToString())
        {
            AudioPlayer.PlayLoopMusic_Static(GameManager.GM.MainMusic, "Main");
        }
    }
    public void SetLanguage(int _language)
    {
        StartBtn.SetActive(true);
        Player.SetLanguage((Language)_language);
    }
    public void CallSetUI(bool _show)
    {
        SetUI.SetActive(_show);
    }
    public void ShowSetUI()
    {
        CallSetUI(InitSetLanguage);
        InitSetLanguage = false;
        PlayerPrefs.SetInt(LocoData.InitSetLanguage.ToString(), 1);
    }
}
