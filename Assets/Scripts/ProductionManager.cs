using UnityEngine;
using System.Collections;

public class ProductionManager
{
    public float ProductionAmount { get; private set; }
    public float ProductionTime { get; private set; }
    public float ProductionCost { get; private set; }
    public Item BaseItem { get; private set; }
    public Item ProducedItem { get; private set; }

    public ProductionManager(float productionAmount, float productionTime, float productionCost, Item baseItem, Item producedItem)
    {
        ProductionAmount = productionAmount;
        ProductionTime = productionTime;
        ProductionCost = productionCost;
        BaseItem = baseItem;
        ProducedItem = producedItem;
    }
}
