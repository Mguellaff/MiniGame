using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Instance statique pour le Singleton
    public static InventoryManager Instance { get; private set; }

    private Dictionary<ResourceType, ResourceData> resourceDictionary = new Dictionary<ResourceType, ResourceData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Une autre instance d'InventoryManager existe déjà. Elle sera détruite.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeResources();
    }

    private void InitializeResources()
    {
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");

        foreach (ResourceType type in resourceTypes)
        {
            resourceDictionary[type] = new ResourceData();
            type.resourceData = resourceDictionary[type];

            if (type.textToChange == null)
            {
                Debug.LogWarning($"textToChange n'est pas assigné pour le ResourceType {type.name}. Assurez-vous de l'assigner dans l'inspecteur.");
            }
        }

        Debug.Log($"Initialized {resourceDictionary.Count} resources.");
    }


    public void AddResource(ResourceType resourceType, int amount)
    {
        // Vérifie si le ResourceType existe dans le dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            resourceData.Add(amount); 
            if (resourceType.textToChange != null)
            {
                resourceType.textToChange.text = resourceData.GetCurrentAmount().ToString();
            }
            else
            {
                Debug.LogWarning($"textToChange n'est pas assigné pour le ResourceType {resourceType.name}.");
            }

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
            resourceData.Spend(amount); 
            if (resourceType.textToChange != null)
            {
                resourceType.textToChange.text = resourceData.GetCurrentAmount().ToString();
            }
            else
            {
                Debug.LogWarning($"textToChange n'est pas assigné pour le ResourceType {resourceType.name}.");
            }
            return true;
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
            return resourceData.GetCurrentAmount();
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} not found in resourceDictionary.");
            return 0;
        }
    }

    public bool TryGetResourceData(ResourceType resourceType, out ResourceData resourceData)
    {
        return resourceDictionary.TryGetValue(resourceType, out resourceData);
    }

}
