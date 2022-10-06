using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateColumns : MonoBehaviour
{
    public static GameObject[,] gameObjectBoardState;
    
    [SerializeField] private GameObject column;
    [SerializeField] private GameObject prefabSpot;
    private List<GameObject> _columns = new();

    private void Awake()
    {
        var M = GameManager.instance.GetM;
        var N = GameManager.instance.GetN;
        gameObjectBoardState = new GameObject[N, M];
    }

    private void Start()
    {
        // SpawnColumns();
        SpawnIndividualSpots();
    }

    private void SpawnColumns()
    {
        for (int i = 0; i < GameManager.instance.GetN; i++)
        {
            var column = Instantiate(this.column, new Vector3(3 - i, 3.7f), Quaternion.identity);
            column.transform.localScale = new Vector3(1, GameManager.instance.GetM, 1);
            column.name = $"Column_{i}";
            _columns.Add(column);
        }
    }

    private void SpawnIndividualSpots()
    {
        for (int i = GameManager.instance.GetN - 1; i >= 0; i--)
        {
            for (int j = 0; j < GameManager.instance.GetM; j++)
            {
                var spot = Instantiate(prefabSpot, new Vector3(3 - i, 1.15f + j), Quaternion.identity);
                spot.name = $"{j},{i}";
                gameObjectBoardState[i, j] = spot;
            }
        }
    }
}
