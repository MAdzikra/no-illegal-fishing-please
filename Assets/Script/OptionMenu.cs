using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        InitSlider();
    }

    void OnEnable()
    {
        InitSlider();
    }

    void InitSlider()
    {
        if (AudioManager.Instance != null)
        {
            float vol = AudioManager.Instance.GetVolume();
            volumeSlider.value = vol;
            Debug.Log("Slider set to: " + vol);
        }

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }

    public void OnVolumeChange(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetVolume(value);
        }
    }
}
