using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
//using Unity.Android.Types;
using TMPro;
using System;
using NUnit.Framework;
using UnityEngine.Rendering;

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
    
    [Header("HUD")]
    public TMP_Text selectedBoxHUDText;


    [Header("Debug")]
    public TMP_Text coords;
    public TMP_Text coordsRound;
    public TMP_Text coordsPlayer;
    public TMP_Text raycastDebugText;

    [Header("Unsorted")]
    public RaycastHit2D raycastHit;
    public Vector2 mouseDirection;


    //private bool isFacingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.linearVelocity.y);
        
        GroundCheck();

        //flip sprite
        //if (Mathf.Sign(rigidBody.linearVelocity.x) != 0) 
        //    SetFacingDirection(Mathf.Sign(rigidBody.linearVelocity.x));


        UpdateHUD();

    }

    public void FixedUpdate()
    {
        

    }
    
    public void UpdateHUD()
    {
        coordsPlayer.text = "PlayerX: " + gameObject.transform.position.x.ToString("F2") + "\n" + "PlayerY: " + gameObject.transform.position.y.ToString("F2");

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
    
    private void OnDrawGizmos()
    {
        //Ground checker
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
        //Gizmos.DrawCube(mousePosRound+ new Vector3(0.5f, -0.5f, 0f), new Vector3(0.1f, 0.1f, 0.01f));//mouse block check
        //Gizmos.DrawCube(mousePos, new Vector3(0.2f, 0.2f, 0.01f));//mouse block check
        Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.01f));//mouse block check
    }


}

