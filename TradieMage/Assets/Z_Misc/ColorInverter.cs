using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class ColorInverter : MonoBehaviour
{
    // Singleton pattern for easy access
    public static ColorInverter Instance { get; private set; }

    [Header("Inversion Settings")]
    [Range(0f, 1f)]
    public float inversionStrength = 1.0f;
    
    [Tooltip("How long the inversion transition takes")]
    public float transitionDuration = 0.5f;
    
    // Reference to the material that will handle color inversion
    private Material inversionMaterial;
    
    // Whether colors are currently inverted
    private bool _isInverted = false;
    public bool IsInverted => _isInverted;
    
    // Property to check and smoothly animate transition
    private float _currentInversionValue = 0f;
    
    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        
        // Create the inversion material with the shader
        inversionMaterial = new Material(Shader.Find("Hidden/ColorInversion"));
        
        // Ensure the shader was loaded correctly
        if (inversionMaterial == null)
        {
            Debug.LogError("Color Inversion Shader not found!");
            enabled = false;
        }
        
        // Subscribe to scene change event to reset state
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Apply the inversion effect with current strength
        inversionMaterial.SetFloat("_InversionStrength", _currentInversionValue);
        Graphics.Blit(source, destination, inversionMaterial);
    }
    
    // Public method to toggle color inversion with smooth transition
    public void ToggleInversion()
    {
        _isInverted = !_isInverted;
        StopAllCoroutines();
        StartCoroutine(TransitionInversion(_isInverted ? inversionStrength : 0f));
    }
    
    // Set a specific inversion state
    public void SetInversion(bool inverted)
    {
        if (_isInverted != inverted)
        {
            _isInverted = inverted;
            StopAllCoroutines();
            StartCoroutine(TransitionInversion(_isInverted ? inversionStrength : 0f));
        }
    }
    
    // Smoothly transition between normal and inverted colors
    private IEnumerator TransitionInversion(float targetValue)
    {
        float startValue = _currentInversionValue;
        float elapsed = 0f;
        
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            
            // Use smooth step for easing
            t = t * t * (3f - 2f * t);
            
            _currentInversionValue = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }
        
        _currentInversionValue = targetValue;
    }
    
    // Reset inversion when scene changes
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset inversion when a new scene loads
        if (_isInverted)
        {
            _isInverted = false;
            _currentInversionValue = 0f;
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from scene change event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
