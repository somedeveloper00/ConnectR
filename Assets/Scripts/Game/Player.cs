using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Game;
using Unity.VisualScripting;
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
        var actingPlayerType = GameManager.instance.board.GetCurrentPlayerType();
        Debug.Log($"AI starting... {actingPlayerType}");
        
        _stopwatch.Restart();
        
        // MAX get all possible moves
        var board = GameManager.instance.board;
        var possibleColumns = board.GetAllValidColumns();
        var value = float.MinValue;
        var bestColumn = 0;
        foreach (var column in possibleColumns)
        {
            // place a coin, check the heuristic of that move and then undo
            var rowIndex = board.FindRowForNewCoin(column);
            var newBoard = board.Clone();
            newBoard.DropCoinAtPosition(column, rowIndex);
            
            var heuristicValueOfCurrentState = PerformMinimax(newBoard, depth, false, float.MinValue, float.MaxValue, actingPlayerType);
            Debug.Log($"Heuristic Value {heuristicValueOfCurrentState} columnIndex {column}");
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
    private float PerformMinimax(Board board, int depth, bool isMaximizing, float alpha, float beta, PlayerType actingPlayerType)
    {
        if (_stopwatch.Elapsed.TotalSeconds >= 1) ranOutOfTime = true;

        var isTerminalState = board.CheckForWin();
        if (depth == 0 || isTerminalState || _stopwatch.Elapsed.TotalSeconds >= 1)
        {
            var currentPlayer = board.GetCurrentPlayerType();
            bool actingPlayerWon = isTerminalState && currentPlayer != actingPlayerType;
            bool opponentPlayerWon = isTerminalState && currentPlayer == actingPlayerType;
            
            var h=  HeuristicValueForState(board, actingPlayerWon, opponentPlayerWon, actingPlayerType, depth);

            return h;

        }

        if (isMaximizing)
        {
            // MAX get all possible moves
            var possibleColumns = board.GetAllValidColumns();
            var value = float.MinValue;
            foreach (var column in possibleColumns)
            {
                // place a coin, check the heuristic of that move and then undo
                var rowIndex = board.FindRowForNewCoin(column);
                board.DropCoinAtPosition(column, rowIndex);
                var heuristicValueOfCurrentState = PerformMinimax(board, depth - 1, false, alpha, beta, actingPlayerType);
                board.Undo();
                
                if (heuristicValueOfCurrentState > value)
                {
                    value = heuristicValueOfCurrentState;
                }

                // alpha = lowest value found
                // beta = highest value found
                if (value >= beta) break;
                alpha = Mathf.Max(alpha, value);
            }

            return value;
        }
        else
        {
            // MIN get all possible moves
            var possibleColumns = board.GetAllValidColumns();
            var value = float.MaxValue;
            
            foreach (var column in possibleColumns)
            {
                // place a coin, check the heuristic of that move and then undo
                var rowIndex = board.FindRowForNewCoin(column);
                board.DropCoinAtPosition(column, rowIndex);
                var heuristicValueOfCurrentState = PerformMinimax(board, depth - 1,  true, alpha, beta, actingPlayerType);
                board.Undo();
                
                if (heuristicValueOfCurrentState < value)
                {
                    value = heuristicValueOfCurrentState;
                }
                if (value <= alpha) break;

                beta = Mathf.Min(beta, value);
            }
            
            return value;
        }
    }

    // assign a heuristic value for a given state
    private float HeuristicValueForState(Board board, bool actingPlayerWon, bool opponentWon, PlayerType actingPlayerType, int depth)
    {
        if (actingPlayerWon)
        {
            return depth * 100_000_000;
        }

        if (opponentWon)
        {
            return depth * -100_000_000;
        }

        // return heuristic value for current state of the board
        float h = 0;

        int actingPlayerConnectedNeighbors = 0;
        int opponentPlayerConnectedNeighbors = 0;

        for (int x = 0; x < GameManager.instance.GetXLength; x++)
        for (int y = 0; y < GameManager.instance.GetYLength; y++)
        {
            var currentCell = board.GetCells()[x, y];
            
            if(currentCell.isEmpty) continue;

            // get acting players' neighbors
            if (currentCell.player == actingPlayerType)
            {
                var n = board.GetCellsNeighbors(new Vector2Int(x, y));
                foreach (var neighbor in n)
                {
                    if (board.GetCells()[neighbor.x, neighbor.y].player == actingPlayerType)
                        actingPlayerConnectedNeighbors++;
                }
            }
            // get opponent' neighbors
            else
            {
                var n = board.GetCellsNeighbors(new Vector2Int(x, y));
                foreach (var neighbor in n)
                {
                    if (board.GetCells()[neighbor.x, neighbor.y].player != actingPlayerType)
                        opponentPlayerConnectedNeighbors++;
                }
            }
        }
        

        h += 2 * (actingPlayerConnectedNeighbors - opponentPlayerConnectedNeighbors);
        
        // do other things to h...
        
        return h;
    }
    
    public void SetPlayerToAi(bool isAi)
    {
        this.isAi = isAi;
    }
}
