using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public LayerMask TeleportLayer;
    public Vector2 TeleportCollider = new Vector2(1.5f, 1.5f);
    private GameObject currentTeleporter;
    public ParticleSystem teleportEffect;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            var boxCheck = Physics2D.OverlapBox(transform.position, TeleportCollider, 0, TeleportLayer);

            if (boxCheck != null)
            //if (Physics2D.OverlapBox(transform.position, TeleportCollider, 0, TeleportLayer))
            {
                currentTeleporter = boxCheck.gameObject.GetComponentInParent<Teleporter>().gameObject;
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
                teleportEffect.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            //currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                //currentTeleporter = null;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, TeleportCollider);
    }

}
