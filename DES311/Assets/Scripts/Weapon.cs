using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] string slotName;
    public string GetSlotTag()
    {
        return slotName;
    }
    public GameObject Player
    {
        get; private set;
    }

    public void Init(GameObject player)
    {
        Player = player;
    }

    public void Equip()
    {
        gameObject.SetActive(true);
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }
}
