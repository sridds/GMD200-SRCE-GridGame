using UnityEngine;

[CreateAssetMenu(fileName = "Armor_SO", menuName = "Items/Armor", order = 1)]
public class ArmorSO : ItemSO
{
    [SerializeField]
    public int MaxDurability { get; set; }
}
