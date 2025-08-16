using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMovement : MonoBehaviour
{
     [Header("Movement Settings")]
    public float speed = 8f;
    public Transform[] waypoints; 
    public bool loop = true; 
    public bool faceDirection = true; 
    public int currentWaypointIndex = 0;
    private bool movingForward = true;
    private void FixedUpdate()
    {
        Move();                     // keep your signature
        Physics2D.SyncTransforms(); // force collider shapes to match transforms
    }

    protected virtual void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (currentWaypointIndex < 0 || currentWaypointIndex >= waypoints.Length) {
            Debug.LogWarning("Waypoint index out of range, resetting to 0");
            currentWaypointIndex = 0;
            return;
        }
        Transform target = waypoints[currentWaypointIndex];
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        transform.position = newPosition;

        if (faceDirection && (targetPosition - currentPosition).sqrMagnitude > 0.01f)
        {
            FaceDirection((targetPosition - currentPosition).normalized);
        }

        // Check if the character has reached the waypoint
        if (Vector3.Distance(newPosition, targetPosition) < 0.1f)
        {
            UpdateWaypoint();
        }
    }

    public void UpdateWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
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
