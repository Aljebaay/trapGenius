using UnityEngine;
using System.Collections.Generic;
using System.IO; // Required for file saving

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Data Storage")]
    [SerializeField] private InventoryData inventory;

    // Events to update UI automatically
    public delegate void OnCoinChange(int amount);
    public static event OnCoinChange onCoinChanged;
    public static event System.Action<int> onKeysChanged;

    private string savePath;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Define where to save the file
            savePath = Application.persistentDataPath + "/savegame.json";
            
            // Load data immediately on startup
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- COIN LOGIC ---

    public void AddCoins(int amount)
    {
        inventory.coins += amount;
        onCoinChanged?.Invoke(inventory.coins); // Tell UI to update
        SaveInventory(); // Save immediately
    }

    public bool SpendCoins(int amount)
    {
        if (inventory.coins >= amount)
        {
            inventory.coins -= amount;
            onCoinChanged?.Invoke(inventory.coins);
            SaveInventory();
            return true;
        }
        return false;
    }

    public int GetCoinCount() => inventory.coins;

    // --- KEY LOGIC ---

    public void AddKey(KeyItem key)
    {
        if (!inventory.keys.Contains(key))
        {
            inventory.keys.Add(key);
            onKeysChanged?.Invoke(inventory.keys.Count); 
            SaveInventory(); // Added Save here so keys persist
        }
    }

    public bool HasKey(KeyItem key)
    {
        return inventory.keys.Contains(key);
    }
    
    public void RemoveKey(KeyItem key)
    {
        if (inventory.keys.Contains(key))
        {
            inventory.keys.Remove(key);
            onKeysChanged?.Invoke(inventory.keys.Count); 
            SaveInventory(); // Added Save here so removal persists
        }
    }

    public int GetKeyCount() => inventory.keys.Count;

    // --- SAVE / LOAD SYSTEM (JSON) ---

    [ContextMenu("Force Save")]
    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(inventory);
        // Ensure path is set if called from Editor
        if (string.IsNullOrEmpty(savePath)) savePath = Application.persistentDataPath + "/savegame.json";
        
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved.");
    }

    [ContextMenu("Force Load")]
    public void LoadInventory()
    {
        // Ensure path is set if called from Editor
        if (string.IsNullOrEmpty(savePath)) savePath = Application.persistentDataPath + "/savegame.json";

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, inventory);
            
            // Update UI after load
            if (Application.isPlaying)
            {
                onCoinChanged?.Invoke(inventory.coins);
                onKeysChanged?.Invoke(inventory.keys.Count);
            }
        }
        else
        {
            // First time playing? Start fresh.
            inventory.ResetData();
        }
    }

    // --- NEW: COMPLETE RESET ---

    [ContextMenu("⚠️ RESET ALL PROGRESS")]
    public void ResetAllProgress()
    {
        // 1. Reset the ScriptableObject Memory
        if(inventory != null) inventory.ResetData();

        // 2. Delete the physical Save File
        // We recalculate path here to ensure it works even if Game isn't running
        string path = Application.persistentDataPath + "/savegame.json";
        
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("🗑️ Save File Deleted.");
        }
        else
        {
            Debug.Log("No Save File found to delete.");
        }

        // 3. Update UI (Only if game is running)
        if (Application.isPlaying)
        {
            onCoinChanged?.Invoke(0);
            onKeysChanged?.Invoke(0);
        }

        Debug.Log("✅ Game Reset Complete!");
    }
}