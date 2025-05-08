using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitType
    {
        Well,
        Field,
        Factory
    }

public class UnitProduction : MonoBehaviour
{
    private UnitType unitType;
    private Producer producer;
    public string GetUnitType()
    {
        return unitType.ToString();
    }

    public void SetProducer(Producer producer)
    {
        this.producer = producer;
    }

    private void Start()
    {
        if (producer == null)
        {
            Debug.LogError("Le Producer est null !");
            return;
        }

        unitType = producer.unitType;
        StartCoroutine(Produce());
    }

    private IEnumerator Produce()
    {
        while (true)
        {
            yield return new WaitForSeconds(producer.ProductionTime);
            ProduceUnit();
        }
    }
}
