using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public void ApplyKnockback(Transform dealer, float knockbackAmount)
    {
        // apply knockback force
        rb.AddForce((transform.position - dealer.position).normalized * knockbackAmount, ForceMode2D.Impulse);
    }
}
