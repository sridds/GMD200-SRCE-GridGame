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

    private Dictionary<int, TextMeshProUGUI> textIndexPair = new Dictionary<int, TextMeshProUGUI>();

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();

        inv.OnItemAdded += UpdateList;
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

    }

    public void AddMaterial()
    {
        inv.AddItem(_testMaterials[Random.Range(0, _testMaterials.Length)]);
    }
}
