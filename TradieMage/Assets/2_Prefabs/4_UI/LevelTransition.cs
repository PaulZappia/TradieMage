using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using Kryz.Tweening;

public class LevelTransition : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas canvas;
    public Image transitionImage;
    public RectTransform rect;

    public float duration = 2.5f;
    private float imgY = -540f;
    private float imgStartX = 4607f; // 3623f
    private float imgEndX = -2687f; // -1703f
    private Vector2 startPos;
    private Vector2 endPos;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        rect = transitionImage.GetComponent<RectTransform>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //gameObject.transform.SetParent(null);
        //canvas = GetComponent<Canvas>();
        //canvas.worldCamera = mainCamera;
        startPos = new Vector2(imgStartX, imgY);
        endPos = new Vector2(imgEndX, imgY);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AnimateTransition());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(this);
        yield return null;
    }

}
