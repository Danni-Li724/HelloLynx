using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

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

    private void Start()
    {
        cam = Camera.main;
        myCollider = GetComponent<Collider2D>();
        if (addressLabel != null)
            addressLabel.text = targetAddress;
    }

    private void Update()
    {
        // Don't update if already allocated
        if (isAllocated)
            return;

        if (!isDragging && trackTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, trackTarget.position, speed * Time.deltaTime);
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
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == this.gameObject)
            {
                isDragging = true;
                dragOffset = transform.position - (Vector3)mouseWorldPos;
                
                // Disable collider during drag to prevent self-collision
                if (myCollider != null)
                    myCollider.enabled = false;
            }
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            transform.position = (Vector3)mouseWorldPos + dragOffset;
        }

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

        // Use layer filtering to only detect RAMSlots (adjust layer name as needed)
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        
        foreach (Collider2D hit in hits)
        {
            // Skip self-collision
            if (hit.gameObject == this.gameObject)
                continue;
                
            Debug.Log($"Hit collider: {hit.gameObject.name}");
            RAMSlot slot = hit.GetComponent<RAMSlot>();
            if (slot != null)
            {
                Debug.Log($"[Allocation] Byte Target: {targetAddress.ToLower()} vs Slot Address: {slot.slotAddress}");
                
                if (slot.slotAddress == targetAddress.ToLower())
                {
                    // Mark as allocated to prevent further interaction
                    isAllocated = true;
                    
                    // Stop dragging
                    isDragging = false;
                    
                    // Notify game manager
                    CPUGameManager.Instance.RegisterCorrectAllocation(this);
                    
                    // Start destruction coroutine
                    StartCoroutine(DestroyAfterDelay(0.3f));
                    return;
                }
                else
                {
                    CPUGameManager.Instance.RegisterIncorrectAllocation();
                    return;
                }
            }
            else
            {
                Debug.Log($"Hit object has no RAMSlot component: {hit.gameObject.name}");
            }
        }
        
        Debug.Log("No valid RAMSlot found");
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
}