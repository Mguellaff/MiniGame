using System;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public ResourceType resourceType;
    public int currentAmount;        
    public int totalAmount;          
    public Action<int> OnChanged;    

    public ResourceData(ResourceType type)
    {
        resourceType = type;
        currentAmount = 0;
        totalAmount = 0;
    }

    public void Add(int amount)
    {
        currentAmount += amount;
        totalAmount += amount;
        OnChanged?.Invoke(currentAmount);
    }

    public bool Spend(int amount)
    {
        if (currentAmount >= amount)
        {
            currentAmount -= amount;
            OnChanged?.Invoke(currentAmount);
            return true;
        }
        return false;
    }
}
