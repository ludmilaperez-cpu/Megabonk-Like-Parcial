using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja los upgrades permanentes: compra en la store, guardado y aplicacion al player.
/// Es estatico (como SaveManager) asi funciona en cualquier escena sin objetos.
/// Los niveles se guardan en SaveData.json junto con las monedas.
/// </summary>
public static class UpgradeManager
{
    public static event Action OnUpgradesChanged;

    private static UpgradeSO[] _all;

    /// <summary>Todos los upgrades definidos en Resources/Upgrades.</summary>
    public static UpgradeSO[] All
    {
        get
        {
            if (_all == null) _all = Resources.LoadAll<UpgradeSO>("Upgrades");
            return _all;
        }
    }

    // ---------- Consultas ----------

    /// <summary>Cuantas veces se compro el upgrade.</summary>
    public static int GetLevel(UpgradeSO upgrade)
    {
        var list = SaveManager.LastLoadedGameData.upgrades;
        if (list == null) return 0;
        foreach (var u in list) if (u.id == upgrade.id) return u.level;
        return 0;
    }

    /// <summary>Cantidad total que suma al stat (nivel x valor por nivel).</summary>
    public static float GetBonus(UpgradeSO upgrade) => GetLevel(upgrade) * upgrade.valuePerLevel;

    /// <summary>Precio de la proxima compra: base x multiplicador^nivel (redondeado hacia arriba).</summary>
    public static float GetPrice(UpgradeSO upgrade)
    {
        return Mathf.Ceil(upgrade.basePrice * Mathf.Pow(upgrade.priceMultiplier, GetLevel(upgrade)));
    }

    public static bool CanAfford(UpgradeSO upgrade) => SaveManager.LastLoadedGameData.coins >= GetPrice(upgrade);

    // ---------- Compra ----------

    /// <summary>Resta las monedas, sube el nivel y guarda. Devuelve false si no alcanzan las monedas.</summary>
    public static bool TryBuy(UpgradeSO upgrade)
    {
        var data = SaveManager.LastLoadedGameData;
        float price = GetPrice(upgrade);
        if (data.coins < price) return false;

        SetLevel(data, upgrade.id, GetLevel(upgrade) + 1);

        // Actualiza el display de monedas y (via SaveManager) guarda monedas + upgrades.
        EventManager.RaiseCoinsChanged(data.coins - price);
        SaveManager.Save(); // por si nadie estaba suscripto al evento

        OnUpgradesChanged?.Invoke();
        return true;
    }

    /// <summary>Avisa a la UI que los upgrades cambiaron (ej: despues de resetear el guardado).</summary>
    public static void NotifyChanged() => OnUpgradesChanged?.Invoke();

    private static void SetLevel(SaveManager.GameData data, string id, int level)
    {
        if (data.upgrades == null) data.upgrades = new List<SaveManager.UpgradeSave>();
        foreach (var u in data.upgrades)
        {
            if (u.id == id) { u.level = level; return; }
        }
        data.upgrades.Add(new SaveManager.UpgradeSave { id = id, level = level });
    }

    // ---------- Aplicacion al player ----------

    /// <summary>Suma el bonus de cada upgrade comprado al stat correspondiente del player.</summary>
    public static void ApplyTo(PlayerController player)
    {
        foreach (var upgrade in All)
        {
            float bonus = GetBonus(upgrade);
            if (bonus == 0f) continue;

            player.ModifyStat(upgrade.stat, bonus);
            Debug.Log($"[UpgradeManager] {upgrade.id}: +{bonus} a {upgrade.stat}");
        }
    }
}
