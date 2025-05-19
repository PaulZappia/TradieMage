using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenuController : MonoBehaviour
{
    public string StartLevel = "TestLevel1";

    public GameManager GM;
    public Canvas optionsMenu;
    public Canvas optionsCanvas;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        //GM.optionsMenu = GetComponent<Canvas>();
        //optionsMenu = GM.optionsMenu;
        //optionsCanvas = optionsMenu.GetComponent<Canvas>();
    }


    public void OnStartClick()
    {
        SceneManager.LoadScene(StartLevel); //load first level here or intro cutscene
    }

    public void Options()
    {
        //gameObject.SetActive(false);
        //optionsMenu.enabled = true;
        //GameObject.Find("StartMenuController").optionsMenu.gameObject.SetActive(true);
        GM.setOptionsMenu(true);
        //Debug.Log(optionsMenu.enabled);
    }

    public  void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
