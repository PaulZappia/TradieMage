using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string levelDestination;

    [SerializeField] private bool isLocked = false;
    [SerializeField] private GameObject lockOverlay;

    [SerializeField] private Sprite defaultCardSprite;
    [SerializeField] private Sprite hoverCardSprite;
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public float scaleSpeed = 8f;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    [SerializeField] private Image cardImage;
    [SerializeField] private Image levelDisplayImage;
    private Vector3 originalScale;
    private Vector3 targetScale;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Color defaultTextColour; // default: #cb4f51
    [SerializeField] private Color hoverTextColour; // default: #4234aa

    [SerializeField] private GameObject levelTransition;

    private Coroutine showLevelDisplayCoroutine;

    void Awake()
    {
        if (levelText != null)
            levelText.color = defaultTextColour;

        cardImage.sprite = defaultCardSprite;
        levelDisplayImage.enabled = false;
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
        if (lockOverlay != null)
        {
            lockOverlay.SetActive(isLocked);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;

        cardImage.sprite = hoverCardSprite;
        targetScale = originalScale * hoverScale;

        if (levelText != null)
            levelText.color = hoverTextColour;

        // Show Level Image but delayed
        //levelDisplayImage.enabled = true;

        //StartCoroutine(ShowLevelDelayed());

        if (showLevelDisplayCoroutine != null)
        {
            StopCoroutine(showLevelDisplayCoroutine);
        }
        showLevelDisplayCoroutine = StartCoroutine(ShowLevelDelayed());

        PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;

        cardImage.sprite = defaultCardSprite;
        targetScale = originalScale;

        if (levelText != null)
            levelText.color = defaultTextColour;

        if (showLevelDisplayCoroutine != null)
        {
            StopCoroutine(showLevelDisplayCoroutine);
            showLevelDisplayCoroutine = null;
        }

        //StopCoroutine(ShowLevelDelayed());
        levelDisplayImage.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;

        targetScale = originalScale * clickScale;
        PlaySound(clickSound);
        //SceneManager.LoadScene(levelDestination);
        StartCoroutine(LoadLevel());

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked) return;

        targetScale = originalScale * hoverScale; // Return to hover size
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator ShowLevelDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        levelDisplayImage.enabled = true;
    }

    IEnumerator LoadLevel()
    {
        Instantiate(levelTransition);
        yield return new WaitForSeconds(.5f);
        if (string.IsNullOrEmpty(levelDestination))
        {
            Debug.LogWarning("No next level name specified! Defaulting to Title Screen!");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene(levelDestination);
        }
        
    }
}
