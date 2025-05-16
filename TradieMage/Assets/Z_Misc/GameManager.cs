using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    [Header("Scene Management")]
    public List<string> sceneNameList = new List<string>();
    public int currentSceneIndex = 0;

    //[Header("Game State")]
    //public bool gameIsPaused = false;
    //public int playerScore = 0;

    [Header("Pause Menu")]
    public Canvas pauseMenu;

    // Track if colors are currently inverted
    private bool _colorsInverted = false;
    public bool ColorsInverted 
    { 
        get { return _colorsInverted; }
        set 
        { 
            _colorsInverted = value;
            // Apply inversion if ColorInverter is available
            if (ColorInverter.Instance != null)
            {
                ColorInverter.Instance.SetInversion(_colorsInverted);
            }
        }
    }
    
    // Keep the GameManager between scenes
    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Register for scene loaded events to reset inversion
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(pauseMenu.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load initial scene if not already in it
        //if (SceneManager.GetActiveScene().name != sceneNameList[currentSceneIndex])
        //{
        //    SceneManager.LoadScene(sceneNameList[currentSceneIndex]);
        //}

        //LoadScene main menu
        SceneManager.LoadScene("MainMenu");

    }

    // Update is called once per frame
    void Update()
    {
        // Add any game-wide update logic here
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenu.GetComponent<PauseMenu>().isActiveAndEnabled)
            {
                pauseMenu.GetComponent<PauseMenu>().Resume();
            }
            else
            {
                pauseMenu.GetComponent<PauseMenu>().Pause();
            }
            
        }
    }
    
    // Method to change scenes
    public void LoadScene(string sceneName)
    {
        if (sceneNameList.Contains(sceneName))
        {
            currentSceneIndex = sceneNameList.IndexOf(sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene not found in scene list: " + sceneName);
        }
    }
    
    // Method to load the next scene in the list
    public void LoadNextScene()
    {
        int nextIndex = (currentSceneIndex + 1) % sceneNameList.Count;
        currentSceneIndex = nextIndex;
        SceneManager.LoadScene(sceneNameList[nextIndex]);
    }
    
    // This is called when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset inversion state when a new scene loads
        if (_colorsInverted)
        {
            _colorsInverted = false;
        }
    }
    
    // Toggle color inversion
    public void ToggleColorInversion()
    {
        ColorsInverted = !ColorsInverted;
    }
    
    // Transforms the game state in some way
    public void TransformGameState(string transformationType)
    {
        switch (transformationType)
        {
            case "invert_colors":
                ToggleColorInversion();
                break;
                
            case "day_to_night":
                // Logic to change lighting/skybox
                Debug.Log("Transformed game from day to night");
                break;
                
            case "unlock_ability":
                // Logic to grant player new abilities
                Debug.Log("Unlocked new player ability");
                break;
                
            case "change_weather":
                // Logic to change weather effects
                Debug.Log("Changed weather conditions");
                break;
                
            default:
                Debug.Log("Unknown transformation type: " + transformationType);
                break;
        }
        
        // You can broadcast this transformation to other game systems
        // using events or direct references
    }
    
    // Clean up when destroyed
    private void OnDestroy()
    {
        // Unregister from scene loaded events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
