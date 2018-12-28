using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenePlayer : MonoBehaviour
{
    static string NextSceneName;
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
        if (SceneManager.GetActiveScene().name == MyScene.Init.ToString())
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
}
