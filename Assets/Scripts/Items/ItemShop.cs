using System;

public static class ItemShop
{
    public static event Action OnItemsChanged;

    public static bool CanAfford(ItemSO item) => SaveManager.LastLoadedGameData.coins >= item.price;

    public static bool TryBuy(ItemSO item)
    {
        if (item.IsUnlocked) return false;

        var data = SaveManager.LastLoadedGameData;
        if (data.coins < item.price) return false;

        item.Unlock();
        EventManager.RaiseCoinsChanged(data.coins - item.price);
        OnItemsChanged?.Invoke();
        return true;
    }

    public static void NotifyChanged() => OnItemsChanged?.Invoke();
}
