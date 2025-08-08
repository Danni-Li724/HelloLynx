using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HoppingElectricity : AutomaticMovement
{
   [Header("Hop Settings")] 
    public float hopHeight = 0.5f;
    public float hopDuration = 0.3f;

    private float previousSine = 0f;

    protected override void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (currentWaypointIndex >= waypoints.Length)
        {
            Destroy(gameObject); // Destroy at the end
            return;
        }
        Transform target = waypoints[currentWaypointIndex];
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // Bobbing
        float sineFrequency = .5f, sineAmplitude = .02f;
        float time = Time.time;
        float currentSine = Mathf.Sin(time * Mathf.PI * 2f * sineFrequency);
        newPosition.y += currentSine * sineAmplitude;

        // Popup at bob peak
        if (previousSine < currentSine && currentSine >= 0.9999f)
        previousSine = currentSine;

        // Set position
        transform.position = newPosition;

        // Flip only, no rotation
        if (faceDirection && Mathf.Abs(targetPosition.x - currentPosition.x) > 0.01f)
            FaceDirection(targetPosition - currentPosition);

        // Advance waypoint or destroy if at last
        if (Vector3.Distance(newPosition, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                Destroy(gameObject);
        }
    }
    protected override void FaceDirection(Vector2 direction)
    {
        // Only flip, do not rotate!
        if (direction.x < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction.x > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
