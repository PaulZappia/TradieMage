using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndSequence : MonoBehaviour
{
    [Tooltip("Name of the next scene to load")]
    [SerializeField] private string nextLevelName;

    [Tooltip("Time to wait before loading the next level")]
    [SerializeField] private float transitionDelay = 5f;

    [Tooltip("Reference to the particle system (optional)")]
    [SerializeField] private ParticleSystem completionEffect;

    private bool isTriggered = false;
    private float timer;

    private void Awake()
    {
        // If no particle system was assigned in the inspector, try to find one in children
        if (completionEffect == null)
        {
            completionEffect = GetComponentInChildren<ParticleSystem>();
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
            if (completionEffect != null)
            {
                completionEffect.Play();
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

                SceneManager.LoadScene(nextLevelName);
            }
        }
    }
}