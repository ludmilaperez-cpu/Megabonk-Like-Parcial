using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Maneja que pantalla (panel de UI) esta visible dentro de una escena.
/// Cada escena tiene su propio ScreenManager con su lista de pantallas.
/// Solo hay una pantalla activa a la vez y se recuerda el historial para poder volver con Back().
/// </summary>
public class ScreenManager : MonoBehaviour
{
    [Serializable]
    public class ScreenEntry
    {
        public string id;
        public GameObject root;
    }

    [SerializeField] List<ScreenEntry> screens = new List<ScreenEntry>();
    [Tooltip("Pantalla que se muestra al iniciar. Vacio = ninguna.")]
    [SerializeField] string startScreen;
    [Tooltip("Si esta activo, Escape vuelve a la pantalla anterior.")]
    [SerializeField] bool backOnEscape;

    private readonly Stack<string> _history = new Stack<string>();

    /// <summary>Id de la pantalla visible (null si no hay ninguna).</summary>
    public string Current { get; private set; }

    public event Action<string> OnScreenChanged;

    private void Awake()
    {
        Apply(string.IsNullOrEmpty(startScreen) ? null : startScreen);
    }

    private void Update()
    {
        if (!backOnEscape || _history.Count == 0) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) Back();
    }

    /// <summary>Muestra la pantalla con ese id y guarda la actual en el historial.</summary>
    public void Show(string id)
    {
        if (id == Current) return;
        if (!Has(id))
        {
            Debug.LogWarning($"[ScreenManager] No existe la pantalla '{id}'", this);
            return;
        }

        if (Current != null) _history.Push(Current);
        Apply(id);
    }

    /// <summary>Vuelve a la pantalla anterior. Si no hay historial, no hace nada.</summary>
    public void Back()
    {
        if (_history.Count == 0) return;
        Apply(_history.Pop());
    }

    /// <summary>Oculta todas las pantallas y borra el historial.</summary>
    public void HideAll()
    {
        _history.Clear();
        Apply(null);
    }

    public bool Has(string id)
    {
        foreach (var s in screens) if (s.id == id) return true;
        return false;
    }

    private void Apply(string id)
    {
        foreach (var s in screens)
        {
            if (s.root != null) s.root.SetActive(s.id == id);
        }
        Current = id;
        OnScreenChanged?.Invoke(id);
    }
}
