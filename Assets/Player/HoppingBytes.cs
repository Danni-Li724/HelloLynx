using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HoppingBytes : AutomaticMovement
{
    [Header("Hop Settings")] 
    public float hopHeight;
    public float hopDuration;

    [Header("Pop-up Settings")] 
    public GameObject popupTextPrefab;
    public float popupDuration;
    public float popupRiseDistance;
    private float previousSine = 0;
    
    private bool isHopping = false;

    protected override void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Transform target = waypoints[currentWaypointIndex];
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;
        
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        float sineFrequency = .5f;
        float sineAmplitude = .02f;
        float time = Time.time;
        float currentSine = Mathf.Sin(time * Mathf.PI * 2f * sineFrequency);
        float bobOffset = currentSine * sineAmplitude;
        newPosition.y += bobOffset;
        if (previousSine < currentSine && currentSine >= 0.9999f)
        {
            ShowPopUp();
        }
        previousSine = currentSine;
        
        transform.position = newPosition;
        if (faceDirection && (targetPosition - currentPosition).sqrMagnitude > 0.01f)
        {
            FaceDirection((targetPosition - currentPosition).normalized);
        }

        if (Vector3.Distance(newPosition, targetPosition) < 0.1f)
        {
            UpdateWaypoint();
        }
    }

    IEnumerator HopToWaypoint(Vector2 target)
    {
        isHopping = true;
        Vector2 start = transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(new Vector3(target.x, target.y+hopHeight, transform.position.z), 
            hopDuration).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOMove(new Vector3(target.x, target.y, transform.position.z), hopDuration / 2)
            .SetEase(Ease.InQuad));
        ShowPopUp();
        yield return sequence.WaitForCompletion();
        transform.position = new Vector3(target.x, target.y, transform.position.z);
        if (faceDirection && (target - start).sqrMagnitude > 0.01f)
        {
            FaceDirection((target - start).normalized);
        }

        UpdateWaypoint();
        isHopping = false;
    }

    void ShowPopUp()
    {
        if (popupTextPrefab == null) return;
        int randomValue = Random.Range(0, 2);
        string text = randomValue.ToString();
        GameObject popup = Instantiate(popupTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        
        Text tempText = popup.GetComponentInChildren<Text>();
        if (tempText != null)
        {
            tempText.text = text;
            Debug.Log("set to" + text);
        }
        
        Sequence popupSeq = DOTween.Sequence();
        popupSeq.Append(popup.transform.DOMoveY(popup.transform.position.y + popupRiseDistance, popupDuration));
        CanvasGroup cg = popup.GetComponent<CanvasGroup>();
        if (!cg) cg = popup.AddComponent<CanvasGroup>();
        popupSeq.Join(cg.DOFade(0, popupDuration));
        popupSeq.OnComplete(() => Destroy(popup));
    }
}
