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
    [SerializeField] private int X_Length;
    public int GetXLength => X_Length;
    [SerializeField] private int Y_Length;
    public int GetYLength => Y_Length;
    [SerializeField] private int R;
    public int GetR => R;
    private Player currentPlayer = Player.A;
    private bool shouldKeepPlaying = true;

    private void Awake()
    {
        instance = this;
    }
    

    public void HandleColumnClick(int columnIndex)
    {
        // Drop a new coin at the bottom most valid row
        var rowIndex = Board.instance.FindRowForNewCoin(columnIndex);
        Debug.Log("RowIndex: " + rowIndex);
        if (rowIndex != -1)
        {
            // Coin should go to the current column clicked at the first empty valid row
            Board.instance.DropCoinAtPosition(columnIndex, rowIndex, currentPlayer);
            PlaceCoinOnBoard(rowIndex, columnIndex);
        }
    }

    // Create the coin game object and place it in the right spot in the board
    private void PlaceCoinOnBoard(int columnIndex, int rowIndex)
    {
        Debug.Log($"ColumnIndex: {columnIndex} | RowIndex: {rowIndex}");
        Debug.Log("CurrentPlayer: " + currentPlayer);
        GenerateColumns.gameObjectBoardState[columnIndex, rowIndex]
            .gameObject.transform.GetChild(currentPlayer == Player.A ? 4 : 5).gameObject.SetActive(true);
        /*GameObject playerCoin = Instantiate(currentPlayer == 1 ? redCoin : yellowCoin);
        playerCoin.transform.position = new Vector3(-(startingPoint.transform.position.x + columnIndex),
            startingPoint.transform.position.y - rowIndex, startingPoint.transform.position.z);*/

        // switch to the other player for proper color coin to be placed next
    }

    public void WinCheck(bool hasWinner)
    {
        if (hasWinner)
        {
            // Show Winner
            Debug.Log($"Player {currentPlayer} has won");
            Debug.Break();
        }
        else
        {
            shouldKeepPlaying = true;
            SwitchPlayer();
        }
    }
    
    // only 2 players so they can be player 1 and player 2 and switched to one or the other depending on the current player value
    private void SwitchPlayer()
    {
        switch (currentPlayer)
        {
            case Player.A:
                currentPlayer = Player.B;
                break;
            case Player.B:
                currentPlayer = Player.A;
                break;
        }
    }
}
