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
        if (isAttracted()) isMoving = true;
        else isMoving = false;

        if (isMoving)
        {
            movementSpeed = 10;
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

    private bool isAttracted()
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
