using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource.loop = true;
            audioSource.playOnAwake = true;

            if (!audioSource.isPlaying)
                audioSource.Play();

            Debug.Log("AudioManager Initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            Debug.Log("SetVolume: " + volume);
        }
    }

    public float GetVolume()
    {
        if (audioSource != null)
        {
            Debug.Log("GetVolume: " + audioSource.volume);
        }
        return audioSource != null ? audioSource.volume : 0f;
    }
}
