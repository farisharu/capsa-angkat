using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public void LoadScene(string sceneName, bool additive = false)
    {
        if(additive)
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(sceneName);
    }

    public void CloseScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        
        if(scene.IsValid())
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        else
        {
            Debug.LogWarning("Scene: " + sceneName + " is not valid");
        }
    }
}
