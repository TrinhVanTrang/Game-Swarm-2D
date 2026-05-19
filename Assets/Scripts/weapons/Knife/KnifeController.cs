using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class KnifeController : WeaponController
{
    void FixedUpdate()
    {
        RunLogic();
    }

    public override void RunLogic()
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
        // GameObject bulletGO = Instantiate(knifePrefab, pos, Quaternion.identity);
        GameObject bulletGO = BulletPoolManager.instance.GetBullet(weaponData);
        if (bulletGO == null) return;
        KnifeBullet proj = bulletGO.GetComponent<KnifeBullet>();
        proj.baseTransform = bulletGO.transform;
        bulletGO.transform.position = pos;
        bulletGO.transform.localScale *= area;
    
        // Truyền các chỉ số vào viên dao
        //proj.Setup(damage, pierce, speed, lastMoveDir);
        proj.poolName = weaponData.poolName;
        proj.bulletDamage = damage;
        proj.movementSpeed = baseSpeed * speed;
        proj.bulletHitBoxRadius = bulletHitBoxRadius * area;
        proj.MovementDirection = lastMoveDir;
        proj.modelTransform = bulletGO.transform;
        proj.knockBack = knockBack;
        proj.pierce = pierce;
        proj.area = area;

        proj.startTravel();
    }
}