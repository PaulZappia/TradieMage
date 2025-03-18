using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SwitchingBlock : ToggleObject
{
    //public bool defaultState;
    public List<Sprite> sprites;
    public SpriteRenderer spriteRenderer;
    public Collision2D collision2D;
    public GameObject groundObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ToggleBlock(isActive);
    }

    void ToggleBlock(bool state)
    {
        collision2D.collider.enabled = state;
        spriteRenderer.sprite = sprites[Convert.ToInt32(isActive)];
    }
    private void Update()
    {
        
    }

}
