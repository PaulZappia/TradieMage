using UnityEngine;

public class Percistent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
