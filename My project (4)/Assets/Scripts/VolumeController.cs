using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    public string parameterName = "MyExposedVolume";

    public Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    
        if (slider != null) 
        {
            slider.minValue = -80f;
            slider.maxValue = 0f;
            slider.onValueChanged.AddListener(SetVolume);
        
            float currentVol;
            mixer.GetFloat(parameterName, out currentVol);
            slider.value = currentVol;
        }
    }


    public void SetVolume(float value)
    {
        mixer.SetFloat(parameterName, value);
    }
}