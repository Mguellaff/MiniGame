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
    }
}
