using UnityEngine;
using UnityEngine.UI;

public static class OpcionesGlobales
{
    const string VOLUMEN_KEY = "volumen";
    const string BRILLO_KEY = "brillo";
    public const float VOLUMEN_INICIAL = 1f;
    public const float BRILLO_INICIAL = 0.5f;
    public const float BRILLO_MAXIMO = 0.9f;

    static Image overlay;

    public static float VolumenActual
    {
        get { return PlayerPrefs.GetFloat(VOLUMEN_KEY, VOLUMEN_INICIAL); }
        set
        {
            PlayerPrefs.SetFloat(VOLUMEN_KEY, value);
            AudioListener.volume = value;
        }
    }

    public static float BrilloActual
    {
        get { return PlayerPrefs.GetFloat(BRILLO_KEY, BRILLO_INICIAL); }
        set
        {
            PlayerPrefs.SetFloat(BRILLO_KEY, value);
            ApplyBrillo(value);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        AudioListener.volume = VolumenActual;
        CreateOverlay();
        ApplyBrillo(BrilloActual);
    }

    static void CreateOverlay()
    {
        if (overlay != null) return;

        GameObject canvasObject = new GameObject("BrilloOverlay", typeof(Canvas));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;
        Object.DontDestroyOnLoad(canvasObject);

        GameObject imageObject = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.SetParent(canvasObject.transform, false);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        overlay = imageObject.GetComponent<Image>();
        overlay.color = Color.black;
        overlay.raycastTarget = false;
    }

    static void ApplyBrillo(float value)
    {
        if (overlay == null) return;
        Color color = overlay.color;
        color.a = BRILLO_MAXIMO - value;
        overlay.color = color;
    }
}
