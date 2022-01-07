/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public Column[] columns;

    public Cell[] CellColumns;




    public List<GameObject> candies = new List<GameObject>();

    public void Start()
    {
        CellColumns = new Cell[columns.Length];
        for (int i = 0; i < CellColumns.Length; i++)
        {
            CellColumns[i] = new Cell();

            CellColumns[i].cellRows = new Transform[columns[i].rows.Length];


        }



        for (int i = 0; i < columns.Length; i++)//column
        {
            for (int j = 0; j < columns[i].rows.Length; j++)//rows
            {

                var go = Instantiate(candies[Random.Range(0, candies.Count)], 
                    columns[i].rows[j], Quaternion.identity);

                CellColumns[i].cellRows[j] = go.transform;


            }
        }   
    }




    
}

[System.Serializable]
public class Column
{
    public Vector3[] rows;
}




[System.Serializable]
public class Cell
{
    public Transform[] cellRows;
}


*/