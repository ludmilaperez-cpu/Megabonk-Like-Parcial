using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ponerlo en un Button. Al tocarlo, todo lo guardado en SaveData.json vuelve a 0
/// (monedas y niveles de upgrades) y la UI se actualiza.
/// </summary>
[RequireComponent(typeof(Button))]
public class ResetSaveButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ResetSave);
    }

    public void ResetSave()
    {
        SaveManager.ResetAll();
        EventManager.RaiseCoinsChanged(0f);   // actualiza el display de monedas
        UpgradeManager.NotifyChanged();       // actualiza precios y botones de la store
        ItemShop.NotifyChanged();             // vuelve a bloquear los items comprados
        Debug.Log("[ResetSaveButton] Datos guardados reseteados");
    }
}
