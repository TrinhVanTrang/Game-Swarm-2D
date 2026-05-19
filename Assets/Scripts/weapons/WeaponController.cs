using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public abstract class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    public static WeaponController instance;
    [Header("Stats")]
    public int currentLevel;
    public int maxLevel;
    public float damage;
    public int amount;
    public int pierce;
    public float cooldown;
    public float projectileInterval;
    public float baseSpeed;  // base speed
    public float speed;  // speed multiplier (%), currentSpeed = baseSpeed * speed
    public float bulletHitBoxRadius;  // base radius 
    public float area = 1f;    // area multiplier, currentArea = baseArea * area
    public float knockBack;
    public Queue<GameObject> pool = new Queue<GameObject>();
    [Header("References")]
    public Transform firePoint; // Thường là vị trí của Player

    public Vector2 lastMoveDir = Vector2.right;
    protected float timer;

    public PlayerStatsManager statsManager;

    public abstract void RunLogic();

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        Debug.Log("Level up to lv" + currentLevel + 1 + "!");
        currentLevel++;
        UpdateStats();
    }

    public void UpdateStats()
    {
        WeaponLevelStats weaponLevelStats = weaponData.levelStats[currentLevel - 1];
        damage = weaponLevelStats.damage;
        speed = weaponLevelStats.speed + statsManager.GetProjectileSpeedMultiplier() - 1;
        area = weaponLevelStats.area;
        amount = weaponLevelStats.amount;
        pierce = weaponLevelStats.pierce;
        cooldown = weaponLevelStats.cooldown;
        projectileInterval = weaponLevelStats.projectileInterval;
        Debug.Log(weaponLevelStats.upgradeDescription);
    }

    void Start()
    {
        statsManager = FindObjectOfType<PlayerStatsManager>();
        BulletPoolManager.instance.CreatePool(weaponData);
        currentLevel = 1;
        UpdateStats();
    }

    void Awake()
    {
        if (this != null) instance = this;
    }

}
