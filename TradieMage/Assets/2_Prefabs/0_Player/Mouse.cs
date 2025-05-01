using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject rangeObject;
    CircleCollider2D rangeCollider;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer highlightRenderer;
    public Color defaultColour;
    public bool inRange = false;
    private float colliderSizeSmall = 0.01f;
    private float colliderSizeLarge = 1.0f;
    //public bool isRangeExtended = false;

    public bool isOutOfMana = false;

    public List<Sprite> blockSprites = new List<Sprite>();
    public List<Sprite> selectionSprites = new List<Sprite>();
    public List<float> selectionSize = new List<float>();
    
    void Start()
    {
        rangeCollider = rangeObject.GetComponent<CircleCollider2D>();
        defaultColour = spriteRenderer.color;
        SetSprite(0);
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        updateOutOfMana();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BuildRange") && !collision.CompareTag("PlayerRadius"))
        {
            SetRange(true);
        } 
        else if (collision.CompareTag("PlayerRadius"))
        {
            SetRange(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BuildRange"))
        {
            SetRange(false);
        }

        if (collision.CompareTag("PlayerRadius"))
        {
            SetRange(true);
        }    
    }

    void SetRange(bool isinRange = false)
    {   
        if (isinRange) {
            spriteRenderer.color = defaultColour;
            highlightRenderer.color = defaultColour;
            updateOutOfMana();
        } 
        else
        {
            spriteRenderer.color = Color.red;
            highlightRenderer.color = Color.red;
        }
        inRange = isinRange;
        //if not enough mana
        

    }

    public void updateOutOfMana()
    {
        if (isOutOfMana)
        {
            spriteRenderer.color = Color.grey;
            highlightRenderer.color = Color.grey;
        }
        else
        {
            if (inRange)
            {
                spriteRenderer.color = defaultColour;
                highlightRenderer.color = defaultColour;
            }
            else
            {
                spriteRenderer.color = Color.red;
                highlightRenderer.color = Color.red;
            }
        }
    }

    public void SetSprite(int index)
    {
        spriteRenderer.sprite = blockSprites[index];
        highlightRenderer.sprite = selectionSprites[index];
        rangeCollider.radius = selectionSize[index];
        /*
        if (highlightRenderer.sprite.bounds.size.x > 8)
        {
            rangeCollider.radius = colliderSizeLarge;
            Debug.Log("Beeg");
        }
        else
        {
            rangeCollider.radius = colliderSizeSmall;
            Debug.Log("Small");
        }
        */
    }

}
