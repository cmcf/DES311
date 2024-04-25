using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    // Reference to the playerLoadout ScriptableObject
    public WeaponItem playerLoadout;

    void Awake()
    {
        // Ensure this GameObject persists between scenes
        DontDestroyOnLoad(gameObject);
    }
}
