using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float baseHitBoxRadius = 0.175f;
    int batchId;
    public int BatchID
    {
        get { return batchId; }
        set { batchId = value; }
    }

    public float movementSpeed = 3f;
    Vector3 currentMovementDirection = Vector3.zero;
    public int spatialGroup = 0;

    float health = 10;
    float damage = 5;
    bool isKnockback = false;
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public void Update()
    {
        //RunLogic();
    }

    public void FixedUpdate()
    {

    }

    public void RunLogic()
    {

        // Calculate direction
        if (GameController.instance.player == null) currentMovementDirection = Vector3.zero;
        else currentMovementDirection = GameController.instance.player.position - transform.position;
        currentMovementDirection.Normalize();

        // Move towards the player //TODO: Maybe have this run every frame? (no, but can definetely be more)
        transform.position += currentMovementDirection * Time.deltaTime * movementSpeed;

        // Push other nearby enemies away
        PushNearbyEnemies();

        // Update spatial group
        int newSpatialGroup = GameController.instance.GetSpatialGroup(transform.position.x, transform.position.y); // GET spatial group
        if (newSpatialGroup != spatialGroup)
        {
            GameController.instance.enemySpatialGroups[spatialGroup].Remove(this); // REMOVE from old spatial group

            spatialGroup = newSpatialGroup; // UPDATE current spatial group
            GameController.instance.enemySpatialGroups[spatialGroup].Add(this); // ADD to new spatial group
        }
    }

    void PushNearbyEnemies()
    {
        List<Enemy> currAreaEnemies = GameController.instance.enemySpatialGroups[spatialGroup].ToList(); // ONLY enemies in the same spatial group

        // Check each enemy, if the distance between them is less than 0.2, push it away
        foreach (Enemy enemy in currAreaEnemies) // ONLY enemies in the same spatial group
        {
            if (enemy == null) continue;
            if (enemy == this) continue;

            float distance = Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y);
            if (distance < baseHitBoxRadius + enemy.GetHitBoxRadius())
            {
                // Push this enemy away
                Vector3 direction = transform.position - enemy.transform.position;
                direction.Normalize();
                enemy.transform.position -= direction * Time.deltaTime * movementSpeed;
            }
        }
    }

    public void ChangeHealth(float amount)
    {
        health += amount;

        if (health <= 0)
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        GameController.instance.UpdateBatchOnUnitDeath("enemy", batchId);
        GameController.instance.enemySpatialGroups[spatialGroup].Remove(this);

        // TODO: Different chances to drop xp?
        //if (Random.Range(0, 100) < 25)
        GameController.instance.DropExperiencePoint(transform.position, 1);

        Destroy(gameObject);
    }

    public void ApplyKnockbackEffect(float kbMultiplier, float duration)
    {
        if (isKnockback || kbMultiplier == 0) return;
        StartCoroutine(KnockbackRoutine(kbMultiplier, duration));
    }

    private IEnumerator KnockbackRoutine(float kbMultiplier, float duration)
    {
        isKnockback = true;
        movementSpeed = (float)movementSpeed * -kbMultiplier;
        yield return new WaitForSeconds(duration);
        movementSpeed = (float)-movementSpeed / kbMultiplier;
        isKnockback = false;

    }

    public float GetHitBoxRadius()
    {
        return baseHitBoxRadius;
    }

    private void OnDrawGizmos()
    {
        // Vẽ vòng tròn va chạm toán học của Enemy bằng màu xanh lá
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, baseHitBoxRadius); // hitRadius là bán kính r của quái
    }
}