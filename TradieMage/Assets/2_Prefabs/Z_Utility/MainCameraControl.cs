using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainCameraControl : MonoBehaviour
{
    public bool isPixelPerfect = false;
    const int cameraSize = 9;

    public Camera mainCamera;
    public PixelPerfectCamera pixelCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        pixelCamera = GetComponent<PixelPerfectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    setPixelPerfect(!isPixelPerfect);
        //}
    }



    void setPixelPerfect(bool newSetting)
    {
        isPixelPerfect = newSetting;
        if (newSetting)
        {
            pixelCamera.enabled = true;
        }
        else
        {
            pixelCamera.enabled = false;
            mainCamera.orthographicSize = cameraSize;
        }
    }

}
