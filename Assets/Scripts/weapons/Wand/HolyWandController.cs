using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class HolyWandController : WeaponController
{
    private float baseAttackRadius = 8f;
    private float maxRadius = 8f;
    private bool isAttacking = false;
    void FixedUpdate()
    {
        RunLogic();
    }
    public override void RunLogic()
    {
        if (firePoint == null) return;
        //UpdateDirection();
        Transform target = GetClosestEnemy();
        if (target != null) {
            Debug.Log("Found closest enemy: "+target.position.x+" "+target.position.y);
            lastMoveDir = target.position - firePoint.position;
            isAttacking = true;
        } else
        {
            isAttacking = false;
        }
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            if (isAttacking == true)
            {
                timer = 0;
                StartCoroutine(FireSequence());
            }

        }
    }


    Transform GetClosestEnemy()
    {
        int spatialGroup = GameController.instance.GetSpatialGroupStatic(firePoint.position.x, firePoint.position.y);
        int widthRange = GameController.instance.SpatialGroupWidth; // ex. 100
        int heightRange = GameController.instance.SpatialGroupHeight; // ex. 100
        int numberOfPartitions = GameController.instance.NumberOfPartitions;
        float groupWidth = GameController.instance.CELL_WIDTH_STATIC;
        float groupHeight = GameController.instance.CELL_HEIGHT_STATIC;
        //Debug.Log(widthRange + " " + heightRange + " " + groupWidth + " " + groupHeight + " " + numberOfPartitions);
        int maxCellRadius = Mathf.CeilToInt((float)maxRadius / Mathf.Min(groupWidth, groupHeight));
        Vector2 targetPos = (Vector2)firePoint.position;

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        for (int r = 0; r <= maxCellRadius; r++)
        {
            // Duyệt qua các ô nằm trên VIỀN của hình vuông bán kính r
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (Mathf.Abs(x) != r && Mathf.Abs(y) != r) continue;
                    int group = spatialGroup + x + y * widthRange;
                    HashSet<Enemy> cellEnemies = GameController.instance.enemySpatialGroups[group];
                    if (cellEnemies != null)
                    {
                        foreach (Enemy enemy in cellEnemies)
                        {
                            if (enemy == null) continue;

                            float dist = Vector2.Distance(targetPos, enemy.transform.position);

                            // Cập nhật kẻ địch gần nhất trong vùng đang xét
                            if (dist < maxRadius && dist < closestDistance)
                            {
                                closestDistance = dist;
                                closestEnemy = enemy.transform;
                            }
                        }
                    } 

                }
            }

            if (closestEnemy != null)
            {
                if (Vector2.Distance(targetPos, closestEnemy.position) <= maxRadius)
                {
                    break;
                }
            }
        }
        return closestEnemy;
    }

    IEnumerator FireSequence()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = firePoint.position;
            if (GameController.instance.IsInsideTheMap(spawnPosition))
            {
                SpawnKnife(spawnPosition);
            }
            // Projectile Interval quy định độ trễ giữa các bullet trong 1 nhóm
            yield return new WaitForSeconds(projectileInterval);

        }
    }

    void SpawnKnife(Vector3 pos)
    {
        // GameObject bulletGO = Instantiate(knifePrefab, pos, Quaternion.identity);
        GameObject bulletGO = BulletPoolManager.instance.GetBullet(weaponData);
        if (bulletGO == null) return;
        HolyWandBullet proj = bulletGO.GetComponent<HolyWandBullet>();
        bulletGO.transform.localScale = baseTransform.localScale;
        bulletGO.transform.position = pos;
        bulletGO.transform.localScale *= areaMultiplier;
        

        // Truyền các chỉ số vào viên đạn
        //proj.Setup(damage, pierce, speed, lastMoveDir);
        proj.poolName = weaponData.poolName;
        proj.bulletDamage = baseDamage;
        proj.movementSpeed = baseSpeed * speedMultiplier;
        proj.bulletHitBoxRadius = bulletHitBoxRadius * areaMultiplier;
        proj.MovementDirection = lastMoveDir;
        proj.modelTransform = bulletGO.transform;
        proj.knockBack = knockBack;
        proj.pierce = pierce;
        proj.area = areaMultiplier;

        proj.StartTravel();
    }

    public override void Start()
    {
        base.Start();
    }
}
