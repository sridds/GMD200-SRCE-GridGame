using UnityEngine;

[CreateAssetMenu(fileName = "Resource_SO", menuName = "Resources/Resource", order = 3)]
public class ResourceSO : ScriptableObject
{
    [Tooltip("The resource that is instantiated on the map")]
    public GameObject resourceTile;

    [Tooltip("The resource collected from harvesting this tile")]
    public ItemSO resourceDrop;

    [Tooltip("The chance of this resource spawning")]
    public int rarity;

    /*private void Start()
    {
        if(resourceDrop is MaterialSO)
        {
            MaterialSO reference = (MaterialSO)resourceDrop;
        }
    }*/
}
