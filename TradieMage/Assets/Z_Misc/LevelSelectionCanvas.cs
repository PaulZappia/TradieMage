using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelSelectionCanvas : MonoBehaviour
{

    public Image leftButton;
    public Image rightButton;


    public Canvas canvas;
    public Image transitionImage;
    public RectTransform rect;

    public float duration = 2.5f;
    private float imgY = -540f;
    private float imgStartX = 4607f; // 3623f
    private float imgEndX = -2687f; // -1703f
    private Vector2 startPos;
    private Vector2 endPos;

    void Awake()
    {

    }

    void Update()
    {

    }

    void Start()
    {
        //var buttons = GetComponentsInChildren<Image>();
        //leftButton = buttons[0];
        //rightButton = buttons[1];
    }

    public void RightButton()
    {
        StartCoroutine(AnimateTransition());
    }

    public void LeftButton()
    {
        StartCoroutine(AnimateTransition());
    }


    IEnumerator AnimateTransition()
    {
        float time = 0f;

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
        Destroy(this.gameObject);
        yield return null;
    }


}