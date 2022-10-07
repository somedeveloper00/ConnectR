using UnityEngine;

public struct Cell
{
    public bool isEmpty;
    public Player player;

    public override string ToString() => isEmpty ? "-" : player.ToString();
    public static bool operator ==(Cell c1, Cell c2) => c1.isEmpty == c2.isEmpty && c1.player == c2.player;
    public static bool operator !=(Cell c1, Cell c2) => !(c1 == c2);
}

public enum Player { A, B }

public class Board : MonoBehaviour
{
    private Cell[,] boardState;
    public static Board instance;
    
    // initialize variables in Awake because it runs before Start
    private void Awake()
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
    }

    private void Start()
    {
        Debug.Log(DebugBoard());
    }
    
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

    // set the board state to show which player has a coin at the given position
    // enable a game object in the scene to indicate whether coin is red or yellow
    public void DropCoinAtPosition(int column, int row, Player player)
    {
        boardState[column, row].isEmpty = false;
        boardState[column, row].player = player;
        
        Debug.Log(DebugBoard());
        // Win Check
        GameManager.instance.WinCheck(CheckForWin());
    }

    private string DebugBoard()
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
        return debugStr;
    }

    private bool CheckForWin()
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
                        Debug.Log($"Vertical winner found - Player {boardState[x, y]}");
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
                        Debug.Log($"Winner found horizontally - Player {boardState[x, y]}");
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
                    Debug.Log($"Winner found Diagonally - Player {boardState[x, y]}");
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
                        Debug.Log($"Winner found Diagonally - Player {boardState[x, y]}");
                        return true;
                    }
                }
            }

        return false;
    }
}
