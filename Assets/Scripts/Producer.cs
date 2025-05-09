using UnityEngine;

[CreateAssetMenu(fileName = "NewProducer", menuName = "Production/Producer")]
public class Producer : ScriptableObject
{
    public Sprite image;
    public int price;
    public GameObject prefab;
    public int ProductionAmount;
    public int ProductionTime;
    public int ProductionCost;

    public ResourceType BaseItem; 
    public ResourceType ProducedItem;

    public Mesh mesh;
    public Material material;
}
