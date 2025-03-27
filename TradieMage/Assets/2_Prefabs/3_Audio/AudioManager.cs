using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //public List<AudioClip> jump = new List<AudioClip>();
    public AudioSource sfxSource;

    public AudioClip sfxJump;


    public void PlaySound(AudioClip sound)
    {
        sfxSource.PlayOneShot(sound);
    }
}
