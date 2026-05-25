using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AxeBullet : Bullet
{
    public float v0;
    public float alpha; // with the horizontal

    float gravity = 9.81f;

    public Vector3 p0;
    public float timeAlive;

    public override void RunLogic()
    {
        timeAlive += Time.deltaTime;

        // Công thức ném xiên:
        // x = x0 + v0 * cos(alpha) * t
        // y = y0 + v0 * sin(alpha) * t - 0.5 * g * t^2
        float x = p0.x + (v0 * Mathf.Cos(alpha) * timeAlive);
        float y = p0.y + (v0 * Mathf.Sin(alpha) * timeAlive) - (0.5f * gravity * timeAlive * timeAlive);
        Debug.Log("parameter: " + x + " " + y + " " + timeAlive);
        Vector2 nextPosition = new Vector2(x, y);
        if (!GameController.instance.IsInsideTheMap(nextPosition))
        {
            transform.position = Vector3.zero;
            DestroyBullet();
            return;
        }
        else
        {
            transform.position = nextPosition;
        }

        transform.Rotate(0, 0, -500 * Time.deltaTime);

        int newSpatialGroup = GameController.instance.GetSpatialGroupStatic(transform.position.x, transform.position.y); // GET spatial group
        if (newSpatialGroup != spatialGroup)
        {
            GameController.instance.bulletSpatialGroups[spatialGroup].Remove(this); // REMOVE from old spatial group

            spatialGroup = newSpatialGroup; // UPDATE current spatial group
            GameController.instance.bulletSpatialGroups[spatialGroup].Add(this); // ADD to new spatial group
            surroundingSpatialGroups = Utils.GetExpandedSpatialGroups(spatialGroup, MovementDirection);
        }

        // Check collision with enemies
        CheckCollisionWithEnemy();
    }
}
