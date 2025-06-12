using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{

    public static BackgroundMusic Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
