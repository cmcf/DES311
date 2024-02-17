using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Weapon[] weaponPrefabs;
    [SerializeField] Transform[] weaponSlots;
    [SerializeField] Transform defaultSlot;

    List<Weapon> weapons;

    int currentWeaponIndex = -1;

     void Start()
     {
        InitWeapon();
     }

    void InitWeapon()
    {
        weapons = new List<Weapon>();   
        foreach (Weapon weapon in weaponPrefabs)
        {
            Transform weaponSlot = defaultSlot;
            foreach (Transform slot in weaponSlot)
            {
                if (slot.gameObject.tag == weapon.GetSlotTag())
                {
                    weaponSlot = slot;
                }
            }
            Weapon newWeapon = Instantiate(weapon, weaponSlot);
            newWeapon.Init(gameObject);
            weapons.Add(newWeapon);
        }
        
    }

   public void NextWeapon()
   {
        int nextWeaponIndex = currentWeaponIndex + 1;
        if (nextWeaponIndex >= weapons.Count)
        {
            nextWeaponIndex = 0;
        }

        EquipWeapon(nextWeaponIndex);
   }

    void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count) { return; }    

        if (currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count)
        {
            weapons[currentWeaponIndex].UnEquip();
        }

        weapons[weaponIndex].Equip();
        currentWeaponIndex = weaponIndex;
    }
}
