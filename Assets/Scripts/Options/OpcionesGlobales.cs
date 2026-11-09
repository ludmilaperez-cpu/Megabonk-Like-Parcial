using UnityEngine;
using UnityEngine.UI;

public static class OpcionesGlobales
{
    const string VOLUMEN_KEY = "volumen";
    const string BRILLO_KEY = "brillo";
    const string PANTALLA_COMPLETA_KEY = "pantallaCompleta";
    const string ANCHO_KEY = "resolucionAncho";
    const string ALTO_KEY = "resolucionAlto";
    public const float VOLUMEN_INICIAL = 1f;
    public const float BRILLO_INICIAL = 0.5f;
    public const float BRILLO_MAXIMO = 0.9f;
    public const bool PANTALLA_COMPLETA_INICIAL = true;

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

    public static bool PantallaCompleta
    {
        get { return PlayerPrefs.GetInt(PANTALLA_COMPLETA_KEY, PANTALLA_COMPLETA_INICIAL ? 1 : 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(PANTALLA_COMPLETA_KEY, value ? 1 : 0);
            ApplyPantalla();
        }
    }

    public static Vector2Int ResolucionActual
    {
        get
        {
            Resolution nativa = Screen.currentResolution;
            return new Vector2Int(PlayerPrefs.GetInt(ANCHO_KEY, nativa.width), PlayerPrefs.GetInt(ALTO_KEY, nativa.height));
        }
    }

    public static void SetResolucion(int ancho, int alto)
    {
        PlayerPrefs.SetInt(ANCHO_KEY, ancho);
        PlayerPrefs.SetInt(ALTO_KEY, alto);
        ApplyPantalla();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        AudioListener.volume = VolumenActual;
        CreateOverlay();
        ApplyBrillo(BrilloActual);
        ApplyPantalla();
    }

    static void ApplyPantalla()
    {
        Vector2Int resolucion = ResolucionActual;
        FullScreenMode modo = PantallaCompleta ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolucion.x, resolucion.y, modo);
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
