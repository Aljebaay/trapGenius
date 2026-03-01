using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
    
    // Salt used for hashing the save file
    private const string SALT = "TrapGeniusSave_v1";

    // --- NEW: DTO (Data Transfer Object) for safe serialization ---
    [System.Serializable]
    private class SaveData
    {
        public int coins;
        public List<string> keyNames = new List<string>();
    }

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            savePath = Application.persistentDataPath + "/savegame.json";
            
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
        onCoinChanged?.Invoke(inventory.coins);
        SaveInventory();
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
            SaveInventory();
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
            SaveInventory();
        }
    }

    public int GetKeyCount() => inventory.keys.Count;

    // --- SAVE / LOAD SYSTEM (Anti-Tamper & Stable SO References) ---

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
        // 1. Map to persistent DTO
        SaveData data = new SaveData { coins = inventory.coins };
        foreach (var key in inventory.keys)
        {
            if (key != null) 
            {
                // Using key.name matches the exact asset filename needed for Resources.Load
                data.keyNames.Add(key.name); 
            }
        }

        // 2. Serialize and Hash
        string json = JsonUtility.ToJson(data);
        string hash = ComputeHash(json);

        if (string.IsNullOrEmpty(savePath)) savePath = Application.persistentDataPath + "/savegame.json";
        
        File.WriteAllText(savePath, json + "\n" + hash);
        Debug.Log("Game Saved with stable key references and tamper protection.");
    }

    [ContextMenu("Force Load")]
    public void LoadInventory()
    {
        if (string.IsNullOrEmpty(savePath)) savePath = Application.persistentDataPath + "/savegame.json";

        if (File.Exists(savePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(savePath);

                if (lines.Length >= 2)
                {
                    string json = lines[0];
                    string savedHash = lines[1];
                    string calculatedHash = ComputeHash(json);

                    if (savedHash == calculatedHash)
                    {
                        // 1. Deserialize into the DTO
                        SaveData data = JsonUtility.FromJson<SaveData>(json);
                        
                        // 2. Map data back to ScriptableObject memory
                        inventory.coins = data.coins;
                        inventory.keys.Clear();

                        foreach (string name in data.keyNames)
                        {
                            // 3. Re-link ScriptableObjects via Resources folder
                            KeyItem loadedKey = Resources.Load<KeyItem>($"Keys/{name}");
                            if (loadedKey != null) 
                            {
                                inventory.keys.Add(loadedKey);
                            }
                            else
                            {
                                Debug.LogWarning($"⚠️ Failed to locate key: '{name}' in Resources/Keys/. Was the asset renamed or moved?");
                            }
                        }

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

    // --- COMPLETE RESET ---

    [ContextMenu("⚠️ RESET ALL PROGRESS")]
    public void ResetAllProgress()
    {
        if(inventory != null) inventory.ResetData();

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

        if (Application.isPlaying)
        {
            onCoinChanged?.Invoke(0);
            onKeysChanged?.Invoke(0);
        }

        Debug.Log("✅ Game Reset Complete!");
    }
}