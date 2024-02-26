using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryDebug : MonoBehaviour
{
    private Inventory inv;

    [SerializeField]
    private MaterialSO[] _testMaterials;
    [SerializeField]
    private ToolSO[] _testTools;
    [SerializeField]
    private WeaponSO[] _testWeapons;
    [SerializeField]
    private ArmorSO[] _testArmor;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _textPrefab;
    [SerializeField]
    private RectTransform _contentHolder;
    [SerializeField]
    private TextMeshProUGUI _armorText;
    [SerializeField]
    private TextMeshProUGUI _weaponText;

    private Dictionary<int, TextMeshProUGUI> textIndexPair = new Dictionary<int, TextMeshProUGUI>();

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();

        inv.OnArmorChanged += UpdateArmor;
        inv.OnWeaponChanged += UpdateWeapon;
        inv.OnInventoryUpdate += InventoryUpdate;
    }

    private void InventoryUpdate(List<Slot> newInv)
    {
        for(int i = 0; i < newInv.Count; i++)
        {
            string count = newInv[i].count > 1 ? $" {newInv[i].count}" : "";
            string text = newInv[i].item.ItemName + count;

            // try to get the value and update text
            if (textIndexPair.TryGetValue(i, out TextMeshProUGUI val)) {
                val.text = text;
            }
            // create new text object with the value pair
            else {
                TextMeshProUGUI newText = Instantiate(_textPrefab, _contentHolder);
                newText.text = text;

                // add key value pair
                textIndexPair.Add(i, newText);
            }
        }

        for(int i = 0; i < textIndexPair.Count; i++)
        {
            if(i > newInv.Count - 1)
            {
                Destroy(textIndexPair[i].gameObject);
                textIndexPair.Remove(i);
            }
        }

        SortHierarchy();
    }

    private void SortHierarchy()
    {
        for(int i = 0; i < inv.Items.Count; i++)
        {
            if (textIndexPair.ContainsKey(i))
                textIndexPair[i].transform.SetSiblingIndex(i);
        }
    }

    private void UpdateArmor(ArmorSO newArmor)
    {
        _armorText.text = $"Equipped Armor: {newArmor.ItemName}";
    }

    private void UpdateWeapon(WeaponSO newWeapon)
    {
        _weaponText.text = $"Equipped Armor: {newWeapon.ItemName}";
    }

    public void AddMaterial() => inv.AddItem(_testMaterials[Random.Range(0, _testMaterials.Length)]);
    public void AddTool() => inv.AddItem(_testTools[Random.Range(0, _testTools.Length)]);
    public void AddWeapon() => inv.AddItem(_testWeapons[Random.Range(0, _testWeapons.Length)]);
    public void AddArmor() => inv.AddItem(_testArmor[Random.Range(0, _testArmor.Length)]);

    public void EquipAnyArmor()
    {
        List<Slot> armors = new List<Slot>();

        foreach(Slot slot in inv.Items)
        {
            if(slot.item is ArmorSO)
            {
                armors.Add(slot);
            }
        }

        if (armors.Count == 0) return;

        inv.EquipArmor(inv.Items.IndexOf(armors[Random.Range(0, armors.Count)]));

        SortHierarchy();
    }

    public void EquipAnyWeapon()
    {
        List<Slot> weapons = new List<Slot>();

        foreach (Slot slot in inv.Items)
        {
            if (slot.item is WeaponSO)
            {
                weapons.Add(slot);
            }
        }

        if (weapons.Count == 0) return;

        inv.EquipWeapon(inv.Items.IndexOf(weapons[Random.Range(0, weapons.Count)]));

        SortHierarchy();
    }
}
