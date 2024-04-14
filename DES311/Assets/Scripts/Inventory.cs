using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IPurchaseListener
{
    // List to hold all available weapons
    private List<Weapon> weapons;

    // Index of the currently equipped weapon
    private int currentWeaponIndex = 0;

    public Transform defaultSlot;
    public Weapon[] weaponPrefabs;

    void Start()
    {
        InitWeapon();
    }

    // Initialise weapons by equipping each weapon prefab
    void InitWeapon()
    {
        weapons = new List<Weapon>();
        foreach (Weapon weapon in weaponPrefabs)
        {
            EquipNewWeapon(weapon);
        }
    }

    // Equip a new weapon
    void EquipNewWeapon(Weapon weapon)
    {
        // Find the appropriate weapon slot for the new weapon
        Transform weaponSlot = defaultSlot;
        foreach (Transform slot in weaponSlot)
        {
            if (slot.gameObject.tag == weapon.GetSlotTag())
            {
                weaponSlot = slot;
            }
        }

        // Instantiate the new weapon and initialise it
        Weapon newWeapon = Instantiate(weapon, weaponSlot);
        newWeapon.Init(gameObject);
        weapons.Add(newWeapon);
    }

    // Switch to the next weapon in the list
    public void NextWeapon()
    {
        int nextWeaponIndex = currentWeaponIndex + 1;
        if (nextWeaponIndex >= weapons.Count)
        {
            nextWeaponIndex = 0;
        }
        EquipWeapon(nextWeaponIndex);
    }

    // Equip a specific weapon by index
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

    // Handle the purchase of a new weapon
    public bool HandlePurchase(Object newPurchase)
    {
        GameObject itemAsGameObject = newPurchase as GameObject;

        if (itemAsGameObject == null) { return false; }

        Weapon itemsAsWeapon = itemAsGameObject.GetComponent<Weapon>();

        if (itemsAsWeapon == null) { return false; }

        EquipNewWeapon(itemsAsWeapon);
        return true;
    }
}
