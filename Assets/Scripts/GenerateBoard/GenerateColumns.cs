using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateColumns : MonoBehaviour
{
    public static SelectableCell[,] gameObjectBoardState;
    
    [SerializeField] private GameObject column;
    [SerializeField] private SelectableCell prefabSpot;
    private List<GameObject> _columns = new();
    
    private void Start()
    {
        SpawnIndividualSpots();
    }

    private void SpawnIndividualSpots()
    {
        var y_length = GameManager.instance.GetYLength;
        var x_length = GameManager.instance.GetXLength;
        gameObjectBoardState = new SelectableCell[x_length, y_length];
        for (int x = 0; x < GameManager.instance.GetXLength; x++)
        {
            for (int y = 0; y < GameManager.instance.GetYLength; y++)
            {
                var spot = Instantiate(prefabSpot, new Vector3(x, y), Quaternion.identity);
                spot.name = $"{x},{y}";
                spot.Init(x, y);
                gameObjectBoardState[x, y] = spot;
            }
        }
    }
}
