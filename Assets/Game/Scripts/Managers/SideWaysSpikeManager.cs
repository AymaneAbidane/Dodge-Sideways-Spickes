using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWaysSpikeManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SideWaysTrigger[] sidesTriggers;

    public event EventHandler<SpickColumSide> onPlayerChooseHisFirstSide;
    public event EventHandler<SpickColumSide> onPlayerReachesOnOfTheSides;

    private void Awake()
    {
        foreach (var trigger in sidesTriggers)
        {
            trigger.onPlayerReachThisSide += Trigger_onPlayerReachThisSide;
        }
    }

    private void Trigger_onPlayerReachThisSide(object sender, SpickColumSide e)
    {
        if (e == SpickColumSide.Right)
        {
            onPlayerReachesOnOfTheSides?.Invoke(this, SpickColumSide.Left);
        }
        else
        {
            onPlayerReachesOnOfTheSides?.Invoke(this, SpickColumSide.Right);
        }
    }

    private void OnDisable()
    {
        foreach (var trigger in sidesTriggers)
        {
            trigger.onPlayerReachThisSide -= Trigger_onPlayerReachThisSide;
        }
    }
    private void Start()
    {
        ActivateSpickesOnOneSide();
    }

    private void ActivateSpickesOnOneSide()
    {
        if (player.GetXDirection() == 1f)
        {
            onPlayerChooseHisFirstSide?.Invoke(this, SpickColumSide.Right);
        }
        else
        {
            onPlayerChooseHisFirstSide?.Invoke(this, SpickColumSide.Left);
        }
    }
}
