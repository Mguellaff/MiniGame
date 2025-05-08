using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpdateHUD : MonoBehaviour
{
    [SerializeField] private GameObject resourceHUDPrefab;

    private Dictionary<ResourceType, TextMeshProUGUI> resourceTexts = new();
    private InventoryManager inventory;
    private Image resourceHUDImage;
    private TextMeshProUGUI resourceText;

    private void Start()
    {
        Debug.Log("UpdateHUD Start called");

        inventory = FindFirstObjectByType<InventoryManager>();
        if (inventory == null)
        {
            Debug.LogError("InventoryManager is null");
            return;
        }

        // Charger tous les ResourceType présents dans Resources/ResourceTypes
        ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("ResourceTypes");
        if (resourceTypes.Length == 0)
        {
            Debug.LogError("No ResourceTypes found in Resources/ResourceTypes!");
            return;
        }

        foreach (ResourceType type in resourceTypes)
        {
            // Instancier le prefab et définir le parent
            GameObject instance = Instantiate(resourceHUDPrefab, transform);
            instance.name = $"HUD_{type.name}";

            // Trouver et initialiser le TextMeshProUGUI à l'intérieur du prefab
            resourceText = instance.GetComponentInChildren<TextMeshProUGUI>();
            resourceHUDImage = instance.GetComponentInChildren<Image>();

            resourceText.text = "0";
            resourceTexts[type] = resourceText;

            resourceHUDImage.sprite = type.icon;

            // Abonner le changement de valeur pour chaque ressource
            inventory.Subscribe(type, (newValue) =>
            {
                if (resourceTexts.TryGetValue(type, out TextMeshProUGUI t))
                    t.text = $"{newValue}"; // Mettre à jour le texte de la ressource
            });
        }
    }
}
