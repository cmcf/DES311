using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IPurchaseListener
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
            EquipNewWeapon(weapon);
        }
        
    }

    void EquipNewWeapon(Weapon weapon)
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

    public bool HandlePurchase(Object newPurchase)
    {
       GameObject itemAsGameObject = newPurchase as GameObject;

        if (itemAsGameObject == null) { return false; }

        Weapon itemsAsWeapon = itemAsGameObject.GetComponent<Weapon>();

        if(itemsAsWeapon == null) { return false;  }

        EquipNewWeapon(itemsAsWeapon);
        return true;
    }
}
