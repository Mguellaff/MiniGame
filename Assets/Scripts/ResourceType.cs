using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResourceType", menuName = "Resources/ResourceType")]
public class ResourceType : ScriptableObject
{
    public string resourceName;
    public Sprite icon;
    public TextMeshProUGUI textToChange;
    public ResourceData resourceData;
}