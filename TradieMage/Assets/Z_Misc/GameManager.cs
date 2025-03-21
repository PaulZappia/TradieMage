using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{



    //public List<SceneData> levels = new List<SceneData>();
    public List<string> sceneNameList = new List<string>();




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene(sceneNameList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
