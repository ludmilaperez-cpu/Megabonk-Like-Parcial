using UnityEngine;
using UnityEngine.UI;

public class Brillo : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image panelBrillo;

    void Start()
    {
        panelBrillo.enabled = false;
        slider.minValue = 0f;
        slider.maxValue = OpcionesGlobales.BRILLO_MAXIMO;
        sliderValue = OpcionesGlobales.BrilloActual;
        slider.SetValueWithoutNotify(sliderValue);
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        OpcionesGlobales.BrilloActual = value;
    }
}
