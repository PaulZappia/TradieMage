using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour//, IPointerClickHandler
{
    //PlayerMovement player;
    public int manaCost = 1;

    public bool isTouchingWall;

    private void Start()
    {
        //Debug.Log("HELLO");
        //player = GetComponent<PlayerMovement>();
    }

    private void Awake()
    {
        //Debug.Log("HI");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.LogAssertion("Collision Enter: " + collision.gameObject);
        //if (collision.gameObject.layer.Equals("GroundLayer")) {
        
        if (collision.collider.CompareTag("GroundTag")) 
        { 
            isTouchingWall = true;
            Debug.Log("BOX IS TOUCHING WALL");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.LogAssertion("Collision Exit: " + collision.gameObject);
        //if (collision.gameObject.layer.Equals("GroundLayer"))
        if (collision.collider.CompareTag("GroundTag"))
        {
            isTouchingWall = false;
            Debug.Log("BOX IS NOT TOUCHING WALL");
        }
    }

    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("GroundTag"))
        {
            isTouchingWall = true;
            Debug.Log("BOX IS CONSTANTLY TOUCHING WALL");
        } 
        else if (!collision.collider.CompareTag("GroundTag") && !collision.collider.CompareTag("MovingPlatformTag"))
        {
            isTouchingWall = false;
            Debug.Log("BOX IS NOT CONSTANTLY TOUCHING WALL");
        }
    }
    */

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            player.mana += manaCost;
            Destroy(this);
    }
    */

}
