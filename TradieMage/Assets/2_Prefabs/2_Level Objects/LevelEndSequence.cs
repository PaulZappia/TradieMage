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

    private bool isTriggered = false;
    private bool loadTriggered = false;
    private bool transitionTriggered = false;
    
    private float timer;


    AsyncOperation asyncLoad;

    private void Awake()
    {
        // If no particle system was assigned in the inspector, try to find one in children
        if (particles == null)
        {
            particles = GetComponentsInChildren<ParticleSystem>();
            particles[1].Play();
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
            if (!loadTriggered)
            {
                StartCoroutine(AsyncLoadScene());
                loadTriggered = true;
            }
            
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // Make sure the level name is valid before loading
                if (string.IsNullOrEmpty(nextLevelName))
                {
                    Debug.LogWarning("No next level name specified!");
                    return;
                }
                if (!transitionTriggered)
                {
                    StartCoroutine(TransitionLevel());
                    transitionTriggered = true;
                    return;
                }                
            }
        }
    }

    IEnumerator AsyncLoadScene()
    {
        
        asyncLoad = SceneManager.LoadSceneAsync(nextLevelName);
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

    
}