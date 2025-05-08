using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ResourceData> resources = new List<ResourceData>();

    // Événements pour notifier les abonnés lors du changement des ressources
    public event Action<ResourceType, int> OnResourceChanged;

    private void Start()
    {
        // Par exemple, on peut initialiser le gestionnaire avec des types de ressources
        InitializeResources();
    }

    private void InitializeResources()
    {
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");

        foreach (ResourceType type in resourceTypes)
        {
            resources.Add(new ResourceData(type));
        }

        Debug.Log($"Initialized {resources.Count} resources from ResourceTypes folder.");
    }


    public void AddResource(ResourceType resourceType, int amount)
    {
        ResourceData resource = resources.Find(r => r.resourceType == resourceType);
        if (resource != null)
        {
            resource.Add(amount);
            // Lorsque la ressource est modifiée, on déclenche l'événement
            OnResourceChanged?.Invoke(resourceType, resource.currentAmount);
        }
    }

    public bool SpendResource(ResourceType resourceType, int amount)
    {
        ResourceData resource = resources.Find(r => r.resourceType == resourceType);
        if (resource != null)
        {
            bool success = resource.Spend(amount);
            if (success)
            {
                // Lorsque la ressource est modifiée, on déclenche l'événement
                OnResourceChanged?.Invoke(resourceType, resource.currentAmount);
            }
            return success;
        }
        return false;
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        ResourceData resource = resources.Find(r => r.resourceType == resourceType);
        if (resource != null)
        {
            return resource.currentAmount;
        }
        return 0;
    }

    // Méthode pour s'abonner aux changements de ressource
    public void Subscribe(ResourceType resourceType, Action<int> callback)
    {
        OnResourceChanged += (type, amount) =>
        {
            if (type == resourceType)
            {
                callback(amount);
            }
        };
    }
}
