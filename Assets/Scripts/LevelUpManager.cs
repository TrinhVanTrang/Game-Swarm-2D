using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private UpgradeCard[] staticUiCards; // 4 thẻ UI tạo sẵn trên Hierarchy

    [Header("All Game Data Pools")]
    [SerializeField] private List<WeaponData> allWeaponsInGame;   // Kéo tất cả file SO Vũ khí vào đây
    [SerializeField] private List<PassiveData> allPassivesInGame; // Kéo tất cả file SO Nội tại vào đây

    public PassiveData floorChicken;
    public PassiveData bigCoinBag;

    public void TriggerLevelUp()
    {
        Time.timeScale = 0f; // Đóng băng game
        levelUpPanel.SetActive(true);

        // Ẩn toàn bộ 4 thẻ UI đi trước
        foreach (var card in staticUiCards) card.gameObject.SetActive(false);

        // 1. Gom tất cả nâng cấp hợp lệ vào một danh sách chung dạng object
        List<ItemData> validUpgrades = new List<ItemData>();

        // Lọc vũ khí: Nếu đã sở hữu và chưa max cấp, HOẶC chưa sở hữu nhưng còn slot trống
        foreach (var w in allWeaponsInGame)
        {
            Debug.Log("Amount weapon: " + inventoryManager.currentWeapons.Count);
            bool isOwned = inventoryManager.currentWeapons.ContainsKey(w);
            if (isOwned)
            {
                int level = inventoryManager.currentWeapons[w];
                if (level < w.maxLevel)
                {
                    validUpgrades.Add(inventoryManager.CreateItemDataFromWeapon(w, level+1));
                }
            }
            else
            {
                if (inventoryManager.currentWeapons.Count < 6)
                {
                    validUpgrades.Add(inventoryManager.CreateItemDataFromWeapon(w, 1));
                }
            }
        }

        // Lọc nội tại: Tương tự như vũ khí
        foreach (var p in allPassivesInGame)
        {
            bool isOwned = inventoryManager.currentPassives.ContainsKey(p);
            if (isOwned)
            {
                int level = inventoryManager.currentPassives[p];
                if (level < p.maxLevel)
                {
                    validUpgrades.Add(inventoryManager.CreateItemDataFromPassive(p, level+1));
                }
            }
            else
            {
                if (inventoryManager.currentWeapons.Count < 6)
                {
                    validUpgrades.Add(inventoryManager.CreateItemDataFromPassive(p, 1));
                }
            }
        }
        if (validUpgrades.Count == 0)
        {
            ItemData floorChickenData = floorChicken.CreateItemDataFromPassive();
            ItemData bigCoinBagData = bigCoinBag.CreateItemDataFromPassive();
            validUpgrades.Add(floorChickenData);
            validUpgrades.Add(bigCoinBagData);
        }

        // 2. Chọn ngẫu nhiên tối đa 4 món từ danh sách đã lọc
        List<ItemData> chosenUpgrades = PickRandomUpgrades(validUpgrades, staticUiCards.Length);

        // 3. Hiển thị lên các thẻ UI có sẵn
        for (int i = 0; i < chosenUpgrades.Count; i++)
        {
            staticUiCards[i].gameObject.SetActive(true);


            staticUiCards[i].SetupCard(chosenUpgrades[i], ApplyUpgrade);

        }
    }

    private List<ItemData> PickRandomUpgrades(List<ItemData> source, int count)
    {
        List<ItemData> pool = new List<ItemData>(source);
        List<ItemData> result = new List<ItemData>();
        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0) break;
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result;
    }

    // Hàm xử lý khi người chơi bấm chọn thẻ
    private void ApplyUpgrade(ItemData chosenData)
    {
        if (chosenData.itemType == ItemType.Weapon)
        {
            WeaponData weapon = (WeaponData)chosenData.rawData;
            // Nếu chưa có trong Inventory thì thêm vào
            if (!inventoryManager.currentWeapons.ContainsKey(weapon))
            {
                inventoryManager.currentWeapons.Add(weapon, 1);
                inventoryManager.AddNewWeapon(weapon);
            }
            else
            {
                inventoryManager.currentWeapons[weapon]++;
                inventoryManager.UpdateStatsWeapon(weapon);
                // Cập nhật chỉ số trực tiếp cho thực thể vũ khí ngoài đời thực
                //UpdateWeaponInGame(weapon);
            }


        }
        else if (chosenData.itemType == ItemType.Passive)
        {
            PassiveData passive = (PassiveData)chosenData.rawData;

            if (passive.name == "Floor Chicken")
            {
                GameController.instance.LifeSteal(passive.levelStats[0].statValue);
                return;
            }
            else if (passive.name == "Big Coin Bag")
            {
                GameController.instance.ModifyCoins((int)passive.levelStats[0].statValue);
                return;
            }
            if (!inventoryManager.currentPassives.ContainsKey(passive))
            {
                inventoryManager.currentPassives.Add(passive, 1);
                inventoryManager.UpdatePassiveUI();
            }
            else
            {
                inventoryManager.currentPassives[passive]++;
                // Cập nhật chỉ số nội tại trực tiếp cho nhân vật
                //UpdatePassiveInGame(passive);
            }
            inventoryManager.UpdateStats();
        }

        // Tắt bảng nâng cấp và tiếp tục game
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void UpdateWeaponInGame(WeaponData data)
    {
        
        //Debug.Log($"Vũ khí {data.itemName} lên cấp {data.currentLevel}. Sát thương mới: {data.levelStats[data.currentLevel - 1].damage}");
    }

    private void UpdatePassiveInGame(PassiveData data)
    {
    
        //Debug.Log($"Nội tại {data.passiveName} lên cấp {data.currentLevel}. Chỉ số cộng thêm: {data.levelStats[data.currentLevel - 1].armorBonus}");
    }
}
