using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Add this class to define the InteractionPromptUI
public class InteractionPromptUI : MonoBehaviour
{
    // Singleton pattern
    public static InteractionPromptUI Instance { get; private set; }
    
    [SerializeField] private GameObject promptCanvas;
    [SerializeField] private TMPro.TextMeshProUGUI promptText;
    
    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ShowPrompt(string message)
    {
        if (promptCanvas != null && promptText != null)
        {
            promptCanvas.SetActive(true);
            promptText.text = message;
        }
    }
    
    public void HidePrompt()
    {
        if (promptCanvas != null)
        {
            promptCanvas.SetActive(false);
        }
    }
}

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    [Tooltip("How close the player needs to be to interact")]
    public float interactionRadius = 2f;
    
    [Tooltip("What layer the player is on")]
    public LayerMask playerLayer;
    
    [Tooltip("Key to press for interaction")]
    public KeyCode interactionKey = KeyCode.E;
    
    [Tooltip("Message to display when in range")]
    public string promptMessage = "Press E to interact";
    
    [Header("Transformation Options")]
    [Tooltip("What scene to load when interacted with")]
    public string sceneToLoad = "";
    
    [Tooltip("Instead of loading a new scene, execute this event")]
    public UnityEvent onInteract;
    
    [Tooltip("Visual effect to play on interaction")]
    public GameObject interactionEffect;
    
    [Header("Color Inversion")]
    [Tooltip("Should this object toggle color inversion when interacted with?")]
    public bool toggleColorInversion = true;
    
    [Tooltip("Whether touching this object toggles inversion (no key press needed)")]
    public bool invertOnTouch = false;
    
    private bool playerInRange = false;
    private GameManager gameManager;
    
    void Start()
    {
        // Find the GameManager in the scene - FIXED DEPRECATED METHOD
        gameManager = FindAnyObjectByType<GameManager>();
        
        // Initialize the event if null
        if (onInteract == null)
            onInteract = new UnityEvent();
    }
    
    void Update()
    {
        // Check for player input when in range
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            Interact();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player based on layer
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = true;
            ShowInteractionPrompt(true);
            
            // If set to invert on touch, trigger immediately
            if (invertOnTouch && toggleColorInversion)
            {
                // Use GameManager if available, otherwise fall back to direct ColorInverter use
                if (gameManager != null)
                {
                    gameManager.ToggleColorInversion();
                }
                else if (ColorInverter.Instance != null)
                {
                    ColorInverter.Instance.ToggleInversion();
                }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        // Check if the collider is the player based on layer
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = false;
            ShowInteractionPrompt(false);
            
            // If set to revert on exit, toggle again
            if (invertOnTouch && toggleColorInversion)
            {
                // Use GameManager if available, otherwise fall back to direct ColorInverter use
                if (gameManager != null)
                {
                    gameManager.ToggleColorInversion();
                }
                else if (ColorInverter.Instance != null)
                {
                    ColorInverter.Instance.ToggleInversion();
                }
            }
        }
    }
    
    // For 2D games, use these instead
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = true;
            ShowInteractionPrompt(true);
            
            // If set to invert on touch, trigger immediately
            if (invertOnTouch && toggleColorInversion)
            {
                // Use GameManager if available, otherwise fall back to direct ColorInverter use
                if (gameManager != null)
                {
                    gameManager.ToggleColorInversion();
                }
                else if (ColorInverter.Instance != null)
                {
                    ColorInverter.Instance.ToggleInversion();
                }
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerInRange = false;
            ShowInteractionPrompt(false);
            
            // If set to revert on exit, toggle again
            if (invertOnTouch && toggleColorInversion)
            {
                // Use GameManager if available, otherwise fall back to direct ColorInverter use
                if (gameManager != null)
                {
                    gameManager.ToggleColorInversion();
                }
                else if (ColorInverter.Instance != null)
                {
                    ColorInverter.Instance.ToggleInversion();
                }
            }
        }
    }
    
    void ShowInteractionPrompt(bool show)
    {
        // Use the UI prompt system if available
        if (show && InteractionPromptUI.Instance != null)
        {
            InteractionPromptUI.Instance.ShowPrompt(promptMessage);
        }
        else if (InteractionPromptUI.Instance != null)
        {
            InteractionPromptUI.Instance.HidePrompt();
        }
        // Fallback to debug log if UI system isn't available
        else if (show)
        {
            Debug.Log(promptMessage);
        }
    }
    
    void Interact()
    {
        // Play effect if assigned
        if (interactionEffect != null)
        {
            Instantiate(interactionEffect, transform.position, Quaternion.identity);
        }
        
        // Toggle color inversion if enabled
        if (toggleColorInversion)
        {
            // Use GameManager if available, otherwise fall back to direct ColorInverter use
            if (gameManager != null)
            {
                gameManager.ToggleColorInversion();
            }
            else if (ColorInverter.Instance != null)
            {
                ColorInverter.Instance.ToggleInversion();
            }
        }
        
        // Invoke any custom events
        onInteract.Invoke();
        
        // Load new scene if specified
        if (!string.IsNullOrEmpty(sceneToLoad) && gameManager != null)
        {
            // Check if the scene exists in the GameManager's list
            if (gameManager.sceneNameList.Contains(sceneToLoad))
            {
                // Load the scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Scene '" + sceneToLoad + "' not found in GameManager scene list!");
            }
        }
    }
}