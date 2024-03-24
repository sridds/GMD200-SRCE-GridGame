using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    private float damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        if(other.gameObject.TryGetComponent<Health>(out Health health)) {
            health.DecreaseStat(damageAmount);
        }
    }
}
