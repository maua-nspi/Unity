using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyChangeScene : MonoBehaviour
{
    //restart button
    public void RestartScene(string sceneName){

        SceneManager.LoadScene(sceneName);
    }

    //quit button
    public void Exit()
    {
        Application.Quit();
    }
}


