using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip DefaultMusic;
    public void Start()
    {
        GameConfig.Music = DefaultMusic;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("quiting");
        Application.Quit();
    }
}
