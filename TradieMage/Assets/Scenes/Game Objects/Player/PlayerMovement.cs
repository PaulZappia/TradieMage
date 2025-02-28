using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
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
    public GameObject box;
    public TMP_Text manaPointsDisplay;

    [Header("Mana")]
    public int mana = 3;
    public int woodenBoxCost = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //boxLayer = this.GetComponent<>
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



        UpdateHUD();

    }

    public void HandleInputs()
    {
        isBuilding = Input.GetMouseButtonDown(0); //left Mouse button
        isDestroying = Input.GetMouseButtonDown(1); //right mouse button
    }

    public void UpdateHUD()
    {
        manaPointsDisplay.text = "MANA: " + mana;
    }

    public void BuildBox()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mana >= woodenBoxCost && !Physics2D.OverlapBox(mousePos, new Vector2(1f, 1f), 0, groundLayer))
        {
            GameObject newBox = Instantiate(box);
            
            mousePos.z = 0f;
            //mousePos.x = Mathf.RoundToInt(mousePos.x) - 0.5f;
            //mousePos.y = Mathf.RoundToInt(mousePos.y) - 0.5f;
            newBox.transform.position = mousePos;

            mana -= woodenBoxCost;
        }
        
    }

    public void RecycleBox()
    {
        
        

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        Collider2D newBox = Physics2D.OverlapBox(mousePos, new Vector2(1f, 1f), 0, boxLayer);
        Debug.Log(newBox);
        if (newBox != null)
        {
            Destroy(newBox.gameObject);
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

