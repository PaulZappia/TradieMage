using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public string StartLevel = "TestLevel1";

   public void OnStartClick()
    {
        SceneManager.LoadScene(StartLevel); //load first level here or intro cutscene
    }

    public  void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
