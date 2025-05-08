using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitChildAmount : MonoBehaviour
{
    [SerializeField] private int maxChildren = 5;
    void Update()
    {
        if (transform.childCount > maxChildren)
        {
            // Supprime les enfants excédentaires
            for (int i = transform.childCount - 1; i >= maxChildren; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
