using UnityEngine;

public class SpikeTrap : TrapBase
{
    protected override void OnPlayerContact(GameObject player)
    {
        // 1. Disable player movement/visuals instantly
        player.SetActive(false); 
        
        // 2. Tell Game Manager to reset
        GameManager.Instance.GameOver();
    }
}