using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    // Tham chiếu đến Inventory (như đã thiết kế ở bài trước)
    public InventoryManager inventory;

    // Chỉ số gốc của nhân vật (Base Stats)
    public float baseHealth = 100f;
    public float baseMoveSpeed = 4f;
    public float baseDamageMultiplier = 1f; // 100%
    public float baseCooldownMultiplier = 1f; // 100%
    public float baseProjectileSpeedMultiplier = 1f; // 100%
    public float baseArea = 1f;

    // Hàm lấy tổng chỉ số (Base + Passives)
    public float GetTotalStatModifier(StatType statType)
    {
        float totalModifier = 0f;

        // Duyệt qua tất cả passive người chơi đang có
        foreach (var kvp in inventory.currentPassives)
        {
            PassiveData passive = kvp.Key;
            int currentLevel = kvp.Value; // Cấp độ hiện tại (bắt đầu từ 1)

            if (passive.affectedStat == statType)
            {
                if (passive.isPercentage)
                {
                    // Mảng bắt đầu từ 0, nên cần trừ 1
                    totalModifier += passive.levelStats[currentLevel - 1].statValue;
                }
                else
                {
                    totalModifier = 1f * passive.levelStats[currentLevel - 1].statValue;
                }

            }
        }
        return totalModifier;
    }

    // Các hàm Helper để các script khác dễ dàng gọi
    public float GetCurrentHealth()
    {
        float maxHealthBuff = GetTotalStatModifier(StatType.MaxHealth);
        if (maxHealthBuff == 0) return baseHealth;
        return baseHealth * GetTotalStatModifier(StatType.MaxHealth);
    }

    public float GetCurrentMoveSpeed()
    {
        // Tốc độ di chuyển = Gốc * (1 + % Tăng tốc)
        return baseMoveSpeed * (1 + GetTotalStatModifier(StatType.MoveSpeed));
    }

    public float GetDamageMultiplier()
    {
        return baseDamageMultiplier + GetTotalStatModifier(StatType.Might);
    }

    public float GetCooldownMultiplier()
    {
        float result = baseCooldownMultiplier + GetTotalStatModifier(StatType.Cooldown);
        result = Mathf.Clamp(result, 0.1f, 0.9f);
        return result;
    }

    public float GetProjectileSpeedMultiplier()
    {
        return baseProjectileSpeedMultiplier + GetTotalStatModifier(StatType.ProjectileSpeed);
    }

    public float GetAreaMultiplier()
    {
        return baseArea + GetTotalStatModifier(StatType.Area);
    }
}
