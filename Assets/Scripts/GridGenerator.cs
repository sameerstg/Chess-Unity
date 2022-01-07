/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    [SerializeField] LevelSystem map;

    //[SerializeField] GameObject square;

    [SerializeField] int columns;
    [SerializeField] int rows;

    [SerializeField] float scaleUnit;
    [SerializeField] float spaceUnit;



    [ContextMenu("Generate Grid")]
    public void CustomMethod()
    {
        map.columns = new Column[columns];

        for (int i = 0; i < map.columns.Length; i++)
        {
            map.columns[i] = new Column();

            map.columns[i].rows = new Vector3[rows];
        }

        var x = columns / 2;
        Vector2 pos = Vector2.zero;
        pos.x = -((x * scaleUnit) + ((x - 1) * spaceUnit));

        var y = rows / 2;
        pos.y = (y * scaleUnit) + ((y - 1) * spaceUnit);

        Vector3 savedPos = pos;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {

                map.columns[j].rows[i] = pos;

                pos.x += (spaceUnit + scaleUnit);
            }
            pos.y -= (spaceUnit + scaleUnit);
            pos.x = savedPos.x;
        }


        //map.Assigning();
    }


}
*/