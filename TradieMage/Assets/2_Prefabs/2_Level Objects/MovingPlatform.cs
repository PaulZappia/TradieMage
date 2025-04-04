using UnityEngine;

public class MovingPlatform : ToggleObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 1f;
    //public bool isActive = true;//{ get; set; } = true;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition = pointB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        }
       

        if(transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
        if (collision.gameObject.GetComponentInChildren<BoxCollider2D>().CompareTag("BoxTag"))
        {
            
            //Debug.Log("Box");
            collision.gameObject.transform.parent = transform;

        }
        //Debug.Log(collision.gameObject.GetComponentInChildren<BoxCollider2D>().ToString());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("BoxTag"))
        {
            collision.gameObject.transform.parent = null;
        }
        if (collision.gameObject.GetComponentInChildren<BoxCollider2D>().CompareTag("BoxTag"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    




}
