using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
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
        if (Player.Tutorial)
            NextSceneName = "Battle";
        else
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
        if (KongregateAPIBehaviour.EndLogin)
        {
            //SceneManager.LoadScene(NextSceneName);
            PopupUI.CallCutScene(NextSceneName);
            NextSceneName = "";
        }
    }
    public void PlaySceneMusic()
    {
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
}
