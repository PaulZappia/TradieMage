using System.Collections;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public Camera mainCamera;
    private float imgX = 3623f;
    private float imgY = -540f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void easeInSine(x)
    {
        return 1 - Math.cos((x * Math.PI) / 2);
    }

    IEnumerator

}
