using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject howToPlayMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        howToPlayMenu.SetActive(false);

    }


    public void Back()
    {
        {
            pauseMenu.SetActive(true);
            howToPlayMenu.SetActive(false);
        }


    }

}
