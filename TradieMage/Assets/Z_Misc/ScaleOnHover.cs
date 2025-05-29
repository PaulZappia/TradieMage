using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

//mostly Pauls code

public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{


    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public float scaleSpeed = 8f;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    private Vector3 originalScale;
    private Vector3 targetScale;


    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        targetScale = originalScale * hoverScale;



        // Show Level Image but delayed
        //levelDisplayImage.enabled = true;

        //StartCoroutine(ShowLevelDelayed());


        PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        targetScale = originalScale * clickScale;
        PlaySound(clickSound);
        //slide panel

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        targetScale = originalScale * hoverScale; // Return to hover size
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}