using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
//using Unity.Android.Types;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jump")]
    public float jumpPower = 10f;
    private int jumpsRemaining = 0;
    public int maxJumps = 1;
    //public int jumpTimerMax = 5; //maybe?
    //private int jumpTimer = 0;

    [Header("Groundcheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    public LayerMask boxLayer;

    [Header("Inputs")]
    public bool isBuilding;
    public bool isDestroying;
    public TMP_Text manaPointsDisplay;
    public GameObject box;

    [Header("Mouse")]
    public Vector3 mousePos;
    public Vector3 mousePosRound;
    public GameObject mouseObject;
    public Mouse mouseScript;

    [Header("Mana")]
    public int mana = 3;
    public int woodenBoxCost = 1;

    [Header("Debug")]
    public TMP_Text coords;
    public TMP_Text coordsRound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //boxLayer = this.GetComponent<>
        mouseScript = mouseObject.GetComponent<Mouse>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.linearVelocity.y);
        HandleInputs();
        /*
        if (jumpTimer > 0)
        {
            jumpTimer--;
        }
        */
        GroundCheck();

        //building
        if (isBuilding)
        {
            BuildBox();
        }

        //destroy box
        if (isDestroying)
        {
            RecycleBox();
        }


        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RoundMouseCoords();
        UpdateHUD();

    }

    public void HandleInputs()
    {
        isBuilding = Input.GetMouseButtonDown(0); //left Mouse button
        isDestroying = Input.GetMouseButtonDown(1); //right mouse button
    }

    public void RoundMouseCoords()
    {
        float x = 0;
        float y = 0;

        x = Mathf.FloorToInt(mousePos.x);
        y = Mathf.CeilToInt(mousePos.y);

        mousePosRound = new Vector3(x, y, 0);
        mouseObject.transform.position = new Vector3(mousePosRound.x + 0.5f, mousePosRound.y - 0.5f, 0);

        //mousePosRound = new Vector3(Mathf.FloorToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);
    }

    public void UpdateHUD()
    {
        manaPointsDisplay.text = "MANA: " + mana;
        coords.text = "X: " + mousePos.x.ToString("F2") + "\n" + "Y: " + mousePos.y.ToString("F2");
        coordsRound.text = "X: " + mousePosRound.x.ToString() + "\n" + "Y: " + mousePosRound.y.ToString();
    }

    public void BuildBox()
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseScript.inRange == false)
        {
            return;
        }

        Collider2D existingBox = Physics2D.OverlapBox(mouseObject.transform.position, new Vector2(0.01f, 0.01f), 0, groundLayer);
        //Collider2D playerInside = Physics2D.OverlapBox(mouseObject.transform.position, new Vector2(0.01f, 0.01f), 0, 0);
        //Debug.Log(playerInside);
        //Debug.Log(existingBox);

        if (existingBox != null)
        {
            return;
        } 
        else
        {
            //Debug.Log("This has Run");
            if (mana >= woodenBoxCost) // && !Physics2D.OverlapBox(mousePosRound, new Vector2(0.01f, 0.01f), 0, groundLayer))
            {
                GameObject newBox = Instantiate(box);

                //mousePos.z = 0f;
                //Debug.Log(mousePos.x + " || " + Mathf.Round(mousePos.x));
                //mousePos.x = Mathf.Round(mousePos.x); //- 0.5f;
                //Debug.Log(mousePos.y + " || " + Mathf.Round(mousePos.y));
                //mousePos.y = Mathf.Round(mousePos.y); //- 0.5f;
                newBox.transform.position = mousePosRound;

                mana -= woodenBoxCost;
            }
        }
    }

    public void RecycleBox()
    {
        if (mouseScript.inRange == false)
        {
            return;
        }

        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //
        Collider2D selectedBox = Physics2D.OverlapBox(mouseObject.transform.position, new Vector2(0.01f, 0.01f), 0, boxLayer);

        //Debug.Log(newBox);
        if (selectedBox != null)
        {
            Destroy(selectedBox.gameObject);
            mana += woodenBoxCost;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0) 
        {
            //On press jumpbutton
            if (context.performed )
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocityX, jumpPower);
                jumpsRemaining--;
                //jumpTimer = jumpTimerMax;
            }
            //onrelease jump button
            else if (context.canceled)
            {
                //cancel jump, for small jumps
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocityY * 0.5f);
            }
        }
        
    }
    /*
    public void Build(InputAction.CallbackContext context)
    {
       print("build");

    }*/

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;

            //print("onground");
        }
        else//if("player has double jump ability or something")
        {
            jumpsRemaining = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Ground checker
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
    }


}

