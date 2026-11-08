using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDD;
    List<Resolution> resolutions = new List<Resolution>();

    void Start()
    {
        toggle.SetIsOnWithoutNotify(Screen.fullScreen);
        CheckResolutions();
    }

    public void ChangeToggle(bool value)
    {
        Screen.fullScreen = value;
    }

    public void CheckResolutions()
    {
        resolutions.Clear();
        resolucionesDD.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;
        foreach (Resolution resolucion in Screen.resolutions)
        {
            if (resolutions.Exists(r => r.width == resolucion.width && r.height == resolucion.height)) continue;
            resolutions.Add(resolucion);
            opciones.Add(resolucion.width + " x " + resolucion.height);
            if (resolucion.width == Screen.width && resolucion.height == Screen.height)
            {
                resolucionActual = resolutions.Count - 1;
            }
        }
        resolucionesDD.AddOptions(opciones);
        resolucionesDD.SetValueWithoutNotify(resolucionActual);
        resolucionesDD.RefreshShownValue();
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        Resolution resolucion = resolutions[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }
}
