using System;
using System.Collections.Generic;
using UnityEngine;


public static class UpgradeManager
{
    public static event Action OnUpgradesChanged;

    private static UpgradeSO[] _all;


    public static UpgradeSO[] All
    {
        get
        {
            if (_all == null) _all = Resources.LoadAll<UpgradeSO>("Upgrades");
            return _all;
        }
    }

   
    public static int GetLevel(UpgradeSO upgrade)
    {
        var list = SaveManager.LastLoadedGameData.upgrades;
        if (list == null) return 0;
        foreach (var u in list) if (u.id == upgrade.id) return u.level;
        return 0;
    }


    public static float GetBonus(UpgradeSO upgrade) => GetLevel(upgrade) * upgrade.valuePerLevel;


    public static float GetPrice(UpgradeSO upgrade)
    {
        return Mathf.Ceil(upgrade.basePrice * Mathf.Pow(upgrade.priceMultiplier, GetLevel(upgrade)));
    }

    public static bool CanAfford(UpgradeSO upgrade) => SaveManager.LastLoadedGameData.coins >= GetPrice(upgrade);


    public static bool TryBuy(UpgradeSO upgrade)
    {
        var data = SaveManager.LastLoadedGameData;
        float price = GetPrice(upgrade);
        if (data.coins < price) return false;

        SetLevel(data, upgrade.id, GetLevel(upgrade) + 1);


        EventManager.RaiseCoinsChanged(data.coins - price);
        SaveManager.Save(); 
        OnUpgradesChanged?.Invoke();
        return true;
    }

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

    public static void ApplyTo(PlayerController player)
    {
        foreach (var upgrade in All)
        {
            float bonus = GetBonus(upgrade);
            if (bonus == 0f) continue;

            player.ModifyStat(upgrade.stat, bonus, upgrade.isPercentage);
            Debug.Log($"[UpgradeManager] {upgrade.id}: +{bonus} a {upgrade.stat}");
        }
    }
}
