using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectColumnFromSpot : MonoBehaviour
{
    private int columnIndex;

    private void Start()
    {
        // Parse the column index from the name of the columns
        int.TryParse(gameObject.name.Split(',')[0], out columnIndex);
        //Debug.Log("column index: "+columnIndex);
        
    }

    // Called every frame while the mouse is over the Collider
    private void OnMouseOver()
    {
        // 0 is for the Left Mouse button
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.HandleColumnClick(columnIndex);
        }
    }
}
