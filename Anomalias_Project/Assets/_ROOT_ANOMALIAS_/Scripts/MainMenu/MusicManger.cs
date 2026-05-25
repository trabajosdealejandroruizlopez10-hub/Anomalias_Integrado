using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public AudioSource musicSource;

    void Start()
    {
        slider.value = musicSource.volume;

        slider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float value)
    {
        musicSource.volume = value;
    }
}