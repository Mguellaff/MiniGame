using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitProduction : MonoBehaviour
{
    private Producer producer;
    private InventoryManager inventoryManager;
    private bool isReadyToHarvest = false;
    private bool isRequiringResource = false;
    private bool isProducing = false; // Nouvelle variable pour indiquer si la production est en cours
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Animator animator;
    private GameObject bubbleCanvas;
    private Image resourceIcon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bubbleCanvas = transform.GetChild(0).gameObject;
        resourceIcon = bubbleCanvas.transform.GetComponentInChildren<Image>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

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

        if (producer.BaseItem != null && inventoryManager.SpendResource(producer.BaseItem, producer.ProductionCost))
        {
            inventoryManager.AddResource(producer.ProducedItem, producer.ProductionAmount);
        }
    }

    public void Harvest()
    {
        if (isProducing) // Empêche toute action si la production est en cours
        {
            Debug.LogWarning("La production est déjà en cours !");
            return;
        }

        if (isReadyToHarvest)
        {
            if (producer.BaseItem == null)
            {
                inventoryManager.AddResource(producer.ProducedItem, producer.ProductionAmount);
                isReadyToHarvest = false;
                bubbleCanvas.SetActive(false);
                StartCoroutine(Produce());
            }
            else if (producer.BaseItem != null && inventoryManager.SpendResource(producer.BaseItem, producer.ProductionCost) && isReadyToHarvest)
            {
                inventoryManager.AddResource(producer.ProducedItem, producer.ProductionAmount);
                isReadyToHarvest = false;
                bubbleCanvas.SetActive(false);
                StartCoroutine(Produce());
            }
        }
        else
        {
            if (producer.BaseItem != null && inventoryManager.SpendResource(producer.BaseItem, producer.ProductionCost))
            {
                StartCoroutine(Produce());
            }
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
        resourceIcon.sprite = producer.ProducedItem.icon;
        if (producer.BaseItem == null)
        {
            Debug.Log("Le BaseItem du Producer est null !");
            StartCoroutine(Produce());
        }
    }

    private IEnumerator Produce()
    {
        isProducing = true; // Marque le début de la production
        while (!isReadyToHarvest)
        {
            animator.SetBool("isProducing", true);
            yield return new WaitForSeconds(producer.ProductionTime);
            isReadyToHarvest = true;
            animator.SetBool("isProducing", false);
            bubbleCanvas.SetActive(true);
        }
        isProducing = false; // Marque la fin de la production
    }

    public bool GetIsReadyToHarvest()
    {
        return isReadyToHarvest;
    }

    public bool GetIsRequiringResource()
    {
        if (producer.BaseItem == null)
        {
            isRequiringResource = false;
        }
        else if (producer.BaseItem != null && inventoryManager.GetResourceAmount(producer.BaseItem) < producer.ProductionCost)
        {
            isRequiringResource = true;
        }
        else if (producer.BaseItem != null && inventoryManager.GetResourceAmount(producer.BaseItem) >= producer.ProductionCost && isReadyToHarvest == false)
        {
            isRequiringResource = true;
        }
        return isRequiringResource;
    }
}
