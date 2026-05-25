using UnityEngine;

[System.Serializable]
public struct PassiveLevelStats
{
    public float statValue; // Ví dụ: 0.1 (tương đương 10%), hoặc 20 (cộng thẳng 20 máu)
    public string description;
}

[CreateAssetMenu(menuName = "ScriptableObjects/PassiveData")]
public class PassiveData : ScriptableObject
{

    public string passiveName;
    public Sprite image;
    public Sprite icon;

    public StatType affectedStat; // Vật phẩm này buff chỉ số nào?
    public bool isPercentage;     // Đây là chỉ số % hay cộng thẳng?

    // Dữ liệu từ cấp 1 đến cấp tối đa
    public PassiveLevelStats[] levelStats;

    public int maxLevel;
    public int currentLevel;

    public ItemData CreateItemDataFromPassive()
    {
        return new ItemData(ItemType.Passive, passiveName, currentLevel,
                        levelStats[currentLevel - 1].description, icon, this);
    }
}


