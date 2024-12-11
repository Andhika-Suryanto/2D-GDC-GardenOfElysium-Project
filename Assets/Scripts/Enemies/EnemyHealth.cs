using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private bool isUrn = false; // Tambahkan flag untuk Urn
    
    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
    private VictoryManager victoryManager;

    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        victoryManager = FindObjectOfType<VictoryManager>();
    }

    private void Start() {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine() {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath() {
    if (currentHealth <= 0) {
        Debug.Log("Enemy Died. Is Urn: " + isUrn);
        Debug.Log("Victory Manager: " + (victoryManager != null));

        Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        GetComponent<PickUpSpawner>().DropItems();
        
        // Cek jika ini adalah Urn, tampilkan Victory Panel
        if (isUrn) {
            Debug.Log("Attempting to show victory scene");
            
            if (victoryManager == null) {
                victoryManager = FindObjectOfType<VictoryManager>();
                Debug.Log("Retry finding Victory Manager: " + (victoryManager != null));
            }

            if (victoryManager != null) {
                Debug.Log("Calling ShowVictoryScene()");
                victoryManager.ShowVictoryScene();
            } else {
                Debug.LogError("VictoryManager not found!");
            }
        }
        
        Destroy(gameObject);
    }
}
}