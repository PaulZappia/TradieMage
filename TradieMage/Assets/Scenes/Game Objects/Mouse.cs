using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject rangeObject;
    Collider2D rangeCollider;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer highlightRenderer;
    public Color defaultColour;
    public bool inRange = false;

    public List<Sprite> blockSprites = new List<Sprite>();
    
    void Start()
    {
        rangeCollider = rangeObject.GetComponent<Collider2D>();
        defaultColour = spriteRenderer.color;
        SetSprite(0);
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        } 
        else
        {
            spriteRenderer.color = Color.red;
            highlightRenderer.color = Color.red;
        }
        inRange = isinRange;
    }

    public void SetSprite(int index)
    {
        spriteRenderer.sprite = blockSprites[index];
    }

}
