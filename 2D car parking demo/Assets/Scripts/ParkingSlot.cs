using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSlot : MonoBehaviour
{
    private BoxCollider2D _collider;

    private bool isCarParked = false;        // true if car is parked
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isCarParked)
            return;
        if(_collider.bounds.Contains(collision.bounds.max) && _collider.bounds.Contains(collision.bounds.min))
        {
            isCarParked = true;
            UIManager.Instance.EnableParkedPanel();
        }
    }
}
