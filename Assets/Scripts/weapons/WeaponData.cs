using UnityEngine;

[System.Serializable]
public struct WeaponLevelStats
{
    public float damage;
    public float cooldown;
    public float projectileInterval;
    public float speed;
    public float area;
    public int amount; // Số lượng đạn bắn ra
    public int pierce;
    public string upgradeDescription; // Mô tả hiện lên UI khi chọn nâng cấp
}

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject prefab;  // prefab bullet
    public Sprite image;
    public Sprite icon;
    
    public bool isEvolutional;
    public int poolLimit;
    public string poolName;

    // Mảng này sẽ chứa dữ liệu từ Level 1 đến Max Level (thường là 8)
    public WeaponLevelStats[] levelStats;

    // Dữ liệu cho việc tiến hóa
    public WeaponData evolutionWeapon;
    public PassiveData requiredPassive;
}