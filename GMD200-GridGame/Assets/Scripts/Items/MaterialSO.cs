using UnityEngine;

[CreateAssetMenu(fileName = "Material_SO", menuName = "Items/Material", order = 1)]
public class MaterialSO : ItemSO
{
    public override ItemSO Clone()
    {
        MaterialSO Instance = ScriptableObject.CreateInstance<MaterialSO>();
        Instance.name = ItemName;

        return CopyValuesReflection(Instance);
    }
}
