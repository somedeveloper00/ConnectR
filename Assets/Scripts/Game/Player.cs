using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Game;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private bool isAi;
    [SerializeField] private int depth = 5;
    private float boardStateUtility; // value for the current board state used in min & max choices
    private int bestMoveSeenSoFar; // Column that represents the best play based off board
    private Stopwatch _stopwatch = new Stopwatch();
    private bool ranOutOfTime = false;
    
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
        var actingPlayerType = GameManager.instance.GetCurrentPlayerType();
        Debug.Log($"AI starting... {actingPlayerType}");
        _stopwatch.Restart();
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
            
            var heuristicValueOfCurrentState = PerformMinimax(depth, false, ref actingPlayerType);
            Debug.Log($"Heuristic Value {heuristicValueOfCurrentState} columnIndex {column}");
            Board.instance.Undo();
            GameManager.instance.SwitchPlayer();
            if (heuristicValueOfCurrentState > value)
            {
                value = heuristicValueOfCurrentState;
                bestColumn = column;
            }
        }

        if (ranOutOfTime)
        {
            Debug.LogWarning("Ran our of time");
            ranOutOfTime = false;
        }
        
        return bestColumn;
    }

    // calculate utility of state
    private float PerformMinimax(int depth, bool isMaximizing, ref PlayerType actingPlayerType)
    {
        if (_stopwatch.Elapsed.TotalSeconds >= 1) ranOutOfTime = true;

        var isTerminalState = Board.instance.CheckForWin();
        if (depth == 0 || isTerminalState || _stopwatch.Elapsed.TotalSeconds >= 1)
        {
            bool actingPlayerWon = isTerminalState && GameManager.instance.GetCurrentPlayerType() != actingPlayerType;
            bool opponentPlayerWon = isTerminalState && GameManager.instance.GetCurrentPlayerType() == actingPlayerType;
            
            var h=  HeuristicValueForState(actingPlayerWon, opponentPlayerWon, ref actingPlayerType);
            // if (isTerminalState)
            // {
            //     Board.instance.DebugBoard();
            //     Debug.Log($"heuristic: {h}");
            // }

            return h;

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
                var heuristicValueOfCurrentState = PerformMinimax(depth - 1, false, ref actingPlayerType);
                Board.instance.Undo();
                GameManager.instance.SwitchPlayer();
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
                var heuristicValueOfCurrentState = PerformMinimax(depth - 1, true, ref actingPlayerType);
                Board.instance.Undo();
                GameManager.instance.SwitchPlayer();
                if (heuristicValueOfCurrentState < value)
                {
                    value = heuristicValueOfCurrentState;
                }
            }

            return value;
        }
    }

    // assign a heuristic value for a given state
    private float HeuristicValueForState(bool actingPlayerWon, bool opponentWon, ref PlayerType actingPlayerType)
    {
        if (actingPlayerWon) return float.MaxValue;
        if (opponentWon) return float.MinValue;
        
        // return heuristic value for current state of the board
        float h = 0;
        

        return 0;
    }

    // 
    public void SetPlayerToAi(bool isAi)
    {
        this.isAi = isAi;
    }
}
