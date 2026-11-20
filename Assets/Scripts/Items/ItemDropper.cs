using System.Collections.Generic;
using UnityEngine;

public static class ItemDropper
{
    private const string TableName = "ItemDropTable";
    private const float SpawnHeight = 0.5f;

    private static ItemDropTable _table;
    private static readonly List<ItemSO> _pool = new List<ItemSO>();

    public static void TryDrop(Vector3 position)
    {
        if (_table == null) _table = Resources.Load<ItemDropTable>(TableName);
        if (_table == null) return;

        if (Random.value >= _table.dropChance) return;

        _table.GetUnlocked(_pool);
        if (_pool.Count == 0) return;

        ItemSO item = _pool[Random.Range(0, _pool.Count)];
        if (item.dropPrefab == null) return;

        Object.Instantiate(item.dropPrefab, position + Vector3.up * SpawnHeight, Quaternion.identity);
        Debug.Log($"[ItemDropper] Solto {item.itemName} (chance {1f / _pool.Count:P0} entre {_pool.Count} items)");
    }
}
