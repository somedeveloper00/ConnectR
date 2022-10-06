using System;
using UnityEngine;

public class SelectableCell : MonoBehaviour
{
    private int columnIndex;
    private int rowIndex;

    public void Init(int column, int row)
    {
        columnIndex = column;
        rowIndex = row;
    }
    
    private void Start()
    {
        // Parse the column index from the name of the columns
        int.TryParse(gameObject.name.Split(',')[1], out columnIndex);
        //Debug.Log("column index: "+columnIndex);
        
    }

    // called when the user has pressed the mouse button while over the Collider
    private void OnMouseDown()
    {
        GameManager.instance.HandleColumnClick(columnIndex);
    }
}
