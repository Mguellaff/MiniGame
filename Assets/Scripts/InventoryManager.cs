using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Instance statique pour le Singleton
    public static InventoryManager Instance { get; private set; }

    private Dictionary<ResourceType, ResourceData> resourceDictionary = new Dictionary<ResourceType, ResourceData>();
    private Dictionary<string, int> resourceTotalAmountsDictionary = new Dictionary<string, int>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Une autre instance d'InventoryManager existe d�j�. Elle sera d�truite.");
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
    //M�thode qui associe tous les ResourceType aux ResourceData
    private void InitializeResources()
    {
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");

        foreach (ResourceType type in resourceTypes)
        {
            resourceDictionary[type] = new ResourceData(type.baseAmount);
            type.resourceData = resourceDictionary[type];

            Debug.Log($"ResourceType charg� : {type.resourceName}");

            if (type.textToChange == null)
            {
                Debug.LogWarning($"textToChange n'est pas assign� pour le ResourceType {type.name}. Assurez-vous de l'assigner dans l'inspecteur.");
            }
        }

        Debug.Log($"Initialized {resourceDictionary.Count} resources.");
    }

    //Ajoute une ressource au ResourceData
    public void AddResource(object resourceIdentifier, int amount)
    {
        ResourceType resourceType = null;

        // V�rifie si l'identifiant est un ResourceType
        if (resourceIdentifier is ResourceType type)
        {
            resourceType = type;
        }
        // V�rifie si l'identifiant est un string
        else if (resourceIdentifier is string resourceName)
        {
            Debug.Log($"Recherche du ResourceType avec le nom : {resourceName}");
            resourceType = FindResourceTypeByName(resourceName);
            if (resourceType == null)
            {
                Debug.LogWarning($"Aucun ResourceType trouv� avec le nom {resourceName}.");
                return;
            }
        }
        else
        {
            Debug.LogWarning("L'identifiant fourni n'est ni un ResourceType ni un string.");
            return;
        }

        // Ajoute la ressource au dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            resourceData.Add(amount);
            if (resourceType.textToChange != null)
            {
                resourceType.textToChange.text = resourceData.GetCurrentAmount().ToString();
            }
            else
            {
                Debug.LogWarning($"textToChange n'est pas assign� pour le ResourceType {resourceType.name}.");
            }
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} non trouv� dans le dictionnaire.");
        }
    }

    //M�thode pour d�penser une ressource
    public bool SpendResource(object resourceIdentifier, int amount)
    {
        ResourceType resourceType = null;

        // V�rifie si l'identifiant est un ResourceType
        if (resourceIdentifier is ResourceType type)
        {
            resourceType = type;
        }
        // V�rifie si l'identifiant est un string
        else if (resourceIdentifier is string resourceName)
        {
            Debug.Log($"Recherche du ResourceType avec le nom : {resourceName}");
            resourceType = FindResourceTypeByName(resourceName);
            if (resourceType == null)
            {
                Debug.LogWarning($"Aucun ResourceType trouv� avec le nom {resourceName}.");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("L'identifiant fourni n'est ni un ResourceType ni un string.");
            return false;
        }

        // V�rifie si le ResourceType existe dans le dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            // Tente de d�penser la ressource
            if (resourceData.Spend(amount))
            {
                // Mise � jour de l'affichage si la d�pense a r�ussi
                if (resourceType.textToChange != null)
                {
                    resourceType.textToChange.text = resourceData.GetCurrentAmount().ToString();
                }
                else
                {
                    Debug.LogWarning($"textToChange n'est pas assign� pour le ResourceType {resourceType.name}.");
                }
                return true; // D�pense r�ussie
            }
            else
            {
                Debug.LogWarning($"Pas assez de ressources pour d�penser {amount} unit�s de {resourceType.name}.");
                return false; // D�pense �chou�e
            }
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} non trouv� dans le dictionnaire.");
            return false;
        }
    }



    public int GetResourceAmount(object resourceIdentifier)
    {
        ResourceType resourceType = null;

        if (resourceIdentifier is ResourceType type)
        {
            resourceType = type;
        }
        else if (resourceIdentifier is string resourceName)
        {
            Debug.Log($"Recherche du ResourceType avec le nom : {resourceName}");
            resourceType = FindResourceTypeByName(resourceName);
            if (resourceType == null)
            {
                Debug.LogWarning($"Aucun ResourceType trouv� avec le nom {resourceName}.");
                return 0;
            }
        }
        else
        {
            Debug.LogWarning("L'identifiant fourni n'est ni un ResourceType ni un string.");
            return 0;
        }

        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            Debug.Log($"ResourceType {resourceType.name} trouv� avec {resourceData.GetCurrentAmount()} unit�s.");
            return resourceData.GetCurrentAmount();
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} non trouv� dans le dictionnaire.");
            return 0;
        }
    }



    private ResourceType FindResourceTypeByName(string resourceName)
    {
        foreach (var key in resourceDictionary.Keys)
        {
            Debug.Log($"Recherche : {key.resourceName}");
            if (key.resourceName == resourceName)
            {
                return key;
            }
        }
        return null;
    }



    public bool TryGetResourceData(ResourceType resourceType, out ResourceData resourceData)
    {
        return resourceDictionary.TryGetValue(resourceType, out resourceData);
    }

    private void UpdateTotalAmounts()
    {
        resourceTotalAmountsDictionary.Clear();

        foreach (var entry in resourceDictionary)
        {
            ResourceType resourceType = entry.Key;
            ResourceData resourceData = entry.Value;

            if (resourceType != null && resourceData != null)
            {
                resourceTotalAmountsDictionary[resourceType.resourceName] = resourceData.GetTotalAmount();
            }
            else
            {
                Debug.LogWarning("Un ResourceType ou ResourceData est null lors de la mise � jour des totaux.");
            }
        }

        Debug.Log("Les totaux des ressources ont �t� mis � jour.");
    }

    public Dictionary<string, int> GetResourceTotalAmounts()
    {
        UpdateTotalAmounts();
        return resourceTotalAmountsDictionary;
    }
    public void ClearInventory()
    {
        resourceDictionary.Clear();
        resourceTotalAmountsDictionary.Clear();
        InitializeResources();
        Debug.Log("Inventaire r�initialis�.");
    }
}
