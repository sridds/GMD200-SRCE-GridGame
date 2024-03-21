using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Breakable : MonoBehaviour
{
    [Header("Resource Settings")]

    [Tooltip("The health of the resource")]
    [SerializeField] private Health myHealth;

    [SerializeField] private ItemDropper dropper;

    private void Start()
    {
        // subscribe to health events
        myHealth.OnHealthDepleted += Break;
        myHealth.OnHealthUpdate += Damage;
    }


    private void Damage(int oldHealth, int newHealth)
    {
        CameraShake.instance.Shake(0.2f, 0.15f);
    }

    /// <summary>
    /// Destroys object and drops items
    /// </summary>
    private void Break()
    {
        CameraShake.instance.Shake(0.2f, 0.15f);
        dropper.DropItem();

        //Destroy resource after its been harvested
        Destroy(gameObject);
    }
}
