using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDD;
    List<Resolution> resolutions = new List<Resolution>();

    void OnEnable()
    {
        toggle.SetIsOnWithoutNotify(OpcionesGlobales.PantallaCompleta);
        CheckResolutions();
    }

    public void ChangeToggle(bool value)
    {
        OpcionesGlobales.PantallaCompleta = value;
    }

    public void CheckResolutions()
    {
        resolutions.Clear();
        resolucionesDD.ClearOptions();
        List<string> opciones = new List<string>();
        Vector2Int actual = OpcionesGlobales.ResolucionActual;
        int resolucionActual = 0;
        foreach (Resolution resolucion in Screen.resolutions)
        {
            if (resolutions.Exists(r => r.width == resolucion.width && r.height == resolucion.height)) continue;
            resolutions.Add(resolucion);
            opciones.Add(resolucion.width + " x " + resolucion.height);
            if (resolucion.width == actual.x && resolucion.height == actual.y)
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
        OpcionesGlobales.SetResolucion(resolucion.width, resolucion.height);
    }
}
