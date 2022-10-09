using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public Toggle player1Toggle;
    public Toggle player2Toggle;
    public TMP_InputField n;
    public TMP_InputField m;
    public TMP_InputField r;
    public void InitiateStartingGame()
    {
        if (n.text == "" || m.text == "" || r.text == "") return;
        
        player1.SetPlayerToAi(player1Toggle.isOn);
        player2.SetPlayerToAi(player2Toggle.isOn);
        
        GameManager.instance.Init(Int32.Parse(n.text), Int32.Parse(m.text), Int32.Parse(r.text));
    }
}
