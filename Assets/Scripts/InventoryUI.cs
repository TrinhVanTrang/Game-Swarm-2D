using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public static InventoryUI instance;
    [Header("Grid Containers")]
    [SerializeField] private Transform weaponGridTransform;
    [SerializeField] private Transform passiveGridTransform;

    [Header("Prefabs")]
    [SerializeField] private GameObject itemSlotPrefab;
    private List<GameObject> spawnedWeapons = new List<GameObject>();
    private List<GameObject> spawnedPassives = new List<GameObject>();

    public void UpdateWeaponUI(List<Sprite> weaponIcons)
    {
        Debug.Log(spawnedWeapons.Count + " " + spawnedPassives.Count);
        foreach (var slot in spawnedWeapons) Destroy(slot);
        spawnedWeapons.Clear();

        foreach (Sprite icon in weaponIcons)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, weaponGridTransform);
            Image slotImage = newSlot.GetComponentInChildren<Image>();

            if (slotImage != null)
            {
                slotImage.sprite = icon;
            }
            spawnedWeapons.Add(newSlot);
        }
    }
    public void UpdatePassiveUI(List<Sprite> passiveIcons)
    {
        Debug.Log(spawnedWeapons.Count + " " + spawnedPassives.Count);
        foreach (var slot in spawnedPassives) Destroy(slot);
        spawnedPassives.Clear();

        foreach (Sprite icon in passiveIcons)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, passiveGridTransform);
            Image slotImage = newSlot.GetComponentInChildren<Image>();

            if (slotImage != null)
            {
                slotImage.sprite = icon;
            }
            spawnedPassives.Add(newSlot);
        }
    }

    void Awake()
    {
        if (this != null) instance = this;
    }
}