using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuyButton : MonoBehaviour
{
    private Producer[] producers;
    private GameObject container;
    [SerializeField] GameObject buttonPrefab;
    private BuyButton buyButton;
    void Start()
    {
        producers = Resources.LoadAll<Producer>("Producers");
        Debug.Log("Producers loaded: " + producers.Length);
        container=transform.GetChild(0).gameObject;
        foreach (var producer in producers)
        {
            Instantiate(buttonPrefab, container.transform);
            buyButton = container.transform.GetChild(container.transform.childCount - 1).GetComponent<BuyButton>();
            buyButton.SetProducer(producer);
        }
    }
}
