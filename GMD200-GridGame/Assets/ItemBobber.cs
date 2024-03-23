using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBobber : MonoBehaviour
{
    [SerializeField]
    private float _sinSpeed;
    [SerializeField]
    private float _sinAmplitude;

    void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, Mathf.Sin(Time.time * _sinSpeed) * _sinAmplitude);
    }
}
