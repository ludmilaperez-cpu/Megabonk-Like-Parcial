using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDropTable", menuName = "Items/Drop Table")]
public class ItemDropTable : ScriptableObject
{
    [Range(0f, 1f)] public float dropChance = 0.2f;
    public List<ItemSO> items = new List<ItemSO>();

    public void GetUnlocked(List<ItemSO> result)
    {
        result.Clear();
        foreach (var item in items)
        {
            if (item != null && item.IsUnlocked) result.Add(item);
        }
    }
}
