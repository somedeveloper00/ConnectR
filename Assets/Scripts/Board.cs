using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Cell
{
    public bool isEmpty;
    public PlayerType player;

    public override string ToString() => isEmpty ? "-" : player.ToString();
    public static bool operator ==(Cell c1, Cell c2) => c1.isEmpty == c2.isEmpty && c1.player == c2.player;
    public static bool operator !=(Cell c1, Cell c2) => !(c1 == c2);
}

public enum PlayerType { A, B }

public class Board 
{
    private Cell[,] boardState;
    public static Board instance;
    public List<Vector2Int> allInputs = new List<Vector2Int>();
    
    // initialize variables in Awake because it runs before Start
    public Board()
    {
        var y_length = GameManager.instance.GetYLength;
        var x_length = GameManager.instance.GetXLength;
        instance = this;
        
        // setting board state
        boardState = new Cell[x_length, y_length];
        for (int x = 0; x < x_length; x++)
        for (int y = 0; y < y_length; y++)
        {
            boardState[x, y].isEmpty = true;
        }
        
        DebugBoard();
    }

    /*private void Start()
    {
        Debug.Log(DebugBoard());
    }*/
    
    // starting at the bottom of the column, check if this spot has a 0 for a value
    // if a 0 is found then a coin can go there else it is full
    public int FindRowForNewCoin(int columnIndex)
    {
        var y_length = GameManager.instance.GetYLength;
        for (int y = 0; y < y_length; y++)
        {
            // return the first row of the column which has no coin placed in it
            if (boardState[columnIndex, y].isEmpty)
            {
                return y;
            }
        }
        Debug.Log("No Valid Move Possible!");
        return -1;
    }

    // if there is at least one empty space in a column it is a valid column
    public List<int> GetAllValidColumns()
    {
        var x_length = GameManager.instance.GetXLength;
        var y_length = GameManager.instance.GetYLength;
        var validColumns = new List<int>();
        for (int x = 0; x < x_length; x++)
        {
            if (boardState[x, y_length - 1].isEmpty)
            {
                validColumns.Add(x);
            }
        }
        return validColumns;
    }
    
    // need to be able to go back to the previous board
    public void Undo()
    {
        boardState[allInputs.Last().x, allInputs.Last().y].isEmpty = true;
        allInputs.RemoveAt(allInputs.Count - 1);
        // DebugBoard();
    }

    // set the board state to show which player has a coin at the given position
    // enable a game object in the scene to indicate whether coin is red or yellow
    public void DropCoinAtPosition(int column, int row, PlayerType player)
    {
        allInputs.Add(new Vector2Int(column, row)); // every position of all coins placed on board
        boardState[column, row].isEmpty = false;
        boardState[column, row].player = player;

        // DebugBoard();
        // Win Check
        // GameManager.instance.WinCheck(CheckForWin());
    }

    private void DebugBoard()
    {
        var y_length = GameManager.instance.GetYLength;
        var x_length = GameManager.instance.GetXLength;
        string debugStr = "";
        string delimiter = ",";
        string border = "|";
        for (int y = y_length - 1; y >= 0; y--)
        {
            debugStr += border;
            for (int x = 0; x < x_length; x++) 
                debugStr += boardState[x, y] + ",";
            debugStr += border + "\n";
        }
        Debug.Log(debugStr);
    }

    // used also in minimax to detect winning states
    public bool CheckForWin()
    {
        if (HorizontalWinCheck() || VerticalWinCheck() || DiagonalWinCheck())
            return true;

        return false;
    }

    private bool VerticalWinCheck()
    {
        var x_length = GameManager.instance.GetXLength;
        var y_length = GameManager.instance.GetYLength;
        var R = GameManager.instance.GetR;
        
        // Check row and column for a placed coin and then check upcoming coins
        for (int x = 0; x < x_length; x++)
        {
            for (int y = 0; y < y_length - R; y++)
            {
                if (!boardState[x, y].isEmpty)
                {
                    using var check = new WinCheck(R);
                    check.Sample(boardState[x, y]);
                    for (int k = 1; k < R; k++) check.Sample(boardState[x, y + k]);
                        
                    if (check.won)
                    {
                        Debug.Log($"Vertical winner found - PlayerType {boardState[x, y]}");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool HorizontalWinCheck()
    {
        var y_length = GameManager.instance.GetYLength;
        var x_length = GameManager.instance.GetXLength;
        var R = GameManager.instance.GetR;
     
        // Check row and column for a placed coin and then check upcoming coins
        for (int y = 0; y < y_length; y++)
        {
            for (int x = 0; x < x_length - R; x++)
            {
                if (!boardState[x, y].isEmpty)
                {
                    using var check = new WinCheck(R);
                    check.Sample(boardState[x, y]);
                    for (int k = 1; k < R; k++) check.Sample(boardState[x + k, y]);
                        
                    if (check.won)
                    {
                        Debug.Log($"Winner found horizontally - PlayerType {boardState[x, y]}");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool DiagonalWinCheck()
    {
        var x_length = GameManager.instance.GetXLength;
        var y_length = GameManager.instance.GetYLength;
        var R = GameManager.instance.GetR;

        // bottom-left
        for (int y = R - 1; y < y_length; y++)
        for (int x = R - 1; x < x_length; x++)
            if (!boardState[x, y].isEmpty)
            {
                using var check = new WinCheck(R);
                check.Sample(boardState[x, y]);
                for (int k = 1; k < R; k++) check.Sample(boardState[x - k, y - k]);

                if (check.won)
                {
                    Debug.Log($"Winner found Diagonally - PlayerType {boardState[x, y]}");
                    return true;
                }
            }

        // top-left
        for (int y = 0; y < y_length - R; y++)
        for (int x = R - 1; x < x_length; x++)
            if (!boardState[x, y].isEmpty)
            {
                using (var check = new WinCheck(R))
                {
                    check.Sample(boardState[x, y]);
                    for (int k = 1; k < R; k++) check.Sample(boardState[x - k, y + k]);

                    if (check.won)
                    {
                        Debug.Log($"Winner found Diagonally - PlayerType {boardState[x, y]}");
                        return true;
                    }
                }
            }

        return false;
    }
}
