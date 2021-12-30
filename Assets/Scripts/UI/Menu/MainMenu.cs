using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //method to open Main-scene (Schieﬂstand)
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }

    //method to close game (only in built version)
    public void QuitGame()
    {
        Application.Quit();
    }
}
