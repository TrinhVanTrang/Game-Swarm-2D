using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;
    public LevelUpManager levelUpManager;
    public Transform player;
    CharacterController playerScript;

    public Transform firePoint;
    public WeaponData firstWeapon;
    // Lưu trữ vũ khí hiện tại và cấp độ của chúng
    public Dictionary<WeaponData, int> currentWeapons = new Dictionary<WeaponData, int>();

    public Dictionary<PassiveData, int> currentPassives = new Dictionary<PassiveData, int>();

    List<WeaponController> weapons = new List<WeaponController>();


    public void AddNewWeapon(WeaponData newWeapon)
    {
        GameObject weapon = Instantiate(newWeapon.controller, player);
        WeaponController weaponController = weapon.GetComponent<WeaponController>();
        weaponController.weaponData = newWeapon;
        weaponController.firePoint = player;
        weapon.SetActive(true);
        weapons.Add(weaponController);
        UpdateWeaponUI();

    }

    public void UpdateWeaponUI()
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (KeyValuePair<WeaponData, int> weapon in currentWeapons)
        {
            Debug.Log(weapon.Key.weaponName + " " + weapon.Value);
            sprites.Add(weapon.Key.icon);
        }
        InventoryUI.instance.UpdateWeaponUI(sprites);
    }
    public void UpdatePassiveUI()
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (KeyValuePair<PassiveData, int> passive in currentPassives)
        {
            sprites.Add(passive.Key.icon);
        }
        InventoryUI.instance.UpdatePassiveUI(sprites);
    }

   


    public ItemData CreateItemDataFromWeapon(WeaponData weaponData, int level)
    {
        return new ItemData(ItemType.Weapon, weaponData.weaponName, level,
        weaponData.levelStats[level - 1].upgradeDescription, weaponData.icon, weaponData);
    }

    public ItemData CreateItemDataFromPassive(PassiveData passiveData, int level)
    {
        return new ItemData(ItemType.Passive, passiveData.passiveName, level,
                        passiveData.levelStats[level - 1].description, passiveData.icon, passiveData);
    }

    void Aware()
    {
        if (this != null) instance = this;
    }

    void Start()
    {
        playerScript = player.GetComponent<CharacterController>();
        currentWeapons.Add(firstWeapon, 1);
        AddNewWeapon(firstWeapon);
        UpdatePassiveUI();

    }

    public void UpdateStats()
    {
        foreach (var weapon in weapons)
        {
            weapon.UpdateStats();
        }
        playerScript.UpdateStats();
    }

    public void UpdateStatsWeapon(WeaponData weaponData)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.weaponData.weaponName == weaponData.weaponName)
            {
                Debug.Log("Update weapon " + weaponData.weaponName);
                weapon.currentLevel = currentWeapons[weaponData];
                weapon.UpdateStats();
                return;
            }
        }

    }
}
