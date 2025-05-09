using System;

[System.Serializable]
public class ResourceData
{
    public int currentAmount;
    public int totalAmount;

    public ResourceData(ResourceType type)
    {
        currentAmount = 0;
        totalAmount = 0;
    }

    public void Add(int amount)
    {
        currentAmount += amount;
        totalAmount += amount;
    }

    public bool Spend(int amount)
    {
        if (currentAmount >= amount)
        {
            currentAmount -= amount;
            return true;
        }
        return false;
    }
}
