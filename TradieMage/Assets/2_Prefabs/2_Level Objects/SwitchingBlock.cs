using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SwitchingBlock : SwitchEnum
{
    //public bool defaultState;
    public List<Sprite> sprites;
    public SpriteRenderer spriteRenderer;
    //public Collision2D collision2D;
    public GameObject groundObject;
    public GameObject blockObject;

    public GameObject switchController;
    public SwitchController switchControllerScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //collision2D = this.gameObject.transform.GetChild(0).GetComponent<Collision2D>();
        //groundObject = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }

    private void Start()
    {
        switchController = GameObject.FindGameObjectWithTag(blockColour + "SwitchController");
        switchControllerScript = switchController.GetComponent<SwitchController>();
        switchControllerScript.gameObjects.Add(gameObject);
        ToggleBlock(switchControllerScript.isSwitchActive);
        
    }

    public void ToggleBlock(bool state)
    {
        isActive = state;
        //collision2D.collider.enabled = state;
        //groundObject.gameObject.SetActive(state);
        //GetComponent<BoxCollider2D>().enabled = state;
        groundObject.GetComponent<BoxCollider2D>().enabled = state;
        blockObject.GetComponent<BoxCollider2D>().enabled = state;
        Debug.Log(gameObject.name);
        //groundObject.SetActive(state);
        spriteRenderer.sprite = sprites[Convert.ToInt32(isActive)];
    }
    private void Update()
    {
        
    }

}
