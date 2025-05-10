using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourcesText : MonoBehaviour
{
    [SerializeField] private GameObject resourceHUDPrefab; // Prefab à instancier
    private TextMeshProUGUI hudText; // Référence au texte à changer
    private Dictionary<ResourceType, TextMeshProUGUI> resourceTexts = new();

    private void Start()
    {
        // Charger tous les types de ressources depuis Resources/ResourceTypes
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");

        foreach (ResourceType resourceType in resourceTypes)
        {
            // Instancier le prefab
            GameObject hudInstance = Instantiate(resourceHUDPrefab, this.gameObject.transform);

            // Configurer l'image
            Image hudImage = hudInstance.GetComponentInChildren<Image>();
            if (hudImage != null)
            {
                hudImage.sprite = resourceType.icon;
            }

            // Configurer le texte
            TextMeshProUGUI localHudText = hudInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (localHudText != null) resourceType.textToChange = localHudText;
            else Debug.LogError("hudtext est null");

            if (localHudText != null)
            {
                localHudText.text = "0";
                resourceTexts[resourceType] = localHudText;

                // Abonner le texte à l'événement OnAmountChanged
                if (InventoryManager.Instance.TryGetResourceData(resourceType, out ResourceData resourceData))
                {
                    resourceData.OnAmountChanged += (newAmount) =>
                    {
                        localHudText.text = newAmount.ToString();
                    };
                }
            }
        }

    }


    // Méthode pour mettre à jour les valeurs des ressources
    public void UpdateResourceText(ResourceType resourceType, int amount)
    {
        if (resourceTexts.TryGetValue(resourceType, out TextMeshProUGUI hudText))
        {
            hudText.text = amount.ToString();
        }
    }
}
