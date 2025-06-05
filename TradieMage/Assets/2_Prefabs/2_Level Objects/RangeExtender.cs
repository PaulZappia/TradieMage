using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
public class RangeExtender : MonoBehaviour
{

    public ParticleSystem ParticleEffect;
    private bool isPlayerIn = false;

    public AudioSource audioSource;
    // Changed to support multiple jump sounds
    public AudioClip warbleSound;
    [UnityEngine.Range(0f, 1f)]
    public float sfxVolume = 0.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (isPlayerIn)
        {
            if (!ParticleEffect.isPlaying) {
                ParticleEffect.Play();
            }
            PlaySound();
        }
        else
        {
            ParticleEffect.Stop();
            audioSource.Stop();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponentInChildren<PlayerBuild>().setBuildRadiusExtended(true);
            isPlayerIn = true;
            Debug.Log("i'm in");

            //if sound not playing
            // play sound
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GetComponent<BoxCollider2D>().IsTouching(collision))
            {
                collision.transform.GetComponentInChildren<PlayerBuild>().setBuildRadiusExtended(false);
                isPlayerIn = false;
                Debug.Log("i'm out");
            }

        }
    }


    private void PlaySound()
    {
        // Check if there are any jump sounds and audio source is assigned
        if (warbleSound != null && audioSource != null)
        {
            // Select a random jump sound from the array
            //AudioClip randomJumpSound = warbleSound;
            audioSource.PlayOneShot(warbleSound);
        }
        else
        {
            Debug.LogWarning("No wable sounds assigned or audio source missing!");
        }
    }


}