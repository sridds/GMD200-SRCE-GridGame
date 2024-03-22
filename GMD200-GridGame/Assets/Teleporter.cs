using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Transform teleportPoint;

    [SerializeField]
    private Vector3 offset;

    [Tooltip("Indicates whether or not the teleport function must be called or if it is handled on trigger")]
    [SerializeField]
    private bool independent = false;

    /// <summary>
    /// Teleports the provided target to the target position
    /// </summary>
    /// <param name="target"></param>
    public void Teleport(Transform target)
    {
        target.position = teleportPoint.position + offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (independent) return;
        if (collision.gameObject.tag != "Player") return;

        Teleport(collision.gameObject.transform);
    }
}
