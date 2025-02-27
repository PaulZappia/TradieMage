using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;

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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.linearVelocity.y);

        /*
        if (jumpTimer > 0)
        {
            jumpTimer--;
        }
        */
        GroundCheck();



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

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer)/* && jumpTimer == 0*/)
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

