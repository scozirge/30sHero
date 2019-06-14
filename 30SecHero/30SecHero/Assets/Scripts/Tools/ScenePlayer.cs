using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenePlayer : MonoBehaviour
{
    static string NextSceneName;
    public Animator MyAni;
    [SerializeField]
    GameObject LoadingGO;
    public static void SetNextScene_static(string _sceneName)
    {
        NextSceneName = _sceneName;
    }
    public void SetNextScene(string _sceneName)
    {
        NextSceneName = _sceneName;
    }
    public void GoToNextScnee()
    {
        if (NextSceneName == "")
            return;
        SceneManager.LoadScene(NextSceneName);
        PlaySceneMusic();
        NextSceneName = "";
    }
    public void PlaySceneMusic()
    {
        if (SceneManager.GetActiveScene().name == MyScene.Init.ToString() && NextSceneName=="Main")
            return;
        AudioPlayer.StopAllMusic();
        switch (NextSceneName)
        {
            case "Battle":
                int rndNum = Random.Range(0, 2);
                if (rndNum == 0)
                    AudioPlayer.PlayLoopMusic_Static(GameManager.GM.FightMusic1, "Battle");
                else
                    AudioPlayer.PlayLoopMusic_Static(GameManager.GM.FightMusic2, "Battle");
                break;
            case "Main":
                AudioPlayer.PlayLoopMusic_Static(GameManager.GM.MainMusic, "Main");
                break;
        }
    }
    void Start()
    {
        LoadingGO.SetActive(false);
    }
    void Update()
    {
        if (KongregateAPIBehaviour.EndLogin && Player.IsInit)
        {
            if (Player.Tutorial)
                NextSceneName = "Battle";
            MyAni.enabled = true;
            LoadingGO.SetActive(false);
        }
    }
    public void LoadingFinishCheck()
    {
        if (!KongregateAPIBehaviour.EndLogin || !Player.IsInit)
        {
            MyAni.enabled = false;
            LoadingGO.SetActive(true);
            Player.ShowInitDataProgress();
        }
        if (Player.Tutorial)
            NextSceneName = "Battle";
    }
}
