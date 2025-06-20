using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject howToPlayMenu;
    public GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        //make sure not in main menu
        
        //if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        if (GameManager.player != null)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);

    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    public void HowToPlay()
    {
        pauseMenu.SetActive(false);
        howToPlayMenu.SetActive(true);

    }



}
