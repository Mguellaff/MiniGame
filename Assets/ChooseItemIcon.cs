using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseItemIcon : MonoBehaviour
{
    private UnitProduction unitProduction;
    private Sprite itemIcon;
    void Start()
    {
        unitProduction = GetComponentInParent<UnitProduction>();
    }
}
