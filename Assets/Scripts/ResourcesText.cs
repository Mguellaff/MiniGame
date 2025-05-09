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
            hudText = hudInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (hudText != null) resourceType.textToChange = hudText;
            else Debug.LogError("hudtext est null");

            if (hudText != null)
            {
                hudText.text = "0";
                resourceTexts[resourceType] = hudText;

                // Abonner le texte à l'événement OnAmountChanged
                if (InventoryManager.Instance.TryGetResourceData(resourceType, out ResourceData resourceData))
                {
                    resourceData.OnAmountChanged += (newAmount) =>
                    {
                        hudText.text = newAmount.ToString();
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
