using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, -10);

    Transform target;

    private void Awake() => target = GameObject.FindWithTag("Player").transform;

    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
