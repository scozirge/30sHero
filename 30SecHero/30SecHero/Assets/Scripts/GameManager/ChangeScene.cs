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
        SceneManager.LoadScene(_scene);
    }
    public static void RestartGame()
    {
        GoToScene(MyScene.Init);
    }
}
