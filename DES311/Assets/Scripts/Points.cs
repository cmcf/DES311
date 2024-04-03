using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public TextMeshProUGUI currentPointsText;
    void Start()
    {
        currentPointsText.text = "Points earned: " + GameManager.instance.currentPoints.ToString();
    }


}
