using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEndSequence : MonoBehaviour
{
    [Tooltip("Name of the next scene to load")]
    [SerializeField] private string nextLevelName;

    [Tooltip("Time to wait before loading the next level")]
    [SerializeField] private float transitionDelay = 3f;

    [Tooltip("Reference to the particle system (optional)")]
    //[SerializeField] private ParticleSystem completionEffect;
    //[SerializeField] private ParticleSystem idleParticles;
    private ParticleSystem[] particles;

    [SerializeField] private GameObject levelTransition;

    public AudioSource audioSource;
    public AudioClip mainVictorySound;
    public AudioClip[] victorySounds;
    [UnityEngine.Range(0f, 1f)]
    public float victoryVolume = 0.7f;
    public float victoryPhraseVolume = 1f;

    private bool isTriggered = false;
    private bool soundTriggered = false;
    private bool loadTriggered = false;
    private bool transitionTriggered = false;
    
    private float timer;
    [Header("Letter Visibility")]
    public bool isLetterVisible = true;

    AsyncOperation asyncLoad;

    private void Awake()
    {
        // If no particle system was assigned in the inspector, try to find one in children
        if (particles == null)
        {
            particles = GetComponentsInChildren<ParticleSystem>();
            if(isLetterVisible)
                particles[1].Play();
        }
        if (!isLetterVisible)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent multiple triggers and check for player tag
        if (!isTriggered && collision.CompareTag("Player"))
        {
            isTriggered = true;
            timer = transitionDelay;

            // Play particle effect if available
            if (particles[0] != null)
            {
                particles[0].Play();
            }

            // You could also notify other systems here, like disabling player controls
            // Example: collision.GetComponent<PlayerController>()?.DisableControls();
        }
    }

    private void Update()
    {
        // Only run the timer logic if the sequence has been triggered
        if (isTriggered)
        {
            if (!soundTriggered)
            {
                StartCoroutine(PlayVictorySound());
                soundTriggered = true;
            }

            if (!loadTriggered)
            {
                // Make sure the level name is valid before loading
                if (string.IsNullOrEmpty(nextLevelName))
                {
                    Debug.LogWarning("No next level name specified! Defaulting to Title Screen!");
                    // If not, set destination to Title Screen
                    StartCoroutine(AsyncLoadScene("LevelSelect"));
                } 
                else
                {
                    StartCoroutine(AsyncLoadScene(nextLevelName));
                }
                loadTriggered = true;
            }
            
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (!transitionTriggered)
                {
                    // Make sure the level name is valid before loading
                    /*
                    if (string.IsNullOrEmpty(nextLevelName))
                    {
                        Debug.LogWarning("No next level name specified!");
                        transitionTriggered = true;
                        return;
                    }
                    */
                
                    StartCoroutine(TransitionLevel());
                    transitionTriggered = true;
                    return;
                }                
            }
        }
    }

    IEnumerator AsyncLoadScene(string level)
    {
        
        asyncLoad = SceneManager.LoadSceneAsync(level); // nextLevelName
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.99f)
        {
            yield return null;
        }

        //StartCoroutine(TransitionLevel());
    }

    IEnumerator TransitionLevel()
    {
        Instantiate(levelTransition);
        yield return new WaitForSeconds(.5f);
        yield return null;
        asyncLoad.allowSceneActivation = true;
        //SceneManager.LoadSceneAsync(nextLevelName, LoadSceneMode.Additive);
        //SceneManager.LoadScene(nextLevelName);
        //yield return null;
    }

    IEnumerator PlayVictorySound()
    {
        audioSource.PlayOneShot(mainVictorySound, victoryVolume);
        yield return new WaitForSeconds(0.5f);
        PlayRandomVictorySound();
        yield return null;
    }

    private void PlayRandomVictorySound()
    {        
        // Check if there are any jump sounds and audio source is assigned
        if (victorySounds != null && victorySounds.Length > 0 && audioSource != null)
        { 
            // Select a random jump sound from the array
            AudioClip randomJumpSound = victorySounds[UnityEngine.Random.Range(0, victorySounds.Length)];
            audioSource.PlayOneShot(randomJumpSound, victoryPhraseVolume);
        }
        else
        {
            Debug.LogWarning("No jump sounds assigned or audio source missing!");
        }
    }
     






}