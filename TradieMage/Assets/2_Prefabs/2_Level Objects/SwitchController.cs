using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : SwitchEnum
{
    public List<GameObject> gameObjects;
    public GameObject[] gameObjectsArray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        PopulateChildBlocks();
    }

    void PopulateChildBlocks()
    {
        gameObjectsArray = GameObject.FindGameObjectsWithTag("SwitchingBlock");
        
        foreach (var child in gameObjectsArray) 
        { 
            if (child.GetComponent<SwitchingBlock>().blockColour == blockColour)
            {
                gameObjects.Add(child);
            } 
            else if (child == null)
            {
                return;
            }
        }
        
    }

    public void ToggleAll(bool state)
    {
        foreach (var child in gameObjects)
        {
            child.GetComponent<SwitchingBlock>().ToggleBlock(state);
        }
    }
}
