using UnityEngine;

public class NailButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isPressed = false;
    public SpriteRenderer buttonSprite;
    public Sprite buttonActive;
    public Sprite buttonInactive;
    public Animator buttonAnimator;

    public ToggleObject connectedObject1;
    public ToggleObject connectedObject2;
    public ToggleObject connectedObject3;
    //public GameObject connectedObject1;

    void Start()
    {
        buttonSprite = GetComponentInChildren<SpriteRenderer>();
        //buttonAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        buttonAnimator.Play(Animator.StringToHash("NailButtonRaised"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!isPressed && collision.gameObject.CompareTag("BoxTag"))
        {
            //Debug.Log("yay");
            isPressed = true;
            buttonAnimator.StopPlayback();
            buttonAnimator.SetBool("Pressed", isPressed);
            buttonAnimator.Play(Animator.StringToHash("NailButtonRaised"), -1, 0.7f);

            if (connectedObject1 != null)
            {
                connectedObject1.isActive = true;
            }
            if (connectedObject2 != null)
            {
                connectedObject2.isActive = true;
            }
            if (connectedObject3 != null)
            {
                connectedObject3.isActive = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPressed && collision.gameObject.CompareTag("BoxTag"))
        {
            //Debug.Log("nay :(");
            isPressed = false;
            buttonAnimator.StopPlayback();
            buttonAnimator.SetBool("Pressed",isPressed);
            buttonAnimator.Play(Animator.StringToHash("NailButtonDown"), -1, 0.7f);

            if (connectedObject1 != null)
            {
                connectedObject1.isActive = false;
            }
            if (connectedObject2 != null)
            {
                connectedObject2.isActive = false;
            }
            if (connectedObject3 != null)
            {
                connectedObject3.isActive = false;
            }
        }
    }


}
