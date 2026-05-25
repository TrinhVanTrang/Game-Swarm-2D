using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Knife : MonoBehaviour
{
    public static Knife instance;
    public WeaponData weaponData;
    //public int ProjectileCount = 0;

    [Header("Stats")]
    public int currentLevel = 1;
    public int maxLevel = 8;
    public float damage = 6.5f;
    public int amount = 1;
    public int pierce = 1;
    public float cooldown = 1.0f;
    public float projectileInterval = 0.1f;
    public float baseSpeed = 5.0f;  // base speed\
    public float speed = 1f;  // speed multiplier, currentSpeed = baseSpeed * speed
    public float bulletHitBoxRadius = 0.2f;  // base radius 
    public float area = 1f;    // area multiplier, currentArea = baseArea * area
    public float knockBack = 0.5f;
    [Header("References")]
    public GameObject knifePrefab;
    public Transform firePoint; // Thường là vị trí của Player

    private Vector2 lastMoveDir = Vector2.right;
    private float timer;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void FixedUpdate()
    {
        RunLogic();
    }

    public void RunLogic()
    {
        if (firePoint == null) return;
        UpdateDirection();

        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0;
            StartCoroutine(FireSequence());
        }
    }

    void UpdateDirection()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 currentInput = new Vector2(moveX, moveY);

        if (currentInput != Vector2.zero)
        {
            // Xử lý 8 hướng nếu dùng bàn phím
            if (IsKeyboardInput())
            {
                lastMoveDir = currentInput.normalized;
            }
            else // Gamepad/Chuột: Tự do hướng
            {
                lastMoveDir = currentInput.normalized;
            }
        }
    }

    bool IsKeyboardInput()
    {
        // Check đơn giản: nếu có nhấn các phím mũi tên/WASD
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
               Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
               Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
    }

    // IEnumerator FireSequence()
    // {
    //     for (int i = 0; i < amount; i++)
    //     {
    //         SpawnKnife();
    //         // Projectile Interval quy định độ trễ giữa các con dao trong 1 nhóm

    //         yield return new WaitForSeconds(projectileInterval);
    //         //Debug.Log(i + " " + Time.time);
    //     }
    // }

    IEnumerator FireSequence()
    {
        int currentBatchCount = Mathf.Min(amount, 3);
        for (int i = 0; i < amount; i++)
        {
            // Tính toán vector vuông góc để xếp hàng ngang
            Vector2 perpendicular = new Vector2(-lastMoveDir.y, lastMoveDir.x).normalized;
            float spacing = 0.2f; // Khoảng cách giữa các lưỡi dao

            // Tính toán độ lệch (offset) để nhóm 1, 2 hoặc 3 dao nằm cân bằng ở giữa
            // i = 0: bên trái, i = 1: ở giữa, i = 2: bên phải (nếu có 3 viên)
            int index = Random.Range(0, currentBatchCount);
            float offset = (index - (currentBatchCount - 1) / 2f) * spacing;
            Vector3 spawnPosition = firePoint.position + (Vector3)(perpendicular * offset);

            if (GameController.instance.IsInsideTheMap(spawnPosition))
            {
                SpawnKnife(spawnPosition);
            }
            // Projectile Interval quy định độ trễ giữa các con dao trong 1 nhóm
            yield return new WaitForSeconds(projectileInterval);

        }
    }

    void SpawnKnife(Vector3 pos)
    {
        //ProjectileCount++;
        GameObject knifeGO = Instantiate(knifePrefab, pos, Quaternion.identity);
        KnifeBullet proj = knifeGO.GetComponent<KnifeBullet>();

        // Truyền các chỉ số vào viên dao
        //proj.Setup(damage, pierce, speed, lastMoveDir);
        proj.bulletDamage = damage;
        proj.movementSpeed = baseSpeed * speed;
        proj.bulletHitBoxRadius = bulletHitBoxRadius * area;
        proj.MovementDirection = lastMoveDir;
        proj.modelTransform = knifeGO.transform;
        proj.knockBack = knockBack;
        proj.pierce = pierce;
    }

    public void LevelUp()
    {
        if (currentLevel == maxLevel) return;
        Debug.Log("Level up knife to lv" + currentLevel + 1 + "!");
        currentLevel++;
        UpdateStats();
    }

    void UpdateStats()
    {
        WeaponLevelStats weaponLevelStats = weaponData.levelStats[currentLevel - 1];
        damage = weaponLevelStats.damage;
        speed = weaponLevelStats.speed;
        area = weaponLevelStats.area;
        amount = weaponLevelStats.amount;
        pierce = weaponLevelStats.pierce;
        cooldown = weaponLevelStats.cooldown;
        projectileInterval = weaponLevelStats.projectileInterval;
        Debug.Log(weaponLevelStats.upgradeDescription);
    }

    void Awake()
    {
        instance = this;

    }

    void Start()
    {
        currentLevel = 1;
        UpdateStats();
    }
}
