using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float[] playerStats;//[0] -health [1] - calories  [2] - hydratation

    
    public float[] playerPositionAndRotation;// position x,y,x  and rotation x,y,z
                                             // public string[] inventoryContent;

    public string[] inventoryContent;

    public string[] quickSlotsContent;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot, string[] _inventoryContent, string[] _quickSlotsContent)
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;
        inventoryContent = _inventoryContent;
       quickSlotsContent = _quickSlotsContent;
    }
}
