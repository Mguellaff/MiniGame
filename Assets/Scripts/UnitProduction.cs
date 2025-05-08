using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitType
    {
        Well,
        Field,
        Factory
    }

public class UnitProduction : MonoBehaviour
{
    private UnitType unitType;
    private Producer producer;
    private InventoryManager inventoryManager;
    
    private void Start()
    {
        if (producer == null)
        {
            Debug.LogError("Le Producer est null !");
            return;
        }

        unitType = producer.unitType;
        StartCoroutine(Produce());
    }
    private void ProduceUnit()
    {
        if (producer == null)
        {
            Debug.LogError("Le Producer est null !");
            return;
        }

        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("Aucun InventoryManager trouvé dans la scène !");
                return;
            }
        }

        if (inventoryManager.SpendResource(producer.BaseItem, producer.ProductionCost))
        {
            inventoryManager.AddResource(producer.ProducedItem, producer.ProductionAmount);
        }
    }
    public string GetUnitType()
    {
        return unitType.ToString();
    }

    public void SetProducer(Producer producer)
    {
        this.producer = producer;
    }

    

    private IEnumerator Produce()
    {
        while (true)
        {
            yield return new WaitForSeconds(producer.ProductionTime);
            ProduceUnit();
        }
    }
}
