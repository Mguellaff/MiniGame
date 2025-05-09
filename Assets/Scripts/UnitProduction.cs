using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UnitProduction : MonoBehaviour
{
    private Producer producer;
    private InventoryManager inventoryManager;
    private bool isReadyToHarvest = false;  
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private void ProduceUnit()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("Aucun InventoryManager trouvé dans la scène !");
                return;
            }
        }

        if (producer.BaseItem!=null&&inventoryManager.SpendResource(producer.BaseItem, producer.ProductionCost))
        {
            inventoryManager.AddResource(producer.ProducedItem, producer.ProductionAmount);
        }
    }
    public void Harvest()
    {
        if (producer.BaseItem != null && inventoryManager.SpendResource(producer.BaseItem, producer.ProductionCost))
        {
            inventoryManager.AddResource(producer.ProducedItem, producer.ProductionAmount);
            isReadyToHarvest = false;
        }
    }
    public void SetProducer(Producer producer)
    {
        this.producer = producer;

        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = null;
        meshFilter.mesh = producer.mesh;
        meshRenderer.material = producer.material;

        if (producer.BaseItem == null)
        {
            Debug.Log("Le BaseItem du Producer est null !");
            //StartCoroutine(Produce());
        }
    }

    

    private IEnumerator Produce()
    {
        while (!isReadyToHarvest)
        {
            yield return new WaitForSeconds(producer.ProductionTime);
            isReadyToHarvest = true;
        }
    }

    public bool GetIsReadyToHarvest()
    {
        return isReadyToHarvest;
    }
}
