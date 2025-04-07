using System.Collections.Generic;
using UnityEngine;

public class SwitchController : SwitchEnum
{
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();
    private GameObject[] gameObjectsArray;

    private void Awake()
    {
        PopulateChildBlocks();
    }

    private void PopulateChildBlocks()
    {
        gameObjectsArray = GameObject.FindGameObjectsWithTag("SwitchingBlock");

        foreach (var child in gameObjectsArray)
        {
            if (child != null)
            {
                SwitchingBlock switchBlock = child.GetComponent<SwitchingBlock>();
                if (switchBlock != null && switchBlock.blockColour == blockColour)
                {
                    gameObjects.Add(child);
                }
            }
        }
    }

    public void ToggleAll(bool state)
    {
        foreach (var child in gameObjects)
        {
            if (child != null)
            {
                SwitchingBlock switchBlock = child.GetComponent<SwitchingBlock>();
                if (switchBlock != null)
                {
                    switchBlock.ToggleBlock(state);
                }
            }
        }
    }
}