using UnityEngine;
using UnityEngine.UI;

public class Volumen : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image imageMute;

    void Start()
    {
        sliderValue = OpcionesGlobales.VolumenActual;
        slider.SetValueWithoutNotify(sliderValue);
        ImMute();
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        OpcionesGlobales.VolumenActual = value;
        ImMute();
    }

    public void ImMute()
    {
        imageMute.enabled = sliderValue == 0;
    }
}
