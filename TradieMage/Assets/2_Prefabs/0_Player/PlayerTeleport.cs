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

    public AudioSource audioSource;
    public AudioClip teleportUseSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTeleporter = Physics2D.OverlapBox(transform.position, TeleportCollider, 0, TeleportLayer).gameObject.GetComponentInParent<Teleporter>().gameObject;

            if (currentTeleporter != null)
            //if (Physics2D.OverlapBox(transform.position, TeleportCollider, 0, TeleportLayer))
            {
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
                teleportEffect.Play();
                audioSource.PlayOneShot(teleportUseSound);
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
