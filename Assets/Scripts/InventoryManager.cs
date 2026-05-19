using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // Lưu trữ vũ khí hiện tại và cấp độ của chúng
    public Dictionary<WeaponData, int> currentWeapons = new Dictionary<WeaponData, int>();
    public Dictionary<PassiveData, int> currentPassives = new Dictionary<PassiveData, int>();

    public void AddOrUpgradeWeapon(WeaponData weapon)
    {
        if (currentWeapons.ContainsKey(weapon))
        {
            // Nếu đã có, tăng cấp độ (nhưng không vượt quá max level)
            int maxLevel = weapon.levelStats.Length;
            if (currentWeapons[weapon] < maxLevel)
            {
                currentWeapons[weapon]++;
            }
        }
        else
        {
            // Nếu chưa có, thêm mới ở cấp 1
            currentWeapons.Add(weapon, 1);
        }
    }

    // public void TryEvolveWeapon()
    // {
    //     WeaponData weaponToEvolve = null;

    //     foreach (var weapon in currentWeapons.Keys)
    //     {
    //         // Kiểm tra 1: Vũ khí đã max level chưa?
    //         bool isMaxLevel = currentWeapons[weapon] == weapon.levelStats.Length;

    //         // Kiểm tra 2: Có vũ khí tiến hóa không và có đồ nội tại yêu cầu chưa?
    //         bool hasPassive = currentPassives.ContainsKey(weapon.requiredPassive);

    //         if (isMaxLevel && weapon.evolutionWeapon != null && hasPassive)
    //         {
    //             weaponToEvolve = weapon;
    //             break; // Chỉ tiến hóa 1 món mỗi rương
    //         }
    //     }

    //     if (weaponToEvolve != null)
    //     {
    //         // Thêm vũ khí tiến hóa (Level 1)
    //         AddOrUpgradeWeapon(weaponToEvolve.evolutionWeapon);

    //         // Xóa vũ khí cũ khỏi Inventory và hệ thống bắn đạn
    //         currentWeapons.Remove(weaponToEvolve);
    //         RemoveWeaponSpawner(weaponToEvolve);
    //     }
    //     else
    //     {
    //         // Nếu không đủ điều kiện tiến hóa, rương sẽ rớt ra nâng cấp ngẫu nhiên hoặc tiền
    //         GiveRandomUpgradeOrGold();
    //     }
    // }
}
