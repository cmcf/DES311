using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public TextMeshProUGUI currentEnemiesKilled;
    public TextMeshProUGUI currentCoins;
    void Start()
    {
        currentEnemiesKilled.text = "Enemies Defeated: " + GameManager.instance.currentEnemiesKilled.ToString();
    }

    public void UpdatePointsText()
    {
        currentEnemiesKilled.text = "Enemies Defeated: " + GameManager.instance.currentEnemiesKilled.ToString();
        currentCoins.text = "Credits Earned: " + GameManager.instance.currentCredits.ToString();
    }

}
