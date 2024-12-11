using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenu; // Referensi ke Main Menu GameObject

    void Start()
    {
        // Tampilkan Main Menu saat game dimulai
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true); // Tampilkan Main Menu
        Time.timeScale = 0;
    }
}
