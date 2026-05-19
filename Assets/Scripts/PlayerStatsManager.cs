using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    // Tham chiếu đến Inventory (như đã thiết kế ở bài trước)
    public InventoryManager inventory;

    // Chỉ số gốc của nhân vật (Base Stats)
    public float baseHealth = 100f;
    public float baseMoveSpeed = 5f;
    public float baseDamageMultiplier = 1f; // 100%
    public float baseCooldownMultiplier = 1f; // 100%
    public float baseProjectileSpeedMultiplier = 1f; // 100%

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
                // Mảng bắt đầu từ 0, nên cần trừ 1
                totalModifier += passive.levelStats[currentLevel - 1].statValue;
            }
        }
        return totalModifier;
    }

    // Các hàm Helper để các script khác dễ dàng gọi
    public float GetCurrentHealth()
    {
        return baseHealth * (1 + GetTotalStatModifier(StatType.MaxHealth));
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
        // Cooldown càng giảm càng tốt, nên ta TRỪ đi modifier
        // Ví dụ: Empty Tome tăng 8% giảm hồi chiêu -> 1 - 0.08 = 0.92 (92% thời gian gốc)
        float cdr = GetTotalStatModifier(StatType.Cooldown);
        return (float)Mathf.Clamp(baseCooldownMultiplier - cdr, 0.1f, 1f); // Giới hạn tối đa giảm 90%
    }

    public float GetProjectileSpeedMultiplier()
    {
        return baseProjectileSpeedMultiplier + GetTotalStatModifier(StatType.ProjectileSpeed);
    }
}
