using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject redCoin;
    [SerializeField] private GameObject yellowCoin;
    [SerializeField] private GameObject startingPoint;
    [SerializeField] private int N;
    public int GetN => N;
    [SerializeField] private int M;
    public int GetM => M;
    [SerializeField] private int R;
    public int GetR => R;
    private int currentPlayer = 1;
    private bool shouldKeepPlaying = true;

    private void Awake()
    {
        instance = this;
    }
    
    public void HandleColumnClick(int columnIndex)
    {
        // Drop a new coin at the bottom most valid row
        var rowIndex = Board.instance.FindRowForNewCoin(columnIndex);
        Debug.Log(rowIndex);
        if (rowIndex != -1)
        {
            // Coin should go to the current column clicked at the first empty valid row
            Board.instance.DropCoinAtPosition(rowIndex, columnIndex, currentPlayer);
            PlaceCoinOnBoard(rowIndex, columnIndex);
        }
    }

    // Create the coin game object and place it in the right spot in the board
    private void PlaceCoinOnBoard(int rowIndex, int columnIndex)
    {
        GameObject playerCoin = Instantiate(currentPlayer == 1 ? redCoin : yellowCoin);
        playerCoin.transform.position = new Vector3(-(startingPoint.transform.position.x + columnIndex),
            startingPoint.transform.position.y - rowIndex, startingPoint.transform.position.z);
        
        // switch to the other player for proper color coin to be placed next
        SwitchPlayer();
    }

    // only 2 players so they can be player 1 and player 2 and switched to one or the other depending on the current player value
    private void SwitchPlayer()
    {
        switch (currentPlayer)
        {
            case 1:
                currentPlayer = 2;
                break;
            case 2:
                currentPlayer = 1;
                break;
        }
    }
}
