using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public struct BreakableData
{
    public ToolSO CachedTool;
    public int DamageDealt;

    public bool IsRandomDamage;

    [AllowNesting]
    [ShowIf(nameof(IsRandomDamage))]
    public int MaxDamageDealt;
}

public interface IBreakable
{
    public List<BreakableData> BreakableDatas { get; }

    public void Damage(ToolSO tool);
}

public class Breakable : MonoBehaviour, IBreakable
{
    [Header("Resource Settings")]

    [Tooltip("The health of the resource")]
    [SerializeField] private Health myHealth;
    [SerializeField] private ItemDropper dropper;

    [SerializeField] private string damageSoundKey;
    [SerializeField] private string destroySoundKey;

    [field: SerializeField] public List<BreakableData> BreakableDatas { get; private set; }

    [Header("Visuals")]
    [SerializeField] private ParticleSystem destroyParticle;

    private void Start()
    {
        // subscribe to health events
        myHealth.OnHealthDepleted += Break;
    }


    public void Damage(ToolSO tool)
    {
        BreakEffects();

        AudioHandler.instance.ProcessAudioData(transform, damageSoundKey);

        // handle the damage
        foreach (BreakableData data in BreakableDatas)
        {
            if(data.CachedTool.ItemName == tool.ItemName)
            {
                myHealth.DecreaseStat(data.IsRandomDamage ? Random.Range(data.DamageDealt, data.MaxDamageDealt + 1) : data.DamageDealt);

                return;
            }
        }

        // take no damage if tool is too weak
        myHealth.DecreaseStat(0);
    }

    /// <summary>
    /// Destroys object and drops items
    /// </summary>
    private void Break()
    {
        AudioHandler.instance.ProcessAudioData(transform, destroySoundKey);

        BreakEffects();
        if (destroyParticle != null) Instantiate(destroyParticle, transform.position, Quaternion.identity);

        dropper.DropItem();

        //Destroy resource after its been harvested
        Destroy(gameObject);
    }

    private void BreakEffects()
    {
        CameraShake.instance.Shake(0.2f, 0.15f);
    }
}
