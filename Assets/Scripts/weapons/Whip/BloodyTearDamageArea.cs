using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BloodyTearDamageArea : Bullet
{
    public float dirX;
    public Vector2 hitBoxSize; // Kích thước (base) vùng quất roi

    public override void RunLogic()
    {
        Invoke("DestroyBullet", 0.2f);
        CheckDamageAreaWithEnemy();
    }

    public void CheckDamageAreaWithEnemy()
    {

        // Tính toán tâm của vùng quét sát thương
        Vector2 strikeCenter = (Vector2)transform.position;
        float radiusCheckBox = Mathf.Sqrt(hitBoxSize.x * hitBoxSize.x + hitBoxSize.y * hitBoxSize.y) / 2 * area;
        // --- BƯỚC 1: XỬ LÝ LOGIC (KHÔNG DÙNG COLLIDER VẬT LÝ) 
        int spatialGroup = GameController.instance.GetSpatialGroupStatic(strikeCenter.x, strikeCenter.y);
        List<Enemy> enemies = Utils.GetAllEnemiesInSpatialGroups(
            Utils.GetExpandedSpatialGroupsV2(spatialGroup, Mathf.CeilToInt(radiusCheckBox)));
        if (enemies != null && enemies.Count > 0)
        {
            RestoreHealth();
        }
        foreach (var enemy in enemies)
        {
            if (IsBoxCollidingWithCircle((Vector2)enemy.transform.position,
                enemy.GetHitBoxRadius(), strikeCenter, hitBoxSize * area))
            {
                enemy.ApplyKnockbackEffect(knockBack, 1.0f);
                enemy.ChangeHealth(-bulletDamage);
            }

        }
        //if (!isDestroyed) DestroyBullet();
    }

    void RestoreHealth()
    {
        
    }

    bool IsBoxCollidingWithCircle(Vector2 circleCenter, float radius, Vector2 boxCenter, Vector2 boxSize)
    {
        // Bước 1: Tính toán các cạnh Biên (Min/Max) của hình chữ nhật vùng Roi
        float halfX = boxSize.x / 2f;
        float halfY = boxSize.y / 2f;

        float minX = boxCenter.x - halfX;
        float maxX = boxCenter.x + halfX;
        float minY = boxCenter.y - halfY;
        float maxY = boxCenter.y + halfY;

        // Bước 2: Tìm tọa độ điểm nằm trên hình chữ nhật GẦN TÂM vòng tròn nhất
        // Sử dụng Mathf.Clamp để "ép" tọa độ tâm quái vật vào phạm vi hình chữ nhật
        float closestX = Mathf.Clamp(circleCenter.x, minX, maxX);
        float closestY = Mathf.Clamp(circleCenter.y, minY, maxY);

        // Bước 3: Tính khoảng cách giữa điểm gần nhất đó và tâm vòng tròn
        float distX = circleCenter.x - closestX;
        float distY = circleCenter.y - closestY;

        // Tính bình phương khoảng cách (SqrMagnitude) để tối ưu hóa hiệu năng
        float distanceSquared = (distX * distX) + (distY * distY);

        // Nếu khoảng cách bình phương nhỏ hơn hoặc bằng bình phương bán kính -> VA CHẠM!
        return distanceSquared <= (radius * radius);
    }

    protected override void resetStats()
    {
        base.resetStats();
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3((float)scale.x / dirX, scale.y, scale.z);  // reset scale
    }
}
