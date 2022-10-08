using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnID : MonoBehaviour
{
    [SerializeField] private TMP_Text turnInfo;

    public void UpdateTurnIDText(Player player)
    {
        turnInfo.text = player == Player.A ? "Turn: Player 1" : "Turn: Player 2";
    }
}