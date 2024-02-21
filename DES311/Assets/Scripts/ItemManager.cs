using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemCards;
    public void DisplayItemChoice()
    {
        // Ensure there are items in the array
        if (itemCards.Length == 0)
        {
            Debug.LogWarning("No item cards assigned.");
            return;
        }

        // Choose a random index
        int randomIndex = Random.Range(0, itemCards.Length);

        // Activate the chosen item card
        itemCards[randomIndex].SetActive(true);

        // Deactivate all other item cards
        for (int i = 0; i < itemCards.Length; i++)
        {
            if (i != randomIndex)
            {
                itemCards[i].SetActive(false);
            }
        }
    }
}
