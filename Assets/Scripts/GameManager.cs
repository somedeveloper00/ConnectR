using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Board board;
    [SerializeField] private GameObject turnID;
    [SerializeField] private GameObject redCoin;
    [SerializeField] private GameObject yellowCoin;
    [SerializeField] private int X_Length;
    public int GetXLength => X_Length;
    [SerializeField] private int Y_Length;
    public int GetYLength => Y_Length;
    [SerializeField] private int R;
    public int GetR => R;
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    private bool shouldKeepPlaying = true;
    private TurnID _turnID;
    

    private void Awake()
    {
        instance = this;
    }

    // Initialization function called when the GameManager in the scene gets set active
    public void Init(int n, int m, int r)
    {
        _turnID = turnID.GetComponent<TurnID>();
        X_Length = n;
        Y_Length = m;
        R = r;
        board = new Board(); // create the board
        _turnID.UpdateTurnIDText(board.GetCurrentPlayerType());
        GetCurrentPlayer().GetPlayerInput();
    }
    
    public void HandleColumnClick(int columnIndex)
    {
        Debug.Log($"player {board.GetCurrentPlayerType()} clicked on column {columnIndex}");
        // Drop a new coin at the bottom most valid row
        var rowIndex = board.FindRowForNewCoin(columnIndex);
        
        if (rowIndex != -1)
        {
            // Coin should go to the current column clicked at the first empty valid row
            PlaceCoinOnBoard(columnIndex, rowIndex);
            board.DropCoinAtPosition(columnIndex, rowIndex);
            WinCheck(board.CheckForWin());
            _turnID.UpdateTurnIDText(board.GetCurrentPlayerType());
        }
    }

    // Create the coin game object and place it in the right spot in the board
    private void PlaceCoinOnBoard(int columnIndex, int rowIndex)
    {
        GenerateColumns.gameObjectBoardState[columnIndex, rowIndex]
            .GetPlayerTypeGameObject(board.GetCurrentPlayerType())
            .gameObject.SetActive(true);
    }

    public void WinCheck(bool hasWinner)
    {
        if (hasWinner)
        {
            // Show Winner
            Debug.Log($"Player {board.GetCurrentPlayerType()} has won");
            Debug.Break();
        }
        else
        {
            shouldKeepPlaying = true;
            GetCurrentPlayer().GetPlayerInput();
        }
    }
    
    public Player GetCurrentPlayer()
    {
        return board.GetCurrentPlayerType() == PlayerType.A ? player1 : player2;
    }

}