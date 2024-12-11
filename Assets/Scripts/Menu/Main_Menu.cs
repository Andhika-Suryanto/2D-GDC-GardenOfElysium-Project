using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject victoryPanel;
    public void PlayGame()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Spawn Forest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        victoryPanel.SetActive(false);
        mainMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void InPanel()
    {
        panelMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void outBack()
    {
        panelMenu.SetActive(false);
        Time.timeScale = 1;
    }
}