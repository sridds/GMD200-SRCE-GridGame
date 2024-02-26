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

        inv.OnItemAdded += UpdateList;
        inv.OnArmorChanged += UpdateArmor;
        inv.OnItemRemoved += ItemRemoved;
        inv.OnWeaponChanged += UpdateWeapon;
    }

    private void UpdateList(ItemSO item, int index)
    {
        string count = inv.Items[index].count > 1 ? $" {inv.Items[index].count}" : "";
        string text = item.ItemName + count;

        if (textIndexPair.ContainsKey(index)) {
            textIndexPair[index].text = text;
        }
        else {
            TextMeshProUGUI newText = Instantiate(_textPrefab, _contentHolder);
            newText.text = text;

            textIndexPair.Add(index, newText);
        }

        SortHierarchy();
    }

    private void ItemRemoved(ItemSO item, int index)
    {
        // something is bugged with this
        if (!textIndexPair.ContainsKey(index)) return;
        // try to get the slot
        if (!inv.TryGetSlot(index, out Slot slot)) return;

        if(slot.count > 1) {
            string count = inv.Items[index].count > 1 ? $" {inv.Items[index].count}" : "";
            string text = item.ItemName + count;

            textIndexPair[index].text = text;
        }
        else {
            Destroy(textIndexPair[index].gameObject);
            textIndexPair.Remove(index);
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
