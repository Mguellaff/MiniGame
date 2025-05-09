using System;
using TMPro;
using UnityEngine;

public class EventTextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToUpdate; 
    [SerializeField] private string prefix = ""; 
    [SerializeField] private string suffix = "";

    public void SubscribeToEvent(Action<int> eventAction)
    {
        if (eventAction != null)
        {
            eventAction += UpdateText;
        }
    }
    private void UpdateText(int value)
    {
        if (textToUpdate != null)
        {
            textToUpdate.text = $"{prefix}{value}{suffix}";
        }
    }
}
