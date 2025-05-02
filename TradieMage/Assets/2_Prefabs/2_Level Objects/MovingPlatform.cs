using UnityEngine;

public class MovingPlatform : ToggleObject
{
    [Header("Movement Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private bool isPaused = false;

    private Vector3 nextPosition;
    private Transform cachedTransform;
    private Transform targetTransform;

    public float waitTimer = 0;
    public float maxWaitTime = 1.5f;

    private void Awake()
    {
        cachedTransform = transform;
        //waitTimer = maxWaitTime;
    }
    
    private void Start()
    {
        // Validate that points are assigned
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Moving platform points not assigned", this);
            enabled = false;
            return;
        }
        waitTimer = maxWaitTime;
        nextPosition = pointB.position;
    }
    
    private void Update()
    {
        // Handle movement when active
        if (isActive && !isPaused)
        {
            //cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, nextPosition, moveSpeed * Time.deltaTime);
            //targetTransform.position = Vector3.MoveTowards(cachedTransform.position, nextPosition, moveSpeed * Time.deltaTime);
            cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, nextPosition, moveSpeed * Time.deltaTime);
        }
        
        // Handle target switching regardless of active state
        if (cachedTransform.position == nextPosition)
        {
            //Wait here for 1 sec
            isPaused = true;

            //waitTimer = waitTime;
            if (waitTimer >= 0)
            {
                waitTimer -= Time.deltaTime;
            } 
            else
            {
                nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
                waitTimer = maxWaitTime;
                isPaused = false;
            }
            
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        
        // Handle player collision
        if (collisionObject.CompareTag("Player"))
        {
            collisionObject.transform.SetParent(cachedTransform);
            return;
        }
        
        // Handle box collision
        BoxCollider2D boxCollider = collisionObject.GetComponentInChildren<BoxCollider2D>();
        if (boxCollider != null && boxCollider.CompareTag("BoxTag"))
        {
            collisionObject.transform.SetParent(cachedTransform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        
        // Handle player collision
        if (collisionObject.CompareTag("Player"))
        {
            collisionObject.transform.SetParent(null);
            return;
        }
        
        // Handle box collision
        BoxCollider2D boxCollider = collisionObject.GetComponentInChildren<BoxCollider2D>();
        if (boxCollider != null && boxCollider.CompareTag("BoxTag"))
        {
            collisionObject.transform.SetParent(null);
        }
    }
}