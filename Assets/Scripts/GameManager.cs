using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Board board;
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
    private PlayerType currentPlayer = PlayerType.A;
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
        _turnID.UpdateTurnIDText(currentPlayer);
        X_Length = n;
        Y_Length = m;
        R = r;
        new Board(); // create the board singleton
        GetCurrentPlayer().GetPlayerInput();
    }

    [ContextMenu("Test Undo")]
    public void TestUndo()
    {
        Board.instance.Undo();
    }
    
    public void HandleColumnClick(int columnIndex)
    {
        _turnID.UpdateTurnIDText(currentPlayer);
        Debug.Log($"clicked on column {columnIndex}");
        // Drop a new coin at the bottom most valid row
        var rowIndex = Board.instance.FindRowForNewCoin(columnIndex);
        Debug.Log("RowIndex: " + rowIndex);
        if (rowIndex != -1)
        {
            // Coin should go to the current column clicked at the first empty valid row
            PlaceCoinOnBoard(columnIndex, rowIndex);
            Board.instance.DropCoinAtPosition(columnIndex, rowIndex, currentPlayer);
            WinCheck(Board.instance.CheckForWin());
        }
    }

    // Create the coin game object and place it in the right spot in the board
    private void PlaceCoinOnBoard(int columnIndex, int rowIndex)
    {
        Debug.Log($"ColumnIndex: {columnIndex} | RowIndex: {rowIndex}");
        Debug.Log("CurrentPlayer: " + currentPlayer);
        
        GenerateColumns.gameObjectBoardState[columnIndex, rowIndex]
            .GetPlayerTypeGameObject(currentPlayer)
            .gameObject.SetActive(true);
    }

    public void WinCheck(bool hasWinner)
    {
        if (hasWinner)
        {
            // Show Winner
            Debug.Log($"Player {currentPlayer} has won");
            // Debug.Break();
        }
        else
        {
            shouldKeepPlaying = true;
            SwitchPlayer();
            GetCurrentPlayer().GetPlayerInput();
        }
    }
    
    // only 2 players so they can be player 1 and player 2 and switched to one or the other depending on the current player value
    public void SwitchPlayer()
    {
        switch (currentPlayer)
        {
            case PlayerType.A:
                currentPlayer = PlayerType.B;
                break;
            case PlayerType.B:
                currentPlayer = PlayerType.A;
                break;
        }
        
        _turnID.UpdateTurnIDText(currentPlayer);
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer == PlayerType.A ? player1 : player2;
    }

    public PlayerType GetCurrentPlayerType()
    {
        return currentPlayer;
    }
}