using UnityEngine;

[CreateAssetMenu(fileName = "Material_SO", menuName = "Items/Material", order = 1)]
public class MaterialSO : ItemSO
{
    public override ItemSO Clone() => CloneGeneric<MaterialSO>();

    public override void OnUse(UseContext ctx) { }

    public override void OnUseDown(UseContext ctx) { }
}
