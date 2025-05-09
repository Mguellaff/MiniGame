using UnityEngine;

[System.Serializable]
public class ResourceManager
{
    private ResourceType resourceType;
    private ResourceData resourceData;

    public ResourceManager(ResourceType type)
    {
        resourceType = type;
        resourceData = new ResourceData(type);
    }

    // M�thode pour obtenir le ResourceType
    public ResourceType GetResourceType()
    {
        return resourceType;
    }

    // M�thode pour obtenir le ResourceData
    public ResourceData GetResourceData()
    {
        return resourceData;
    }

    // M�thode pour obtenir le ResourceData en fonction d'un ResourceType
    public ResourceData GetResourceDataByType(ResourceType type)
    {
        if (resourceType == type)
        {
            return resourceData;
        }
        return null; // Retourne null si le type ne correspond pas
    }
}
