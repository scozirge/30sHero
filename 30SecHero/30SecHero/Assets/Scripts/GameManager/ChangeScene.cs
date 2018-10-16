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
    public void GoToScene(string _scene)
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
        if(KongregateAPIBehaviour.EndLogin)
            SceneManager.LoadScene(NextSceneName);
    }
}
