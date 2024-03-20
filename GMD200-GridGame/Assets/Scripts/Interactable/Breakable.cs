using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, Interactable
{
    [Header("Resource Settings")]

    [Tooltip("The health of the resource")]
    [SerializeField] private int durability = 3;

    [Tooltip("How many materials the resource drops on collection")]
    [SerializeField] private int materialAmount = 3;

    [SerializeField] private List<MaterialSO> dropList;
    public void Interact()
    {
        //Subject to change for damage implementation
        durability--;

        if (durability <= 0)
            Break();
    }
    /// <summary>
    /// Destroys object and drops items
    /// </summary>
    private void Break()
    {
        int spawnChance = Random.Range(0, 100);

        List<MaterialSO> spawnPool = new();

        for (int i = 0; i < materialAmount; i++)
        {
            //Add materials to drop pool based on rarity
            foreach (MaterialSO material in dropList)
            {
                if (material.rarity <= spawnChance)
                {
                    spawnPool.Add(material);
                }
            }

            //Randomly select material from drop pool
            if (spawnPool.Count > 0)
            {
                //Pick material at random
                int material = Random.Range(0, spawnPool.Count - 1);
                MaterialSO materialInstance = spawnPool[material];

                //Add to inventory
                GameManager.Instance.inventory.AddItem(materialInstance);
            }
        }
        //Destroy resource after its been harvested
        Destroy(gameObject);
    }
}
