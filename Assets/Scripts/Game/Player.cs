using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private bool isAi;
    [SerializeField] private int depth = 5;
    private float boardStateUtility; // value for the current board state used in min & max choices
    private int bestMoveSeenSoFar; // Column that represents the best play based off board
    
    public void DropCoin(int x)
    {
        
        // called via script no column click needed
        GameManager.instance.HandleColumnClick(x);
        HandleUserInput.handleUserInputSelection -= DropCoin;
    }

    public void GetPlayerInput()
    {
        // if not ai subscribe the callback for column selection
        if(!isAi)
            HandleUserInput.handleUserInputSelection += DropCoin;

        if (isAi)
        {
            DropCoin(GetActionFromMiniMax());
        }
    }

    // return the column index to drop coin
    private int GetActionFromMiniMax()
    {
        // MAX get all possible moves
        var possibleColumns = Board.instance.GetAllValidColumns();
        var value = float.MinValue;
        var bestColumn = 0;
        foreach (var column in possibleColumns)
        {
            // place a coin, check the heuristic of that move and then undo
            var rowIndex = Board.instance.FindRowForNewCoin(column);
            Board.instance.DropCoinAtPosition(column, rowIndex, GameManager.instance.GetCurrentPlayerType());
            GameManager.instance.SwitchPlayer();
            var heuristicValueOfCurrentState = PerformMinimax(depth, false);
            Board.instance.Undo();
            if (heuristicValueOfCurrentState > value)
            {
                value = heuristicValueOfCurrentState;
                bestColumn = column;
            }
        }
        return bestColumn;
    }

    // calculate utility of state
    private float PerformMinimax(int depth, bool isMaximizing)
    {
        var checkWin = Board.instance.CheckForWin();
        if (depth == 0 || checkWin)
        {
            return HeuristicValueForState(checkWin);
        }

        if (isMaximizing)
        {
            // MAX get all possible moves
            var possibleColumns = Board.instance.GetAllValidColumns();
            var value = float.MinValue;
            foreach (var column in possibleColumns)
            {
                // place a coin, check the heuristic of that move and then undo
                var rowIndex = Board.instance.FindRowForNewCoin(column);
                Board.instance.DropCoinAtPosition(column, rowIndex, GameManager.instance.GetCurrentPlayerType());
                GameManager.instance.SwitchPlayer();
                var heuristicValueOfCurrentState = PerformMinimax(depth - 1, false);
                Board.instance.Undo();
                if (heuristicValueOfCurrentState > value)
                {
                    value = heuristicValueOfCurrentState;
                }
            }

            return value;
        }
        else
        {
            // MIN get all possible moves
            var possibleColumns = Board.instance.GetAllValidColumns();
            var value = float.MaxValue;
            
            foreach (var column in possibleColumns)
            {
                // place a coin, check the heuristic of that move and then undo
                var rowIndex = Board.instance.FindRowForNewCoin(column);
                Board.instance.DropCoinAtPosition(column, rowIndex, GameManager.instance.GetCurrentPlayerType());
                GameManager.instance.SwitchPlayer();
                var heuristicValueOfCurrentState = PerformMinimax(depth - 1, true);
                Board.instance.Undo();
                if (heuristicValueOfCurrentState < value)
                {
                    value = heuristicValueOfCurrentState;
                }
            }

            return value;
        }
    }

    // assign a heuristic value for a given state
    private float HeuristicValueForState(bool didPlayerWin)
    {
        // if currently acting player has won then return positive infinity
        if (didPlayerWin) return float.MaxValue;

        return 0;
    }

    // 
    public void SetPlayerToAi(bool isAi)
    {
        this.isAi = isAi;
    }
}
