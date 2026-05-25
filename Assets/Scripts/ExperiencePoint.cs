using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExperiencePoint : MonoBehaviour
{
    public Transform player;
    public CharacterController playerScript;
    public int Amount;
    public int SpatialGroup = 0;
    public HashSet<int> SurroundingSpatialGroups = new HashSet<int>();
    public float movementSpeed = 0;
    bool isMoving = false;
    Vector3 currentMovementDirection = Vector3.zero;

    public void FixedUpdate()
    {
        RunLogic();
    }
    public void RunLogic()
    {
        if (player == null) return;
        if (IsAttracted()) isMoving = true;
        else isMoving = false;

        if (isMoving)
        {
            movementSpeed = 5;
            if (player == null) currentMovementDirection = Vector3.zero;
            else currentMovementDirection = player.position - transform.position;
            currentMovementDirection.Normalize();

            transform.position += currentMovementDirection * Time.deltaTime * movementSpeed;
        }

        if (Vector2.Distance(transform.position, player.position) <= 0.4f)
        {
            playerScript.ModifyExperience(Amount);
            Destroy(gameObject);
        }

    }

    //  private void OnBecameInvisible()
    // {
    //     // Khi ra khỏi màn hình
    //     gameObject.SetActive(false);

    // }
    // private void OnBecameVisible()
    // {
    //     gameObject.SetActive(true);
    // }

    private bool IsAttracted()
    {
        if (player == null) return false;
        Vector3 playerPosition = player.position;
        int targetSpatialGroup = GameController.instance.GetSpatialGroup(playerPosition.x, playerPosition.y);
        if (targetSpatialGroup == SpatialGroup || SurroundingSpatialGroups.Contains(targetSpatialGroup))
        {
            return true;
        }
        return false;
    }


    
}
