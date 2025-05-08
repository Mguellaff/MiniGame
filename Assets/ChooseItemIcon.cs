using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseItemIcon : MonoBehaviour
{
    private UnitProduction unitProduction;
    private string unitType;
    private Sprite itemIcon;
    void Start()
    {
        unitProduction = GetComponentInParent<UnitProduction>();
        if (unitProduction != null)
        {
            unitType = unitProduction.GetUnitType();
            Debug.Log("Unit Type: " + unitType);
        }
        else
        {
            Debug.LogError("UnitProduction component not found in parent.");
        }
    }
}
