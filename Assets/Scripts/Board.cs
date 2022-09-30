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

    public void DropCoinAtPosition(int x, int y, int playerCoin)
    {
        boardState[x, y] = playerCoin;
        Debug.Log(DebugBoard());
        // Win Check
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

    private void CheckForWin()
    {
        
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
                    
                }
            }
        }
        
        return false;
    }

    private bool VerticalWinCheck()
    {
        
        return false;
    }

    private bool DiagonalWinCheck()
    {
        
        return false;
    }
}
