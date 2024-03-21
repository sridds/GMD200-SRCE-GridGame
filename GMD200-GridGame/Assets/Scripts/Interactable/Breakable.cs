using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Breakable : MonoBehaviour, Interactable
{
    [Header("Resource Settings")]

    [Tooltip("The health of the resource")]
    [SerializeField] private Health myHealth;

    [Tooltip("How many materials the resource drops on collection")]
    [SerializeField] private int materialAmount = 3;

    [SerializeField] private List<MaterialSO> dropList;
    [SerializeField] private ItemDrop itemDropPrefab;

    private void Start()
    {
        // subscribe to health events
        myHealth.OnHealthDepleted += Break;
    }

    public void Interact()
    {
        myHealth.TakeDamage(1);
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

                // drop
                ItemDrop drop = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
                drop.Init(GameManager.Instance.player.transform, materialInstance);
            }
        }
        //Destroy resource after its been harvested
        Destroy(gameObject);
    }
}
