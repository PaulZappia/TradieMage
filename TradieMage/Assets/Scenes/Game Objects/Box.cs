using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerClickHandler
{
    PlayerMovement player;
    public int manaCost = 1;

    private void Start()
    {
        //Debug.Log("HELLO");
        player = GetComponent<PlayerMovement>();
    }

    private void Awake()
    {
        //Debug.Log("HI");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            player.mana += manaCost;
            Destroy(this);
    }
}
