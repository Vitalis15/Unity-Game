using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstructionData
{
    public List<ConstructionItemData> constructedItems;

    public ConstructionData(List<ConstructionItemData> items)
    {
        constructedItems = items;
    }
}

[System.Serializable]
public class ConstructionItemData
{
    public string itemName;
    public Vector3 position;
    public Quaternion rotation;

    public ConstructionItemData(string name, Vector3 pos, Quaternion rot)
    {
        itemName = name;
        position = pos;
        rotation = rot;
    }
}
