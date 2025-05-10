using System;
using UnityEngine;

public static class Utils
{
    public static void VerifyIfNull<T>(T obj, Action action, bool executeIfNull = false) where T : class
    {
        if (obj == null)
        {
            Debug.LogWarning($"L'objet de type {typeof(T).Name} est null.");
            if (executeIfNull)
            {
                action?.Invoke();
            }
        }
        else
        {
            if (!executeIfNull)
            {
                action?.Invoke();
            }
        }
    }
}


