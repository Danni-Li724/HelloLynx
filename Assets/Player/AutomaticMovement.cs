using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMovement : MonoBehaviour
{
     [Header("Movement Settings")]
    public float speed = 8f;
    public Vector2[] waypoints; 
    public bool loop = true; 
    public bool faceDirection = true; 
    public int currentWaypointIndex = 0;
    private bool movingForward = true;
    private void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Vector2 target = waypoints[currentWaypointIndex];
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, target, speed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        if (faceDirection && (target - currentPosition).sqrMagnitude > 0.01f)
        {
            FaceDirection((target - currentPosition).normalized);
        }

        // Check if the character has reached the waypoint
        if (Vector2.Distance(newPosition, target) < 0.1f)
        {
            UpdateWaypoint();
        }
    }

    private void UpdateWaypoint()
    {
        if (loop)
        {
            // loop waypoints
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        else
        {
            // Reverse direction when reaching the ends of the waypoints
            if (movingForward)
            {
                if (currentWaypointIndex < waypoints.Length - 1)
                    currentWaypointIndex++;
                else
                    movingForward = false;
            }
            else
            {
                if (currentWaypointIndex > 0)
                    currentWaypointIndex--;
                else
                    movingForward = true;
            }
        }
    }
    protected virtual void FaceDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
