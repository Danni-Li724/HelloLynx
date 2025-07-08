using UnityEngine;

public class GirlController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public float jumpInterval = 1f;
    
    [Header("Ground Check")]
    public LayerMask groundLayer = 1;
    public float groundCheckDistance = 0.1f;
    
    private Rigidbody2D rb;
    private Vector3 targetPoint;
    private bool movingToB = true;
    private bool isGrounded = false;
    private float lastJumpTime = 0f;
    private BoxCollider2D boxCollider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        
        // Set initial target
        if (pointA != null && pointB != null)
        {
            targetPoint = pointB.position;
        }
        else
        {
            Debug.LogError("Point A or Point B is not assigned!");
        }
    }
    
    void Update()
    {
        CheckGrounded();
        MoveTowardsTarget();
        HandleJumping();
    }
    
    void CheckGrounded()
    {
        Vector2 rayStart = new Vector2(transform.position.x, transform.position.y - boxCollider.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckDistance, groundLayer);
        
        bool wasGrounded = isGrounded;
        isGrounded = hit.collider != null;
        if (isGrounded && !wasGrounded && hit.collider.CompareTag("Ground"))
        {
            if (CollisionManager.Instance != null)
            {
                CollisionManager.Instance.RegisterCollision();
            }
        }
        
        Debug.DrawRay(rayStart, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
    
    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        float horizontalMovement = direction.x * moveSpeed;
        rb.linearVelocity = new Vector2(horizontalMovement, rb.linearVelocity.y);
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(.05f, 0.1f, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-.05f, 0.1f, 1);
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.5f)
        {
            SwitchTarget();
        }
    }
    
    void HandleJumping()
    {
        if (isGrounded && Time.time - lastJumpTime > jumpInterval)
        {
            Jump();
            lastJumpTime = Time.time;
        }
    }
    
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    
    void SwitchTarget()
    {
        if (movingToB)
        {
            targetPoint = pointA.position;
            movingToB = false;
        }
        else
        {
            targetPoint = pointB.position;
            movingToB = true;
        }
    }
}
