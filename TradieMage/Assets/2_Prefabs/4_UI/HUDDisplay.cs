using UnityEngine;
using UnityEngine.UI;

public class HUDDisplay : MonoBehaviour
{
    //[Header("NOTE: YOU WILL HAVE TO DRAG IN THE CAMERA FROM THE SCENE TO MAKE THIS SHOW UP IN THE GAME VIEW.")]
    //public bool iUnderstand;

    [Header("Camera")]
    public Camera mainCamera;
    public Canvas canvas;

    // Player Reference
    private PlayerBuild player;


    // Block Select
    [Header("Block Select")]
    public Image selectedBox;


    [Header("Mana")]
    public int mana;
    public int maxMana = 10;

    public Sprite emptyMana;
    public Sprite fullMana;
    public Image[] availableMana;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBuild>();
        mana = player.mana;
    }

    private void Start()
    {
        canvas.worldCamera = mainCamera;
        selectedBox.sprite = player.mouseScript.blockSprites[player.selectedBox];
    }

    // Update is called once per frame
    void Update()
    {
        //box
        selectedBox.sprite = player.mouseScript.blockSprites[player.selectedBox];

        //mana
        mana = player.mana;
        
        for (int i = 0; i < availableMana.Length; i++)
        {
            if (i < mana)
            {
                availableMana[i].sprite = fullMana;
            }
            else
            {
                availableMana[i].sprite = emptyMana;
            }
            
            if (i < maxMana)
            {
                availableMana[i].enabled = true;
            }
            else
            {
                availableMana[i].enabled = false;
            }
        }
    }
}
