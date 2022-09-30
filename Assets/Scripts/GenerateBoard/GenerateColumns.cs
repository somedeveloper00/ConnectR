using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateColumns : MonoBehaviour
{
    [SerializeField] private GameObject column;
    [SerializeField] private GameObject spot;
    private List<GameObject> _columns = new();
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
        for (int i = 0; i < GameManager.instance.GetN; i++)
        {
            for (int j = 0; j < GameManager.instance.GetM; j++)
            {
                var spot = Instantiate(this.spot, new Vector3(3 - i, 1.15f + j), Quaternion.identity);
                spot.name = $"{i},{j}";
            }
        }
    }
}
