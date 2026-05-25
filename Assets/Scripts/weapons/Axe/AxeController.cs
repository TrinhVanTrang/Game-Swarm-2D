using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AxeController : WeaponController
{
    float dirX = 1;

    void FixedUpdate()
    {
        RunLogic();
    }
    public override void RunLogic()
    {
        if (firePoint == null) return;
        float moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0) dirX = moveX;
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            timer = 0;
            StartCoroutine(FireSequence());
        }
    }

    IEnumerator FireSequence()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = firePoint.position;
            if (GameController.instance.IsInsideTheMap(spawnPosition))
            {
                float v0 = baseSpeed * speedMultiplier * 2.5f;
                float angle = 90 - i * 15 * dirX;
                SpawnKnife(spawnPosition, v0, angle);
            }
            // Projectile Interval quy định độ trễ giữa các con dao trong 1 nhóm
            yield return new WaitForSeconds(projectileInterval);

        }
    }

    void SpawnKnife(Vector3 pos, float v0, float angle)
    {
        // GameObject bulletGO = Instantiate(knifePrefab, pos, Quaternion.identity);
        GameObject bulletGO = BulletPoolManager.instance.GetBullet(weaponData);
        if (bulletGO == null) return;
        AxeBullet proj = bulletGO.GetComponent<AxeBullet>();
        bulletGO.transform.localScale = baseTransform.localScale;
        bulletGO.transform.position = pos;
        bulletGO.transform.localScale *= areaMultiplier;

        // Truyền các chỉ số vào viên dao
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

        proj.alpha = angle * Mathf.Deg2Rad;
        proj.v0 = v0;
        proj.p0 = firePoint.position;
        proj.timeAlive = 0;

        proj.StartTravel();
    }

    public override void Start()
    {
        base.Start();
    }
}
