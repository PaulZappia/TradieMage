using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndSequence : MonoBehaviour
{
    public string levelName;
    [SerializeField] private bool isLevelFinished = false;
    [SerializeField] private float endTimer = 5;
    //public ParticleSystem = particles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelFinished)
        {
            if (endTimer > 0)
            {
                endTimer -= Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(levelName);
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLevelFinished = true;
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }


}
