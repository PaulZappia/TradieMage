using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    
    [SerializeField] private AudioMixer myMixer;
    [Header("Music Volume")]
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private int volumeMulti = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        optionsMenu.SetActive(false);
        SetMusicVolume();
        SetSoundVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("bgm", Mathf.Log10(volume)* volumeMulti);
    }

    public void SetSoundVolume()
    {
        float volume = soundSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * volumeMulti);
    }

    public void Back()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            optionsMenu.SetActive(false);

        }
        else
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
       

    }



}
