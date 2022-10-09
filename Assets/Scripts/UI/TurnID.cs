using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnID : MonoBehaviour
{
    [SerializeField] private TMP_Text turnInfo;

    public void UpdateTurnIDText(PlayerType player)
    {
        turnInfo.text = player == PlayerType.A ? "Turn: Player 1" : "Turn: Player 2";
    }
}