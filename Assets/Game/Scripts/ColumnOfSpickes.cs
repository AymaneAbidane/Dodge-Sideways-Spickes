using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum SpickColumSide { Left, Right };

public class ColumnOfSpickes : MonoBehaviour
{
    [SerializeField] private SpickColumSide side;

    [SerializeField] private float xMoveForwardEndPosition;
    [SerializeField] private float xHideposition;
    [SerializeField] private float timeIn;
    [SerializeField] private float timeOut;

    [SerializeField, ChildGameObjectsOnly] private Transform[] spickesOnTheColumnArrays;
    [SerializeField, SceneObjectsOnly] private SideWaysSpikeManager sideWaysSpikeManager;

    private const float Z_ANGLE = 90f;
    private const int MIN_NUMBER_OF_INDEXES = 4;
    private const int MAX_NUMBER_OF_INDEXES = 10;

    private void Awake()
    {
        sideWaysSpikeManager.onPlayerChooseHisFirstSide += SideWaysSpikeManager_onPlayerChooseHisFirstSide;
        sideWaysSpikeManager.onPlayerReachesOnOfTheSides += SideWaysSpikeManager_onPlayerReachesOnOfTheSides;

        if (side == SpickColumSide.Right)
        {
            SetSpickesAngle(Z_ANGLE);
            SetColumsStartPositionOnX(xHideposition);
        }
        else
        {
            SetSpickesAngle(-Z_ANGLE);
            SetColumsStartPositionOnX(-xHideposition);
        }
    }
    private void OnDisable()
    {
        sideWaysSpikeManager.onPlayerChooseHisFirstSide -= SideWaysSpikeManager_onPlayerChooseHisFirstSide;
        sideWaysSpikeManager.onPlayerReachesOnOfTheSides -= SideWaysSpikeManager_onPlayerReachesOnOfTheSides;
    }

    private void SideWaysSpikeManager_onPlayerChooseHisFirstSide(object sender, SpickColumSide e)
    {
        if (e == side)
        {
            ActivateTheSpickes();
        }
    }
    private void SideWaysSpikeManager_onPlayerReachesOnOfTheSides(object sender, SpickColumSide e)
    {
        if (e != side)
        {
            ActivateTheSpickes();
        }
    }


    private void ActivateTheSpickes()
    {

        foreach (var spike in spickesOnTheColumnArrays)
        {
            spike.gameObject.SetActive(false);
        }

        int[] indexesArray = GenerateRandomIndexes(spickesOnTheColumnArrays.Length, UnityEngine.Random.Range(MIN_NUMBER_OF_INDEXES, MAX_NUMBER_OF_INDEXES));

        foreach (var index in indexesArray)
        {
            spickesOnTheColumnArrays[index].gameObject.SetActive(true);
        }

        if (side == SpickColumSide.Right)
        {
            ColumnMoveForward(xMoveForwardEndPosition);
        }
        else
        {
            ColumnMoveForward(-xMoveForwardEndPosition);
        }
    }

    private void ColumnMoveForward(float xMoveForward)
    {
        transform.DOMoveX(xMoveForward, timeIn);
    }

    private void SetSpickesAngle(float zAngle)
    {
        foreach (var spick in spickesOnTheColumnArrays)
        {
            spick.localEulerAngles = Vector3.forward * zAngle;
        }
    }

    private void SetColumsStartPositionOnX(float xHideposition)
    {
        transform.position = new Vector2(xHideposition, transform.position.y);
    }

    public int[] GenerateRandomIndexes(int arrayLength, int numberOfIndexes)
    {
        if (numberOfIndexes > arrayLength)
        {
            Debug.LogError("Number of indexes to generate cannot be greater than the array length.");
            return null;
        }

        HashSet<int> uniqueIndexes = new HashSet<int>();
        System.Random random = new System.Random();

        while (uniqueIndexes.Count < numberOfIndexes)
        {
            int randomIndex = random.Next(0, arrayLength);
            uniqueIndexes.Add(randomIndex);
        }

        return new List<int>(uniqueIndexes).ToArray();
    }
}
