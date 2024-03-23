using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOvertime : MonoBehaviour
{
    [SerializeField]
    private float _destroyTime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _destroyTime);
    }
}
