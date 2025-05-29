using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class RangeExtender : MonoBehaviour
{

    public ParticleSystem ParticleEffect;
    private bool isPlayerIn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (isPlayerIn)
        {
            if (!ParticleEffect.isPlaying) {
                ParticleEffect.Play();
            }
        }
        else
        {
            ParticleEffect.Stop();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponentInChildren<PlayerBuild>().setBuildRadiusExtended(true);
            isPlayerIn = true;
            Debug.Log("i'm in");

            //if sound not playing
            // play sound
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GetComponent<BoxCollider2D>().IsTouching(collision))
            {
                collision.transform.GetComponentInChildren<PlayerBuild>().setBuildRadiusExtended(false);
                isPlayerIn = false;
                Debug.Log("i'm out");
            }

        }
    }
}