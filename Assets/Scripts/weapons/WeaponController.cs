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
    public float damageMultiplier;
    public float baseDamage;
    public int amount;
    public int pierce;
    public float cooldown;
    public float projectileInterval;
    public float baseSpeed;  // base speed
    public float speedMultiplier;  // speed multiplier (%), currentSpeed = baseSpeed * speed
    public float bulletHitBoxRadius;  // base radius 
    public float areaMultiplier = 1f;    // area multiplier, currentArea = baseArea * area
    public float knockBack;
    [Header("References")]
    public Transform firePoint; // Thường là vị trí của Player

    public Vector2 lastMoveDir = Vector2.right;
    protected float timer;
    protected Transform baseTransform;

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
        baseDamage = weaponLevelStats.damage;
        speedMultiplier = 1 + weaponLevelStats.speed - 1 + statsManager.GetProjectileSpeedMultiplier() - 1;
        areaMultiplier = 1 + (weaponLevelStats.area - 1) + (statsManager.GetAreaMultiplier() - 1);
        amount = weaponLevelStats.amount;
        pierce = weaponLevelStats.pierce;
        cooldown = weaponLevelStats.cooldown * statsManager.GetCooldownMultiplier();
        projectileInterval = weaponLevelStats.projectileInterval ;
        Debug.Log(weaponLevelStats.upgradeDescription);
    }

    public virtual void Start()
    {
        statsManager = FindObjectOfType<PlayerStatsManager>();
        GameObject obj = BulletPoolManager.instance.CreatePool(weaponData);
        baseTransform = obj.transform;

        knockBack = weaponData.knockBack;
        baseSpeed = weaponData.baseSpeed;
        bulletHitBoxRadius = weaponData.bulletHitBoxRadius;  // base area
        //Destroy(obj);

        currentLevel = 1;
        UpdateStats();
    }

    void Awake()
    {
        if (this != null) instance = this;
    }

}
