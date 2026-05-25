using UnityEngine;

public enum StatType
{
    MaxHealth,      // Máu tối đa
    Armor,          // Giáp
    MoveSpeed,      // Tốc độ di chuyển
    Might,          // % Sát thương cộng thêm
    Cooldown,       // % Giảm thời gian hồi chiêu
    ProjectileSpeed,      // Tốc độ bay của đạn
    Area,           // % Tăng kích thước vũ khí
    Amount,         // Số lượng đạn cộng thêm
    Luck,            // May mắn
    Coin, 
    CurrentHealth,
    Curse              // Lời nguyền tăng máu và tốc độ của enemy
}

public enum ItemType
{
    Weapon,
    Passive
}
public class ItemData
{
    public ItemType itemType;
    public string name;
    public int level;
    public string description;
    public Sprite icon;
    public ScriptableObject rawData;

    public ItemData(ItemType itemType, string name, int level, string description, Sprite icon, ScriptableObject rawData)
    {
        this.itemType = itemType;
        this.name = name;
        this.level = level;
        this.description = description;
        this.icon = icon;
        this.rawData = rawData;
    }
}
