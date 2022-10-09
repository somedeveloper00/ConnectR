using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckStartUpInput : MonoBehaviour
{
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject nObject;
    [SerializeField] private GameObject mObject;
    [SerializeField] private GameObject rObject;

    private void Update()
    {
        if(nObject.GetComponent<TMP_InputField>().text.Length > 0 && mObject.GetComponent<TMP_InputField>().text.Length > 0 && rObject.GetComponent<TMP_InputField>().text.Length > 0) EnableConfirmButton();
    }

    private void EnableConfirmButton()
    {
        confirmButton.SetActive(true);
        
        Destroy(this);
    }
    
    
}
