using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSlotUI : MonoBehaviour
{
    [SerializeField]
    private SlotUI[] slots;

    [SerializeField]
    private Vector3 posOffset = new Vector3(0, -0.5f, 0);

    PlayerInteractor interactor;

    void Start(){
        interactor = FindObjectOfType<PlayerInteractor>();
        interactor.OnUpdateCurrentSlot += UpdatePosition;
    }

    private void UpdatePosition(int slot)
    {
        // set position
        transform.position = slots[slot].transform.position + posOffset;
    }
}
