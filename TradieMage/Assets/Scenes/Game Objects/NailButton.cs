using UnityEngine;

public class NailButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isPressed = false;
    public SpriteRenderer buttonSprite;
    public Sprite buttonActive;
    public Sprite buttonInactive;

    public MovingPlatform connectedObject1;
    //public GameObject connectedObject1;

    void Start()
    {
        buttonSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isPressed && collision.gameObject.CompareTag("BoxTag"))
        {
            //Debug.Log("yay");
            isPressed = true;
            buttonSprite.sprite.
            if (connectedObject1 != null)
            {
                connectedObject1.isActive = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPressed && collision.gameObject.CompareTag("BoxTag"))
        {
            //Debug.Log("nay :(");
            isPressed = false;
            if (connectedObject1 != null)
            {
                connectedObject1.isActive = false;
            }
        }
    }


}
