using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SwitchController : SwitchEnum
{
    public List<GameObject> gameObjects;
    //public GameObject[] gameObjectsArray;
    public bool isSwitchActive = false;
    private bool previousSwitchActiveState = false;
    public bool invertBlocks = false;

    private void Awake()
    {
        //PopulateChildBlocks();
        previousSwitchActiveState = isSwitchActive;
        gameObject.tag = blockColour + "SwitchController";
    }
    /*
    private void Start()
    {
        StartCoroutine(waitToToggle());
    }
    */

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);
        ToggleAll(isSwitchActive);
        Debug.Log("ToggleAll");
    }


    private void PopulateChildBlocks()
    {
        //gameObjectsArray = GameObject.FindGameObjectsWithTag("SwitchingBlock");
        
        /*
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
        */
        
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
                    switchBlock.ToggleBlock(state ^ invertBlocks);
                }
            }
        }
    }

    public void Update()
    {
        isSwitchActive = isActive;
        if (isSwitchActive != previousSwitchActiveState) 
        {
            ToggleAll(isSwitchActive);
            previousSwitchActiveState = isSwitchActive;
        }
    }

}
