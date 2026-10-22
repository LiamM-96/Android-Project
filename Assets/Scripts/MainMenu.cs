using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "HorrorGame";

    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
