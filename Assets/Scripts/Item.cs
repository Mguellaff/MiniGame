using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;

    public string linkedResource;

}
