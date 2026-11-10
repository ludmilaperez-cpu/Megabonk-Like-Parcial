using UnityEngine;

/// <summary>
/// Datos de un item pasivo. Se crea desde Assets > Create > Items > Item.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [Tooltip("Cuanto modifica el stat (ej: +2 de velocidad)")]
    public float modifierValue;
}
