using System;

public class ResourceData
{
    private int currentAmount;
    private int totalAmount;

    // Événement déclenché lorsque currentAmount change
    public event Action<int> OnAmountChanged;

    public ResourceData(int baseAmount)
    {
        currentAmount = baseAmount;
        totalAmount = 0;
    }

    public void Add(int amount)
    {
        currentAmount += amount;
        totalAmount += amount;
        OnAmountChanged?.Invoke(currentAmount);
    }

    public bool Spend(int amount)
    {
        if (currentAmount >= amount)
        {
            currentAmount -= amount;

            OnAmountChanged?.Invoke(currentAmount);

            return true;
        }
        return false;
    }

    public int GetCurrentAmount()
    {
        return currentAmount;
    }

    public int GetTotalAmount()
    {
        return totalAmount;
    }
}
