using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Inventory/Storage")]
public class InventoryData : ScriptableObject
{
    [Header("Currency")]
    public int coins = 0;

    [Header("Items")]
    public List<KeyItem> keys = new List<KeyItem>();

    // Helper to reset data (useful for debugging or new game)
    public void ResetData()
    {
        coins = 0;
        keys.Clear();
    }
}