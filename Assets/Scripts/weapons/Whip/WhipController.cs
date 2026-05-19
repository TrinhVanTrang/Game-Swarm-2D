using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WhipController : WeaponController
{
    float dirX = 1f;
    public Vector2 hitBoxSize;
    void FixedUpdate()
    {
        RunLogic();
    }
    public override void RunLogic()
    {
        if (firePoint == null) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        if (moveX != 0) dirX = moveX;
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            if (moveX == 0 && moveY == 0)
            {
                timer = 0;
                StartCoroutine(FireSequence());
            }
        }
    }

    IEnumerator FireSequence()
    {
        Vector3 spawnPosition = firePoint.position + new Vector3(hitBoxSize.x / 2 * dirX * area, 0, 0);

        SpawnKnife(spawnPosition, dirX);

        // Projectile Interval quy định độ trễ giữa các bullet trong 1 nhóm
        if (amount >= 2)
        {
            yield return new WaitForSeconds(projectileInterval);
            spawnPosition = firePoint.position + new Vector3(-hitBoxSize.x / 2 * dirX * area, 0, 0);

            SpawnKnife(spawnPosition, -dirX);
        }
    }

    void SpawnKnife(Vector3 pos, float direct)
    {
        // GameObject bulletGO = Instantiate(knifePrefab, pos, Quaternion.identity);
        GameObject bulletGO = BulletPoolManager.instance.GetBullet(weaponData);
        if (bulletGO == null) return;

        WhipDamageArea proj = bulletGO.GetComponent<WhipDamageArea>();
        proj.baseTransform = bulletGO.transform;  // luu model nguyen ban
        bulletGO.transform.position = pos;
        bulletGO.transform.localScale *= area;    // scale theo pham vi
        // Truyền các chỉ số vào viên dao
        //proj.Setup(damage, pierce, speed, lastMoveDir);
        proj.poolName = weaponData.poolName;
        proj.bulletDamage = damage;
        proj.movementSpeed = baseSpeed * speed;
        proj.bulletHitBoxRadius = bulletHitBoxRadius * area;
        proj.MovementDirection = lastMoveDir;
        proj.modelTransform = bulletGO.transform;
        Vector3 currentScale = bulletGO.transform.localScale;
        bulletGO.transform.localScale = new Vector3(currentScale.x * direct, currentScale.y, currentScale.z);  // xoay theo dirX
        proj.knockBack = knockBack;
        proj.pierce = pierce;
        proj.area = area;
        proj.dirX = dirX;
        proj.hitBoxSize = hitBoxSize;
        //proj.enemyLayer = enemyLayer;
        proj.startTravel();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Vẽ hitbox phía trước
        Vector2 frontCenter = (Vector2)firePoint.position + new Vector2(dirX * hitBoxSize.x / 2 * area, 0f);
        Gizmos.DrawWireCube(frontCenter, hitBoxSize * area);

        // Vẽ hitbox phía sau nếu level >= 2
        if (amount >= 2)
        {
            Vector2 backCenter = (Vector2)firePoint.position + new Vector2(-dirX * hitBoxSize.x / 2 * area, 0f);
            Gizmos.DrawWireCube(backCenter, hitBoxSize * area);
        }
    }
}
