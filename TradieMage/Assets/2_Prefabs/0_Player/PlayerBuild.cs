using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
//using Unity.Android.Types;
using TMPro;
using System;
using NUnit.Framework;
using UnityEngine.Rendering;

public class PlayerBuild : MonoBehaviour
{

    //public Rigidbody2D rigidBody;
    
    /*
    [Header("Groundcheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    */
    public LayerMask groundLayer;
    public LayerMask boxLayer;
    
    [Header("Inputs")]
    public bool isBuilding;
    public bool isDestroying;
    public TMP_Text manaPointsDisplay;
    //public GameObject box;

    [Header("Mouse")]
    public Vector3 mousePos;
    public Vector3 mousePosRound;
    public GameObject mouseObject;
    public Mouse mouseScript;
    public GameObject buildRadiusCollider;
    public float buildRange = 5;
    public bool isRangeExtended = false;
    public float buildRadius = 4.5f;
    public float buildRadiusExtended = 13.5f;
    public Vector3 mouseBuildCheckOffset = new Vector3(0.5f, -0.5f, 0f);

    [Header("Mana")]
    public int mana = 3;
    //public int woodenBoxCost = 1;
    //public int metalBox = 3;
    public List<int> boxCosts = new List<int>();

    [Header("SelectedBox")]
    public int selectedBox = 0;
    //public int[] boxArray = 
    public List<GameObject> boxTypes = new List<GameObject>();
    public List<string> boxTypeNames = new List<string>();

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buildSound;
    public AudioClip destroySound;
    [UnityEngine.Range(0f, 1f)]
    public float soundVolume = 0.7f;

    [Header("Destruction Effects")]
    public GameObject destroyEffectPrefab;

    [Header("HUD")]
    public TMP_Text selectedBoxHUDText;

    [Header("Animation")]
    public Animator playerAnimator;
    private SpriteRenderer playerSprite;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;
    public TMP_Text coords;
    public TMP_Text coordsRound;
    public TMP_Text coordsPlayer;
    public TMP_Text raycastDebugText;//?

    [Header("Unsorted")]
    public RaycastHit2D raycastHit;
    public Vector2 mouseDirection;


    //private bool isFacingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        //boxLayer = this.GetComponent<>
        //mouseScript = mouseObject.GetComponent<Mouse>();
        setBuildRadiusExtended(isRangeExtended);

        mouseObject = GameObject.Find("MouseCollider");
        mouseScript = mouseObject.GetComponent<Mouse>();
        mouseScript.isOutOfMana = mana < boxCosts[selectedBox];

        if (debugMode)
        {
            GameObject.Find("DebugStats").gameObject.SetActive(true);
            coords = GameObject.Find("Coords").GetComponent<TMP_Text>();
            coordsRound = GameObject.Find("Coords Rounded").GetComponent<TMP_Text>();
            coordsPlayer = GameObject.Find("Coords Player").GetComponent<TMP_Text>();
            //raycastDebugText =  GameObject.Find("Coords").GetComponent<TMP_Text>();
            manaPointsDisplay = GameObject.Find("ManaPointsHUDText").GetComponent<TMP_Text>();
            selectedBoxHUDText = GameObject.Find("SelectedBoxHUD").GetComponent<TMP_Text>();
        }
        else
        {
            GameObject.Find("DebugStats").gameObject.SetActive(false);
        }


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
        
        HandleInputs();

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBox = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBox = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedBox = 2;
        }


        mouseScript.isOutOfMana = mana < boxCosts[selectedBox];
        

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RoundMouseCoords();
        
        if (debugMode)
        {
            UpdateHUD();
        }
    }

    public void FixedUpdate()
    {
        HandleInputs();
        RoundMouseCoords();
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
        mouseScript.isOutOfMana = mana < boxCosts[selectedBox];

    }

    public void MouseRaycast()
    {
        mouseDirection = (transform.position-mousePosRound).normalized;
        /*
        float modifiedBuildRange = buildRange;
        if (isRangeExtended)
        {
            modifiedBuildRange *= buildRangeMultiplier;
        }
        */
        raycastHit = Physics2D.Raycast(transform.position, mouseDirection, buildRange, groundLayer);

        if (raycastHit)
        {
            //Debug.Log(raycastHit.collider);
            raycastDebugText.text = raycastHit.ToString();
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
        //coordsPlayer.text = "PlayerX: " + gameObject.transform.position.x.ToString("F2") + "\n" + "PlayerY: " + gameObject.transform.position.y.ToString("F2");

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
        
        if (mouseScript.inRange == false)
        {
            return;
        }

        /// BROKEN HELLSPAWN \/
        /* 
        float dist = Vector3.Distance(this.transform.position, currentMousePos);
        if(dist > buildRange)
        {
            return;
        }
        */

        Collider2D existingBox = Physics2D.OverlapBox(currentMousePos+ mouseBuildCheckOffset, new Vector2(0.01f, 0.01f), 0, groundLayer);
        //Collider2D playerInside = Physics2D.OverlapBox(mouseObject.transform.position, new Vector2(0.01f, 0.01f), 0, 0);
        //Debug.Log(playerInside);
        //Debug.Log();

        if (existingBox != null)
        {
            //Debug.Log("canceled box");
            return;
        } 
        else
        {
            //Debug.Log("This has Run");
            if (mana >= boxCosts[selectedBox]) // && !Physics2D.OverlapBox(mousePosRound, new Vector2(0.01f, 0.01f), 0, groundLayer))
            {
                GameObject newBox = Instantiate(boxTypes[selectedBox]);

                //mousePos.z = 0f;
                //Debug.Log(mousePos.x + " || " + Mathf.Round(mousePos.x));
                //mousePos.x = Mathf.Round(mousePos.x); //- 0.5f;
                //Debug.Log(mousePos.y + " || " + Mathf.Round(mousePos.y));
                //mousePos.y = Mathf.Round(mousePos.y); //- 0.5f;
                newBox.transform.position = currentMousePos; // changed from directly using mouse pos to avoid illegal block placement
                newBox.GetComponentInChildren<Box>().manaCost = boxCosts[selectedBox];
                newBox.GetComponentInChildren<ParticleSystem>().Play();
                mana -= boxCosts[selectedBox];
                mouseScript.isOutOfMana = mana < boxCosts[selectedBox];
                //Debug.Log(boxCosts[selectedBox]);

                //playerAnimator.Play(Animator.StringToHash("Build"));
                playerAnimator.SetTrigger("build");

                // Play build sound
                if (buildSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(buildSound, soundVolume);
                }
            }
        }
    }

    public void RecycleBox()
    {
        Vector3 currentMousePos = mousePosRound;

        if (mouseScript.inRange == false)
        {
            return;
        }

        Collider2D overlappingBox = Physics2D.OverlapBox(currentMousePos + mouseBuildCheckOffset, new Vector2(0.01f, 0.01f), 0, boxLayer);

        if (overlappingBox != null)
        {
            // Store position before destroying for effects
            Vector3 boxPosition = overlappingBox.transform.position;
            int boxManaCost = overlappingBox.GetComponent<Box>().manaCost;
            
            // Play destroy effects
            PlayDestroyEffects(boxPosition);
            
            // Destroy the box
            Destroy(overlappingBox.gameObject.transform.parent.gameObject);
            
            // Add mana
            mana += boxManaCost;
            
            // Update UI
            mouseScript.isOutOfMana = mana < boxCosts[selectedBox];
            mouseScript.updateOutOfMana();

            //playerAnimator.Play(Animator.StringToHash("Destroy"));
            playerAnimator.SetTrigger("destroy");
        }
    }

    private void PlayDestroyEffects(Vector3 position)
    {
        // Play destroy sound
        if (destroySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destroySound, soundVolume);
        }
        
        // Spawn visual effect if we have one
        if (destroyEffectPrefab != null)
        {
            GameObject effect = Instantiate(destroyEffectPrefab, position, Quaternion.identity);
            
            // Auto-destroy effect after 2 seconds
            Destroy(effect, 2f);
        }
    }

    public void setBuildRadiusExtended(bool extended)
    {
        if (extended)
        {
            buildRadiusCollider.transform.GetComponent<CircleCollider2D>().radius = buildRadiusExtended;
            isRangeExtended = true;
        }
        else
        {
            buildRadiusCollider.transform.GetComponent<CircleCollider2D>().radius = buildRadius;
            isRangeExtended = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Ground checker
        //Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
        Gizmos.DrawCube(mousePosRound+ new Vector3(0.5f, -0.5f, 0f), new Vector3(0.1f, 0.1f, 0.01f));//mouse block check
        Gizmos.DrawCube(mousePos, new Vector3(0.2f, 0.2f, 0.01f));//mouse block check
        Gizmos.DrawWireCube(transform.position, new Vector3(0.01f, 0.01f, 0.01f));//mouse block check
    }
}