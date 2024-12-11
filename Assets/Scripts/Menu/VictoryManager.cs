using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    
    private void Start()
    {
        // Pastikan victory panel tidak aktif di awal
        victoryPanel.SetActive(false);
    }

    public void ShowVictoryScene()
    {
        // Hentikan gameplay
        Time.timeScale = 0f;
        
        // Aktifkan victory panel
        victoryPanel.SetActive(true);
    }
}