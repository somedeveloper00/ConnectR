using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    
    private int[,] boardState;
    public static Board instance;
    
    // initialize variables in Awake because it runs before Start
    private void Awake()
    {
        var M = GameManager.instance.GetM;
        var N = GameManager.instance.GetN;
        instance = this;
        boardState = new int[M, N];
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                boardState[i, j] = 0;
            }
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
        var M = GameManager.instance.GetM;
        for (int i = M - 1; i >= 0; i--)
        {
            // return the first row of the column which has no coin placed in it
            if (boardState[i, columnIndex] == 0)
            {
                return i;
            }
        }
        Debug.Log("No Valid Move Possible!");
        return -1;
    }

    // set the board state to show which player has a coin at the given position
    // enable a game object in the scene to indicate whether coin is red or yellow
    public void DropCoinAtPosition(int x, int y, int playerCoin)
    {
        boardState[x, y] = playerCoin;
        Debug.Log(DebugBoard());
        // Win Check
        GameManager.instance.WinCheck(CheckForWin());
    }

    private string DebugBoard()
    {
        var M = GameManager.instance.GetM;
        var N = GameManager.instance.GetN;
        string debugStr = "";
        string delimiter = ",";
        string border = "|";
        for (int i = 0; i < M; i++)
        {
            debugStr += border;
            for (int j = 0; j < N; j++)
            {
                debugStr += boardState[i, j]+",";
            }
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
        var N = GameManager.instance.GetN;
        var M = GameManager.instance.GetM;
        var R = GameManager.instance.GetR;
        
        // Check row and column for a placed coin and then check upcoming coins
        for (int i = M - 1; i > M - R; i--)
        {
            for (int j = 0; j < N; j++)
            {
                if (boardState[i, j] > 0)
                {
                    var winCondition = 1;
                    for (int k = 1; k <= R - 1; k++)
                    {
                        /*Debug.Log($"i: {i} j: {j} k: {k}");
                        Debug.Log($"Board state at i, j: {boardState[i,j]}");
                        Debug.Log($"Board state at i - k, j: {boardState[i - k,j]}");*/

                        if (boardState[i, j] == boardState[i - k, j])
                        {
                            //Debug.Log("Same Coin Found! winCondition++");
                            winCondition++;
                        }

                        if (winCondition != R) continue;
                        Debug.Log($"Vertical winner found - Player {boardState[i,j]}");
                        return true;
                    }
                }
            }
        }
        //Debug.Log($"Winner NOT found Vertically");
        return false;
    }

    private bool HorizontalWinCheck()
    {
        var M = GameManager.instance.GetM;
        var R = GameManager.instance.GetR;
     
        // Check row and column for a placed coin and then check upcoming coins
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < R; j++)
            {
                if (boardState[i, j] > 0)
                {
                    var winCondition = 1;
                    for (int k = 0; k < R - 1; k++)
                    {
                        /*Debug.Log($"i: {i} j: {j} k: {k}");
                        Debug.Log("BoardState compare Horizontal "+boardState[i,j]+" "+boardState[i,j+k+1]);*/
                        if (boardState[i, j] == boardState[i, j + k + 1])
                        {
                            winCondition++;
                        }
                    }
                    if (winCondition == R)
                    {
                        Debug.Log($"Winner found horizontally - Player {boardState[i,j]}");
                        return true;
                    }
                    /*if (boardState[i, j] == boardState[i, j + 1] && boardState[i,j+1] == boardState[i, j + 2] && boardState[i,j+2] == boardState[i, j + 3])
                    {
                        Debug.Log($"Winner found horizontally - Player {boardState[i,j]}");
                        return true;
                    }*/
                }
            }
        }
        //Debug.Log($"Winner NOT found horizontally");
        return false;
    }

    private bool DiagonalWinCheck()
    {
        var N = GameManager.instance.GetN;
        var M = GameManager.instance.GetM;
        var R = GameManager.instance.GetR;

        for (int i = M - 1; i >= R - 1; i--)
        {
            for (int j = 0; j < N - R; j++)
            {
                if (boardState[i, j] > 0)
                {
                    var winCondition = 1;
                    for (int k = 1; k < R; k++)
                    {
                        if (boardState[i - k, j + k] == boardState[i, j])
                        {
                            winCondition++;
                        }
                    }

                    if (winCondition == R)
                    {
                        Debug.Log($"Winner found Diagonally - Player {boardState[i,j]}");
                        return true;
                    }
                }
            }
        }
        
        for (int i = 0; i <= M - R; i++)
        {
            for (int j = 0; j < N - R; j++)
            {
                if (boardState[i, j] > 0)
                {
                    var winCondition = 1;
                    for (int k = 1; k < R; k++)
                    {
                        if (boardState[i + k, j + k] == boardState[i, j])
                        {
                            winCondition++;
                        }
                    }

                    if (winCondition == R)
                    {
                        Debug.Log($"Winner found Diagonally - Player {boardState[i,j]}");
                        return true;
                    }
                }
            }
        }
        for (int i = 0; i <= M - R; i++)
        {
            for (int j = N - 1; j >= N - R; j--)
            {
                if (boardState[i, j] > 0)
                {
                    var winCondition = 1;
                    for (int k = 1; k < R; k++)
                    {
                        if (boardState[i + k, j - k] == boardState[i, j])
                        {
                            winCondition++;
                        }
                    }

                    if (winCondition == R)
                    {
                        Debug.Log($"Winner found Diagonally - Player {boardState[i,j]}");
                        return true;
                    }
                }
            }
        }
        for (int i = M - 1; i > M - R; i--)
        {
            for (int j = N - 1; j >= N - R; j--)
            {
                if (boardState[i, j] > 0)
                {
                    var winCondition = 1;
                    for (int k = 1; k < R; k++)
                    {
                        if (boardState[i - k, j - k] == boardState[i, j])
                        {
                            winCondition++;
                        }
                    }

                    if (winCondition == R)
                    {
                        Debug.Log($"Winner found Diagonally - Player {boardState[i,j]}");
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
