// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class LvlManager : MonoBehaviour
// {
//     public int level;
//     public int currentExp;
//     public int expToLevel = 10;
//     public float expGrowthMultiplier = 1.2f;
//     public Slider expSlider;
//     public TMP_Text currentLevelText;

//     private void Start() 
//     {
//         UpdateUI();
//     }

//     private void Update() 
//     {
//         if(Input.GetKeyDown(KeyCode.Return))
//         {
//             GainExperience(2);
//         }
//     }

//     private void OnEnable()
//     {
//         EnemyHealth.OnMonsterDefeated += GainExperience;
//     }

//     private void OnDisable()
//     {
//         EnemyHealth.OnMonsterDefeated -= GainExperience;
//     }

//     public void GainExperience(int amount)
//     {
//        currentExp += amount;
//        if(currentExp >= expToLevel)
//        {
//         LevelUp();
//        }

//        UpdateUI(); 
//     }

//     private void LevelUp()
//     {
//         level++;
//         currentExp -= expToLevel;
//         expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier);
//     }

//     private void UpdateUI() 
//     {
//         expSlider.maxValue = expToLevel;
//         expSlider.value = currentExp;
//         currentLevelText.text = "Level : " + level;
//     }

// }
