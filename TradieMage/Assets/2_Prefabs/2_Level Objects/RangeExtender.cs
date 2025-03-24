using Unity.VisualScripting;
using UnityEngine;

public class RangeExtender : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponentInChildren<PlayerBuild>().setBuildRadiusExtended(true);
            Debug.Log("i'm in");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GetComponent<BoxCollider2D>().IsTouching(collision))
            {
                collision.transform.GetComponentInChildren<PlayerBuild>().setBuildRadiusExtended(false);
                Debug.Log("i'm out");
            }
            
        }
    }

}
