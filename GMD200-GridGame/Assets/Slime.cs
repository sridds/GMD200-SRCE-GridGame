using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    private float damageAmount;

    [Header("Move Settings")]

    [SerializeField]
    private float chargeTime = 2.0f;

    [SerializeField]
    private float moveTime = 3.0f;

    [SerializeField]
    private float moveStrength = 3.0f;

    [SerializeField]
    private float playerDetectRadius;

    [Header("Sounds")]
    [SerializeField]
    private string slimeHurtKey = "slime_hurt";

    [SerializeField]
    private string slimeDeathKey = "slime_death";

    [Header("VFX")]
    [SerializeField]
    private Animator animator;

    private bool stunned;
    private Health health;
    private Rigidbody2D rb;
    private float moveTimer;

    private void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();

        health.OnHealthDepleted += HealthDepleted;
        health.OnHealthUpdate += HealthUpdate;
    }

    private void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer > chargeTime)
        {
            animator.SetTrigger("Charge");
            animator.SetBool("Idle", false);
        }

        if (moveTimer < moveTime) return;
        moveTimer = 0.0f;
        animator.SetTrigger("Jump");
        animator.SetBool("Idle", true);

        if (Vector2.Distance(GameManager.Instance.player.transform.position, transform.position) < playerDetectRadius) {
            rb.AddForce((GameManager.Instance.player.transform.position - transform.position).normalized * moveStrength, ForceMode2D.Impulse);
        }
        else {
            rb.AddForce(Random.insideUnitCircle * moveStrength, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void HealthUpdate(int oldHealth, int newHealth) => AudioHandler.instance.ProcessAudioData(transform, slimeHurtKey);

    private void HealthDepleted()
    {
        AudioHandler.instance.ProcessAudioData(transform, slimeDeathKey);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        if(other.gameObject.TryGetComponent<Health>(out Health health)) {
            health.DecreaseStat(damageAmount);
        }
    }
}
