using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Globalization;
//using UnityEngine.UIElements;

public class LevelSelectionCanvas : MonoBehaviour
{

    public Image leftButton;
    public Image rightButton;


    public Canvas canvas;
    //public IPanel menuPanel;
    public RectTransform rect;
    public int panelOffset = 1920;
    public float duration = 2.5f;
    private float imgY = -540f;
    private float imgStartX = 4607f; // 3623f
    private float imgEndX = -2687f; // -1703f
    private Vector2 startPos;
    private Vector2 endPos;
    private int panelCount;
    private int currentPanel = 0;
    private bool isAnimating = false;

    void Awake()
    {

    }

    void Update()
    {
        //rect.anchoredPosition += Vector2.left;
    }

    void Start()
    {
        //var buttons = GetComponentsInChildren<Image>();
        //leftButton = buttons[0];
        //rightButton = buttons[1];
        startPos = rect.anchoredPosition;
        endPos = rect.anchoredPosition;
        //targetPos = startPos;
        panelCount = rect.childCount;
        Debug.Log(panelCount);
        CheckIfLastPanel();
    }

    public void RightButton()
    {
        if (isAnimating)
            return;
        startPos = rect.anchoredPosition;
        endPos.x = startPos.x - panelOffset;
        StartCoroutine(AnimateTransition());
        currentPanel++;
        CheckIfLastPanel();
    }

    public void LeftButton()
    {
        if (isAnimating)
            return;
        startPos = rect.anchoredPosition;
        endPos.x = startPos.x + panelOffset;
        StartCoroutine(AnimateTransition());
        currentPanel--;
        CheckIfLastPanel();
    }

    public void CheckIfLastPanel()
    {
        if (currentPanel <= 0)
        {
            currentPanel = 0;
            leftButton.gameObject.SetActive(false);
            Debug.Log("left disappear");
        }
        else if (currentPanel >= panelCount - 1)
        {
            currentPanel = panelCount - 1;
            rightButton.gameObject.SetActive(false);
            Debug.Log("right disappear");
        }
        else
        {
            leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(true);
            Debug.Log("both return");
        }
    }



    IEnumerator AnimateTransition()
    {
        float time = 0f;
        isAnimating = true;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Ease-in
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            //transitionImage.transform.position = Vector2.Lerp(startPos, endPos, easedT);

            yield return null;
        }

        // Set at final position just to make sure dawg
        rect.anchoredPosition = endPos;
        //transitionImage.transform.position = endPos;
        //Destroy(this.gameObject);
        isAnimating = false;
        yield return null;
    }


    public void OnBackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

}