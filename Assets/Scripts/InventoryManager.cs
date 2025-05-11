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
    //Méthode qui associe tous les ResourceType aux ResourceData
    private void InitializeResources()
    {
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");

        foreach (ResourceType type in resourceTypes)
        {
            resourceDictionary[type] = new ResourceData(type.baseAmount);
            type.resourceData = resourceDictionary[type];

            Debug.Log($"ResourceType chargé : {type.resourceName}");

            if (type.textToChange == null)
            {
                Debug.LogWarning($"textToChange n'est pas assigné pour le ResourceType {type.name}. Assurez-vous de l'assigner dans l'inspecteur.");
            }
        }

        Debug.Log($"Initialized {resourceDictionary.Count} resources.");
    }

    //Ajoute une ressource au ResourceData
    public void AddResource(object resourceIdentifier, int amount)
    {
        ResourceType resourceType = null;

        // Vérifie si l'identifiant est un ResourceType
        if (resourceIdentifier is ResourceType type)
        {
            resourceType = type;
        }
        // Vérifie si l'identifiant est un string
        else if (resourceIdentifier is string resourceName)
        {
            Debug.Log($"Recherche du ResourceType avec le nom : {resourceName}");
            resourceType = FindResourceTypeByName(resourceName);
            if (resourceType == null)
            {
                Debug.LogWarning($"Aucun ResourceType trouvé avec le nom {resourceName}.");
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
                Debug.LogWarning($"textToChange n'est pas assigné pour le ResourceType {resourceType.name}.");
            }
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} non trouvé dans le dictionnaire.");
        }
    }

    //Méthode pour dépenser une ressource
    public bool SpendResource(object resourceIdentifier, int amount)
    {
        ResourceType resourceType = null;

        // Vérifie si l'identifiant est un ResourceType
        if (resourceIdentifier is ResourceType type)
        {
            resourceType = type;
        }
        // Vérifie si l'identifiant est un string
        else if (resourceIdentifier is string resourceName)
        {
            Debug.Log($"Recherche du ResourceType avec le nom : {resourceName}");
            resourceType = FindResourceTypeByName(resourceName);
            if (resourceType == null)
            {
                Debug.LogWarning($"Aucun ResourceType trouvé avec le nom {resourceName}.");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("L'identifiant fourni n'est ni un ResourceType ni un string.");
            return false;
        }

        // Vérifie si le ResourceType existe dans le dictionnaire
        if (resourceDictionary.TryGetValue(resourceType, out ResourceData resourceData))
        {
            // Tente de dépenser la ressource
            if (resourceData.Spend(amount))
            {
                // Mise à jour de l'affichage si la dépense a réussi
                if (resourceType.textToChange != null)
                {
                    resourceType.textToChange.text = resourceData.GetCurrentAmount().ToString();
                }
                else
                {
                    Debug.LogWarning($"textToChange n'est pas assigné pour le ResourceType {resourceType.name}.");
                }
                return true; // Dépense réussie
            }
            else
            {
                Debug.LogWarning($"Pas assez de ressources pour dépenser {amount} unités de {resourceType.name}.");
                return false; // Dépense échouée
            }
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} non trouvé dans le dictionnaire.");
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
                Debug.LogWarning($"Aucun ResourceType trouvé avec le nom {resourceName}.");
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
            Debug.Log($"ResourceType {resourceType.name} trouvé avec {resourceData.GetCurrentAmount()} unités.");
            return resourceData.GetCurrentAmount();
        }
        else
        {
            Debug.LogWarning($"ResourceType {resourceType.name} non trouvé dans le dictionnaire.");
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
                Debug.LogWarning("Un ResourceType ou ResourceData est null lors de la mise à jour des totaux.");
            }
        }

        Debug.Log("Les totaux des ressources ont été mis à jour.");
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
        Debug.Log("Inventaire réinitialisé.");
    }
}
