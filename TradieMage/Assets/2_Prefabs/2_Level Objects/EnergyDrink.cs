using UnityEngine;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using System;




public class EnergyDrink : MonoBehaviour
{

    public int manaGain = 1;
    public bool isCollected;
    public void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            collision.gameObject.GetComponent<PlayerBuild>().mana+=manaGain;
            GetComponentInChildren<ParticleSystem>().Play();
            Destroy(gameObject, 2);
        }
    }
}