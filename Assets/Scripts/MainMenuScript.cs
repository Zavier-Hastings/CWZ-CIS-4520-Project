using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Call to play
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Call to quit
    public void QuitGame ()
    {
        // Debug to show this works
        Debug.Log("Program Terminated");
        Application.Quit();
    }
}
