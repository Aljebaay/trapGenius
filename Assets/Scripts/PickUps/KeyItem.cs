using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Inventory/Key Item")]
public class KeyItem : ScriptableObject
{
    public string keyName;
    [TextArea] public string description;
    public Sprite icon; // Useful for UI later
}