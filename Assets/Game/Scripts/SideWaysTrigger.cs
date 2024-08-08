using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWaysTrigger : MonoBehaviour
{
    [SerializeField] private SpickColumSide side;

    public event EventHandler<SpickColumSide> onPlayerReachThisSide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onPlayerReachThisSide?.Invoke(this, side);
        }
    }
}
