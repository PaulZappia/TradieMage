//using System.Diagnostics;
using Unity.VisualScripting;
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
    private Vector3 targetTransform;
    private Vector3 tempTransform;
    public float waitTimer = 0;
    public float maxWaitTime = 1.5f;

    public LayerMask groundLayer;

    public Transform checkUp;
    public Transform checkDown;
    public Transform checkLeft;
    public Transform checkRight;

    private void Awake()
    {
        cachedTransform = transform;
        targetTransform = transform.position;
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
            targetTransform = Vector3.MoveTowards(targetTransform, nextPosition, moveSpeed * Time.deltaTime);
            //if nothing there move

            tempTransform = cachedTransform.position;
            cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, targetTransform, moveSpeed * Time.deltaTime);

            if (Physics2D.OverlapBox(checkLeft.position, new Vector2(0.01f,0.5f), 0, groundLayer) || Physics2D.OverlapBox(checkRight.position, new Vector2(0.01f, 0.5f), 0, groundLayer))
            {
                Debug.Log("<AUNBT");
            }
            /*
            Physics2D solid = Physics2D.OverlapBox(cachedTransform.position, cachedTransform.GetComponent<BoxCollider2D>().size, 0, groundLayer);
            if (solid != null &&   )
            {

                cachedTransform.position = tempTransform;
                Debug.Log("Bonk");
            }
            else
            {
                
            }
            */

        }
        
        // Handle target switching regardless of active state
        if (targetTransform == nextPosition)
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

        //if (Phys)


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





    private void OnDrawGizmos()
    {
        //Ground checker
        //Gizmos.DrawWireCube(cachedTransform.position, cachedTransform.GetComponent<BoxCollider2D>().size);
    }

}