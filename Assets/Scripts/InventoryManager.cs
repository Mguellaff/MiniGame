using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Dictionary<ResourceType, ResourceData> resourceDictionary = new Dictionary<ResourceType, ResourceData>();

    private void Start()
    {
        InitializeResources();
    }
    private void InitializeResources()
    {
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");

        foreach (ResourceType type in resourceTypes)
        {
            // Associe chaque ResourceType à un nouveau ResourceData
            resourceDictionary[type] = new ResourceData(type);
        }

        Debug.Log($"Initialized {resourceDictionary.Count} resources.");
    }

    public void AddResource(ResourceType resourceType, int amount)
    {
        // Vérifie si le ResourceType existe dans le dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            resourceType.textToChange.text = resourceData.totalAmount.ToString();
            resourceData.Add(amount);
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} not found in resourceDictionary.");
        }
    }

    public bool SpendResource(ResourceType resourceType, int amount)
    {
        // Vérifie si le ResourceType existe dans le dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            resourceType.textToChange.text = resourceData.totalAmount.ToString();
            return resourceData.Spend(amount);
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} not found in resourceDictionary.");
            return false;
        }
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        // Vérifie si le ResourceType existe dans le dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            return resourceData.currentAmount;
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} not found in resourceDictionary.");
            return 0;
        }
    }
}
