using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

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
    
    [Header("Audio")]
    public AudioSource audioSource;
    // Changed to support multiple jump sounds
    public AudioClip[] jumpSounds;
    [UnityEngine.Range(0f, 1f)]
    public float jumpVolume = 0.7f;

    [Header("Groundcheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    public LayerMask boxLayer;
    
    [Header("HUD")]
    public TMP_Text selectedBoxHUDText;

    [Header("Animation")]
    public Animator playerAnimator;
    private SpriteRenderer playerSprite;
    public float fallAnimThreshold = 1f;

    [Header("Debug")]
    public TMP_Text coords;
    public TMP_Text coordsRound;
    public TMP_Text coordsPlayer;
    public TMP_Text raycastDebugText;

    [Header("Unsorted")]
    public RaycastHit2D raycastHit;
    public Vector2 mouseDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();

        // Get or add AudioSource component
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rigidBody.linearVelocity.y);
        
        GroundCheck();

        UpdateHUD();
        playerAnimator.SetFloat("magnitude", rigidBody.linearVelocity.magnitude);
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
            playerAnimator.Play(Animator.StringToHash("Walk"));
        }
        else
        {
            playerAnimator.Play(Animator.StringToHash("Idle"));
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0) 
        {
            //On press jumpbutton
            if (context.performed)
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocityX, jumpPower);
                jumpsRemaining--;

                //play jump sound
                PlayJumpSound();
                playerAnimator.SetTrigger("jump");
            }
            //onrelease jump button
            else if (context.canceled)
            {
                //cancel jump, for small jumps
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocityY * 0.5f);
            }
        }
    }

    private void PlayJumpSound()
    {
        // Check if there are any jump sounds and audio source is assigned
        if (jumpSounds != null && jumpSounds.Length > 0 && audioSource != null)
        {
            // Select a random jump sound from the array
            AudioClip randomJumpSound = jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
            audioSource.PlayOneShot(randomJumpSound, jumpVolume);
        }
        else
        {
            Debug.LogWarning("No jump sounds assigned or audio source missing!");
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
        }
        else
        {
            jumpsRemaining = 0;
            if (rigidBody.linearVelocity.y < fallAnimThreshold)
            {
                playerAnimator.Play(Animator.StringToHash("Fall"));
            }
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
        Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.01f));//mouse block check
    }
}