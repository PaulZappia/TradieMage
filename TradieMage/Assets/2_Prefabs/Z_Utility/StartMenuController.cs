using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
   public void OnStartClick()
    {
        SceneManager.LoadScene("TestLevel1"); //load first level here or intro cutscene
    }

    public  void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
