using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public TMP_InputField n;
    public TMP_InputField m;
    public TMP_InputField r;
    public void InitiateStartingGame()
    {
        if (n.text == "" || m.text == "" || r.text == "") return;
        
        GameManager.instance.Init(Int32.Parse(n.text), Int32.Parse(m.text), Int32.Parse(r.text));
    }
}
