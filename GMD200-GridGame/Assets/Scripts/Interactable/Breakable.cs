using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Breakable : MonoBehaviour
{
    [Header("Resource Settings")]

    [Tooltip("The health of the resource")]
    [SerializeField] private Health myHealth;
    [SerializeField] private ItemDropper dropper;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem destroyParticle;

    private void Start()
    {
        // subscribe to health events
        myHealth.OnHealthDepleted += Break;
        myHealth.OnHealthUpdate += Damage;
    }


    private void Damage(int oldHealth, int newHealth) => BreakEffects();

    /// <summary>
    /// Destroys object and drops items
    /// </summary>
    private void Break()
    {
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
