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

    private const float Z_ANGLE = 90f;
    private const int MIN_NUMBER_OF_INDEXES = 3;
    private const int MAX_NUMBER_OF_INDEXES = 5;

    private void Awake()
    {
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

    void Start()
    {
        StartCoroutine(COR_MoveColumsAndActivateSpickes());
    }

    private IEnumerator COR_MoveColumsAndActivateSpickes()
    {
        while (true)
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
                ColumnMovment(xMoveForwardEndPosition, xHideposition);
            }
            else
            {
                ColumnMovment(-xMoveForwardEndPosition, -xHideposition);
            }

            yield return new WaitForSeconds(timeOut + timeOut + 0.15f);
        }
    }

    private void ColumnMovment(float xMoveForward, float xHide)
    {
        transform.DOMoveX(xMoveForward, timeIn).OnComplete(() =>
        {
            transform.DOMoveX(xHide, timeOut);
        });
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

    [Button]
    private void TEST_GenerateIndexes()
    {
        int[] array = GenerateRandomIndexes(spickesOnTheColumnArrays.Length, UnityEngine.Random.Range(3, 9));
        foreach (var item in array)
        {
            Debug.Log(item);
        }
    }
}
