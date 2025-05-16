using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   
    [SerializeField] GameObject pauseMenu;

    

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
        //TODO make sure not in main menu
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
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

    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }



}
