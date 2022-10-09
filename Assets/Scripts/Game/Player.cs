using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private bool isAi;
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
        return Random.Range(0, GameManager.instance.GetXLength);
        
        
    }

    // calculate utility of state
    private float PerformMinimax(int depth)
    {
        
        return float.MinValue;
    }
    
    // 
    public void SetPlayerToAi(bool isAi)
    {
        this.isAi = isAi;
    }
}
