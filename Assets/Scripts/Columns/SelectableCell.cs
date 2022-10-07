using System;
using UnityEngine;

public class SelectableCell : MonoBehaviour
{
    [SerializeField]
    private GameObject player1Coin, player2Coin;
    
    private int columnIndex;
    private int rowIndex;

    public void Init(int column, int row)
    {
        columnIndex = column;
        rowIndex = row;
    }

    public GameObject GetPlayerGameObject(Player player)
    {
        switch (player)
        {
            case Player.A:
                return player1Coin;
            case Player.B:
                return player2Coin;
            default:
                throw new ArgumentOutOfRangeException(nameof(player), player, null);
        }
    }

    // called when the user has pressed the mouse button while over the Collider
    private void OnMouseDown()
    {
        GameManager.instance.HandleColumnClick(columnIndex);
    }
}
