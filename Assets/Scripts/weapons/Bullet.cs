using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class Bullet : MonoBehaviour
{
    public string poolName;
    private float timer = 0f;
    public bool spinBullet;

    protected int spatialGroup = 0;

    Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection
    {
        get { return movementDirection; }
        set { movementDirection = value; }
    }

    public float bulletDamage;
    public float movementSpeed;
    public float knockBack;
    public float bulletHitBoxRadius;
    public float area = 1;

    public Transform modelTransform;

    public int pierce;

    // Currently surrounding spatial groups
    protected List<int> surroundingSpatialGroups = new List<int>();

    // ON SPAWN
    public delegate void BulletSpawnAction();
    public event BulletSpawnAction OnBulletSpawned;

    // ON TRAVEL
    public delegate void BulletTravelAction(Transform parentBullet);
    public event BulletTravelAction OnBulletTravel;

    // ON DESTROY
    public delegate void BulletDestructionAction(Transform parentBullet);
    public event BulletDestructionAction OnDestroy;

    // ON CONTACT WITH ENEMY
    public delegate void BulletEnemyContactAction(Transform parentBullet);
    public event BulletEnemyContactAction OnContactWithEnemy;

    protected bool isDestroyed = false;

    // void Start()
    // {
    //     spatialGroup = GameController.instance.GetSpatialGroupStatic(transform.position.x, transform.position.y); // GET spatial group

    //     // Trigger the event when the bullet is instantiated
    //     OnBulletSpawned?.Invoke();
    // }

    void FixedUpdate()
    {
        if (!isDestroyed) RunLogic();
    }

    public abstract void RunLogic();

    public void OnceASecondLogic()
    {
        // If destroyed, skip
        if (this == null) return;

        // Destroy if out of bounds
        DestroyIfOutOfBounds();

        OnBulletTravel?.Invoke(transform);
    }

    protected void CheckCollisionWithEnemy()
    {
        List<Enemy> surroundingEnemies = Utils.GetAllEnemiesInSpatialGroups(surroundingSpatialGroups);

        foreach (Enemy enemy in surroundingEnemies)
        {
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < bulletHitBoxRadius * area + enemy.GetHitBoxRadius())
            {
                // Trigger the event when the bullet collides with an enemy
                OnContactWithEnemy?.Invoke(transform);

                // Deal damage to them
                enemy.ApplyKnockbackEffect(knockBack, 1.0f);
                enemy.ChangeHealth(-bulletDamage);


                // Destroy bullet
                pierce--;
                if (pierce <= 0)
                {
                    // Destroy bullet
                    DestroyBullet();
                    break;
                }
            }
        }
    }

    void DestroyIfOutOfBounds()
    {
        if (
            transform.position.x < GameController.instance.MAP_WIDTH_MIN ||
            transform.position.x > GameController.instance.MAP_WIDTH_MAX ||
            transform.position.y < GameController.instance.MAP_HEIGHT_MIN ||
            transform.position.y > GameController.instance.MAP_HEIGHT_MAX ||
            Vector2.Distance(transform.position, GameController.instance.player.position) > 20f
        )
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        if (isDestroyed) return;

        // Trigger the event when the bullet is destroyed
        OnDestroy?.Invoke(transform);

        //Knife.instance.ProjectileCount--;
        GameController.instance.bulletSpatialGroups[spatialGroup].Remove(this);
        //Destroy(gameObject);
        //ResetStats();
        BulletPoolManager.instance.ReturnToPool(poolName, this.gameObject);
        isDestroyed = true;
    }

    private void OnBecameInvisible()
    {
        // Khi ra khỏi màn hình, gọi hàm để thu hồi về Pool
        DestroyBullet();
    }

    public void StartTravel()
    {
        isDestroyed = false;
        spatialGroup = GameController.instance.GetSpatialGroupStatic(transform.position.x, transform.position.y); // GET spatial group
        //GameController.instance.bulletSpatialGroups[spatialGroup].Add(this);
        // Trigger the event when the bullet is instantiated
        OnBulletSpawned?.Invoke();
    }

    protected virtual void ResetStats()
    {
        transform.position = Vector3.zero;
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3((float)scale.x / area, (float)scale.y / area, (float)scale.z / area);  // reset scale
    }
}