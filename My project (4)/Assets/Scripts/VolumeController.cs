using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    public string parameterName = "MyExposedVolume";

    private Slider slider;

    void Start()
    {
        slider.minValue = -80f;
        slider.maxValue = 0f;

        slider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat(parameterName, value);
    }
}