using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    public WeaponItem weaponItem;
    public TextMeshProUGUI projectileSpeedIncrease;

    void Start()
    {
       gameObject.SetActive(false);
    }

    public void DisplayItems()
    {
        gameObject.SetActive(true);
    }
}
