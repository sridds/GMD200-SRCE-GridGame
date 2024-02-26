using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryDebug : MonoBehaviour
{
    private Inventory inv;

    [SerializeField]
    private ItemSO[] testItem;
    [SerializeField]
    private WeaponSO testWeapon;
    [SerializeField]
    private ArmorSO testArmor;
    [SerializeField]
    private ArmorSO testArmor2;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();

        for(int i = 0; i < testItem.Length; i++)
        {
            inv.AddItem(testItem[i]);
        }

        inv.AddItem(testWeapon);
        inv.AddItem(testArmor);

        // brody
        inv.EquipArmor(inv.Items.Count - 1);
        inv.EquipWeapon(inv.Items.Count - 1);

        inv.AddItem(testArmor2);
        inv.EquipArmor(inv.Items.Count - 1);
    }
}
