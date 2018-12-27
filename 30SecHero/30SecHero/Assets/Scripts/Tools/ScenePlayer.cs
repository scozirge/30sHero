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
        NextSceneName = "";
    }
}
