using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

public class BytePacket : MonoBehaviour
{
     public string targetAddress;
    public Text addressLabel;
    public float speed = 2f;
    public Transform trackTarget;

    private Vector3 dragOffset;
    private bool isDragging = false;
    private bool isAllocated = false; // Prevent further interaction after allocation
    private Camera cam;
    private Collider2D myCollider;

    private RAMSlot touchedSlot;
    
    [Header("Navigation")]
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;
    public bool movingForward = true;
    
    public bool IsDragging => isDragging;

    private void Start()
    {
        cam = Camera.main;
        myCollider = GetComponent<Collider2D>();
        if (addressLabel != null)
            addressLabel.text = targetAddress;
    }

    private void Update()
    {
        if (isAllocated) return;

        if (!isDragging && waypoints != null && waypoints.Length > 0)
        {
            int last = waypoints.Length - 1;
            currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, last);

            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);

            if ((transform.position - target.position).sqrMagnitude < 0.5f * 0.5f)
            {
                // Stop at the end (or set a flag / event)
                if (currentWaypointIndex < last)
                {
                    currentWaypointIndex++;
                }
                else
                {
                    // reached final waypoint — choose your behavior:
                    // isAllocated = true; // e.g., mark done
                    // currentWaypointIndex = last; // stay put
                    // OR loop: currentWaypointIndex = 0;
                    // OR ping-pong: flip direction logic
                }
            }
        }

        HandleDragInput();
    }

    private void HandleDragInput()
    {
        if (Mouse.current == null || cam == null || isAllocated)
            return;

        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorldPos);
            BytePacket topPacket = null;
            int highestOrder = int.MinValue;
            foreach (Collider2D hit in hits)
            {
                BytePacket packet = hit.GetComponent<BytePacket>();
                if (packet == null || packet.isAllocated) continue;

                int sortingOrder = 0;
                var sr = hit.GetComponent<SpriteRenderer>();
                if (sr != null) sortingOrder = sr.sortingOrder;
                // get highest sorting order
                if (topPacket == null || sortingOrder > highestOrder)
                {
                    topPacket = packet;
                    highestOrder = sortingOrder;
                }
            }

            if (topPacket == this)
            {
                isDragging = true;
                dragOffset = transform.position - (Vector3)mouseWorldPos;

                if (myCollider != null)
                    myCollider.enabled = false;
            }
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            transform.position = (Vector3)mouseWorldPos + dragOffset;
        }
        // drop
        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            
            // Re-enable collider before trying to allocate
            if (myCollider != null)
                myCollider.enabled = true;
                
            TryAllocate();
        }
    }
    
    private void TryAllocate()
    {
        if (isAllocated)
            return;
        Debug.Log($"TryAllocate called at position: {transform.position}");
        // get all colliders at the drop position
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        Debug.Log($"Found {hits.Length} colliders at drop position");
        // find the closest RAMSlot to our drop position
        RAMSlot closestSlot = null;
        float closestDistance = float.MaxValue;
    
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == this.gameObject)
                continue;
            
            RAMSlot slot = hit.GetComponent<RAMSlot>();
            if (slot != null)
            {
                // find distance between drop position and slot center
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                Debug.Log($"Found slot {slot.slotAddress} at distance {distance}");
            
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSlot = slot;
                }
            }
        }
    
        if (closestSlot != null)
        {
            isAllocated = true;
            isDragging = false;
        
            Debug.Log($"[Allocation] Byte Target: {targetAddress.ToLower()} vs Closest Slot: {closestSlot.slotAddress}");

            if (closestSlot.slotAddress == targetAddress.ToLower())
            {
                CPUGameManager.Instance.RegisterCorrectAllocation(this);
                closestSlot.HighlightWhenMatched();
            }
            else
            {
                CPUGameManager.Instance.RegisterIncorrectAllocation();
                closestSlot.HighlightWhenIncorrect();
            }
        
            StartCoroutine(DestroyAfterDelay(0.3f));
        }
        else
        {
            Debug.Log("No RAMSlot found - dropped in empty space");
            CPUGameManager.Instance.RegisterIncorrectAllocation();
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        // visual feedback here
        yield return new WaitForSeconds(delay);
        if (CPUGameManager.Instance != null)
        {
            // CPUGameManager.Instance.OnByteDestroyed(this);
        }
        
        Destroy(gameObject);
    }
    public void CancelDestruction()
    {
        StopAllCoroutines();
        isAllocated = false;
    }
    private void OnSuccessfulAllocation()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }
    
    public void FlashRedAndDestroy(float seconds)
    {
        StartCoroutine(FlashAndDestroyRoutine(seconds));
    }

    private IEnumerator FlashAndDestroyRoutine(float seconds)
    {
        // Lock interaction
        isAllocated = true;
        isDragging = false;
        if (myCollider != null) myCollider.enabled = false;

        // Visual: flash red
        var sr = GetComponent<SpriteRenderer>();
        Color original = Color.white;
        if (sr != null)
        {
            original = sr.color;
            sr.color = Color.red;
        }

        // Use realtime so this still finishes if Time.timeScale == 0
        float endTime = Time.unscaledTime + seconds;
        while (Time.unscaledTime < endTime)
            yield return null;

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var slot = other.GetComponent<RAMSlot>();
        if (slot != null)
        {
            touchedSlot = slot;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var slot = other.GetComponent<RAMSlot>();
        if (slot != null)
        {
            touchedSlot = null;
        }
    }
}