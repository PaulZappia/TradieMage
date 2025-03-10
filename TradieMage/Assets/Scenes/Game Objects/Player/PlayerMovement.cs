using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
//using Unity.Android.Types;
using TMPro;
using System;
using NUnit.Framework;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    //bool facingRight = true;

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
    public float buildRange = 5;

    [Header("Mana")]
    public int mana = 3;
    public int woodenBoxCost = 1;

    [Header("SelectedBox")]
    public int selectedBox = 0;
    //public int[] boxArray = 
    public List<GameObject> boxTypes = new List<GameObject>();
    public List<string> boxTypeNames = new List<string>();


    [Header("HUD")]
    public TMP_Text selectedBoxHUDText;


    [Header("Debug")]
    public TMP_Text coords;
    public TMP_Text coordsRound;
    public TMP_Text coordsPlayer;


    //private bool isFacingRight = false;

    
   

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

        

        //cycle boxes
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log(boxTypes.Count);
            selectedBox++;
            if(selectedBox >= boxTypes.Count)
            {
                selectedBox = 0;
            }
            mouseScript.SetSprite(selectedBox);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log(boxTypes.Count);
            selectedBox--;
            if (selectedBox < 0)
            {
                selectedBox = boxTypes.Count - 1;
            }
            mouseScript.SetSprite(selectedBox);
        }




        //flip sprite
        //if (Mathf.Sign(rigidBody.linearVelocity.x) != 0) 
        //    SetFacingDirection(Mathf.Sign(rigidBody.linearVelocity.x));


        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RoundMouseCoords();
        UpdateHUD();

    }

    public void FixedUpdate()
    {
        HandleInputs();
        
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
    }



    public void HandleInputs()
    {
        isBuilding = Input.GetMouseButton(0); //left Mouse button
        isDestroying = Input.GetMouseButton(1); //right mouse button
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
        coords.text = "MouseX: " + mousePos.x.ToString("F2") + "\n" + "MouseY: " + mousePos.y.ToString("F2");
        coordsRound.text = "MouseRoundX: " + mousePosRound.x.ToString() + "\n" + "MouseRoundY: " + mousePosRound.y.ToString();
        coordsPlayer.text = "PlayerX: " + gameObject.transform.position.x.ToString("F2") + "\n" + "PlayerY: " + gameObject.transform.position.y.ToString("F2");

        selectedBoxHUDText.text = "BOX: " + boxTypeNames[selectedBox];

    }

    public void BuildBox()
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // CHANGE CODE TO BE:
        // 1. Grab Current Mouse Coords
        // 2. Check if those Coords are in range
        // a. If not, return function.
        // b. If so, continue and place block at Saved Coords.

        Vector3 currentMousePos = mousePosRound;
        /*
        if (mouseScript.inRange == false)
        {
            return;
        }
        */
        float dist = Vector3.Distance(this.transform.position, currentMousePos);
        if(dist > buildRange)
        {
            return;
        }


        Collider2D existingBox = Physics2D.OverlapBox(currentMousePos, new Vector2(0.01f, 0.01f), 0, groundLayer);
        //Collider2D playerInside = Physics2D.OverlapBox(mouseObject.transform.position, new Vector2(0.01f, 0.01f), 0, 0);
        //Debug.Log(playerInside);
        Debug.Log(existingBox);
        //Debug.Log();

        if (existingBox != null)
        {
            return;
        } 
        else
        {
            //Debug.Log("This has Run");
            if (mana >= woodenBoxCost) // && !Physics2D.OverlapBox(mousePosRound, new Vector2(0.01f, 0.01f), 0, groundLayer))
            {
                GameObject newBox = Instantiate(boxTypes[selectedBox]);

                //mousePos.z = 0f;
                //Debug.Log(mousePos.x + " || " + Mathf.Round(mousePos.x));
                //mousePos.x = Mathf.Round(mousePos.x); //- 0.5f;
                //Debug.Log(mousePos.y + " || " + Mathf.Round(mousePos.y));
                //mousePos.y = Mathf.Round(mousePos.y); //- 0.5f;
                newBox.transform.position = currentMousePos; // changed from directly using mouse pos to avoid illegal block placement

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
        if (horizontalMovement != 0)
        {
            SetFacingDirection(horizontalMovement);
        }
        //Debug.Log(horizontalMovement);
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

    private void SetFacingDirection(float dir)
    {
        if(dir != 0)
        {
            Vector3 ls = transform.localScale;
            if (dir == 1f)
            {
                ls.x = 1;
            } 
            else if (dir == -1f)
            {
                ls.x = -1;
            }
            
            transform.localScale = ls;
            //this.transform.localScale.x *= -1f;
        }
        else 
        { 
            return; 
        }

    }



    private void OnDrawGizmosSelected()
    {
        //Ground checker
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
    }


}

