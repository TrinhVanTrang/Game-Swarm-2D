using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ThousandEdgeBullet: Bullet
{
     public override void  RunLogic()
    {
        // timer += Time.deltaTime;
        // if (timer > 10f)
        // {
        //     DestroyBullet();
        // }
        // Move
        Vector3 nextPosition = transform.position + Utils.V2toV3(MovementDirection) * Time.deltaTime * movementSpeed;
        if (!GameController.instance.IsInsideTheMap(nextPosition))
        {
            //Debug.Log("Through a bound! " + spatialGroup + " " + transform.position.x + " " + transform.position.y);
            transform.position = Vector3.zero;
            DestroyBullet();
            return;
        }
        else
        {
            transform.position = nextPosition;
        }

        // Calculate the angle
        //float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.Atan2(MovementDirection.y, MovementDirection.x) * Mathf.Rad2Deg;

        // Set the rotation of the transform
        if (!spinBullet) modelTransform.rotation = Quaternion.Euler(0, 0, angle);
        else modelTransform.Rotate(0, 0, 10f);

        // Update spatial group
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