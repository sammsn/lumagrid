using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip onClick;
    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;

    public void PlayOnClick()
    {
        if (onClick)
            audioSource.PlayOneShot(onClick);
    }
    public void PlayFlip()
    {
        if (flipClip)
            audioSource.PlayOneShot(flipClip);
    }
    public void PlayMatch()
    {
        if (matchClip)
            audioSource.PlayOneShot(matchClip);
    }
    public void PlayMismatch()
    {
        if (mismatchClip)
            audioSource.PlayOneShot(mismatchClip);
    }
    public void PlayGameOver()
    {
        if (gameOverClip)
            audioSource.PlayOneShot(gameOverClip);
    }
}
