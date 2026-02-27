using UnityEngine;
using System.Collections.Generic;
using System.IO; // Required for file saving
using System.Security.Cryptography; // Required for HMAC hashing
using System.Text; // Required for encoding

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
    
    // Salt used for hashing the save file (keep this hidden/obfuscated in a real production environment)
    private const string SALT = "TrapGeniusSave_v1";

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

    // --- SAVE / LOAD SYSTEM WITH ANTI-TAMPER (HMAC) ---

    // Computes an HMAC-SHA256 hash of the JSON data to verify integrity
    private string ComputeHash(string data)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SALT)))
        {
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return System.Convert.ToBase64String(hashBytes);
        }
    }

    [ContextMenu("Force Save")]
    public void SaveInventory()
    {
        // Default ToJson generates a single-line string
        string json = JsonUtility.ToJson(inventory);
        string hash = ComputeHash(json);

        // Ensure path is set if called from Editor
        if (string.IsNullOrEmpty(savePath)) savePath = Application.persistentDataPath + "/savegame.json";
        
        // Save the JSON on line 1, and the Hash on line 2
        File.WriteAllText(savePath, json + "\n" + hash);
        Debug.Log("Game Saved with Tamper Protection.");
    }

    [ContextMenu("Force Load")]
    public void LoadInventory()
    {
        // Ensure path is set if called from Editor
        if (string.IsNullOrEmpty(savePath)) savePath = Application.persistentDataPath + "/savegame.json";

        if (File.Exists(savePath))
        {
            try
            {
                // Read all lines (Line 0 = JSON, Line 1 = Hash)
                string[] lines = File.ReadAllLines(savePath);

                if (lines.Length >= 2)
                {
                    string json = lines[0];
                    string savedHash = lines[1];
                    
                    // Verify the hash against the data
                    string calculatedHash = ComputeHash(json);

                    if (savedHash == calculatedHash)
                    {
                        JsonUtility.FromJsonOverwrite(json, inventory);
                        Debug.Log("Game Loaded Successfully. Data integrity verified.");
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ Save file tampered with! Hash mismatch. Resetting data.");
                        inventory.ResetData();
                    }
                }
                else
                {
                    Debug.LogWarning("⚠️ Old or invalid save file format detected. Resetting data.");
                    inventory.ResetData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load save file: {e.Message}. Resetting.");
                inventory.ResetData();
            }
        }
        else
        {
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