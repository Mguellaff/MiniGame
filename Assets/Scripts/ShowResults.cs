using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowResults : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private Dictionary<string, int> resourceTotalAmountsDictionary = new Dictionary<string, int>();
    [SerializeField] private GameObject textPrefab; // Assurez-vous d'assigner un prefab dans l'inspecteur

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        ShowInventoryResults();
    }

    private void ShowInventoryResults()
    {
        resourceTotalAmountsDictionary = inventoryManager.GetResourceTotalAmounts();

        foreach (var resource in resourceTotalAmountsDictionary)
        {
            GameObject resourceDisplay = Instantiate(textPrefab, this.transform);
            TextMeshProUGUI textMeshProUGUI = resourceDisplay.GetComponent<TextMeshProUGUI>();

            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.text = $"{resource.Key}: {resource.Value}";
            }
            else
            {
                Debug.LogWarning("Le prefab n'a pas de composant TextMeshProUGUI !");
            }
        }
    }
}
