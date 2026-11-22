using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Datos de un item pasivo. Se crea desde Assets > Create > Items > Item.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    [Tooltip("Identificador unico, se usa para guardar si el item esta comprado")]
    public string id;
    public string itemName;
    [TextArea(2, 5)]
    [Tooltip("Descripcion que se muestra en la store al tocar Desc.")]
    public string desc;
    public Sprite icon;

    [Tooltip("Cuanto modifica el stat (ej: +2 de velocidad)")]
    public float modifierValue;

    [Tooltip("Prefab que aparece en el mundo cuando un enemigo suelta el item")]
    public GameObject dropPrefab;

    [Tooltip("Cuantas monedas cuesta desbloquearlo en la store")]
    public float price = 50f;

    [Tooltip("Si esta marcado, el item puede caer sin haberlo comprado")]
    public bool unlockedByDefault;

    public bool IsUnlocked
    {
        get
        {
            if (unlockedByDefault) return true;
            var unlocked = SaveManager.LastLoadedGameData.unlockedItems;
            return unlocked != null && unlocked.Contains(id);
        }
    }

    public void Unlock()
    {
        var data = SaveManager.LastLoadedGameData;
        if (data.unlockedItems == null) data.unlockedItems = new List<string>();
        if (data.unlockedItems.Contains(id)) return;

        data.unlockedItems.Add(id);
        SaveManager.Save();
    }
}
