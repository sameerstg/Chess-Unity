using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public static BoardGenerator _instance;
    public GameObject bTile, wTile;
    bool matColor = false;
    public Cell cell;
    public Cell[] CellColumns  ;
    Vector3 firstPos;
    public enum coordinates
    {
        A,B,C,D,E,F,G,H
    }
    private void Awake()
    {
        _instance = this;

    }
    void Start()
    {
        
            CellColumns = new Cell[8];

        for (int i = 0; i < 8; i++)
        {
            CellColumns[i] = new Cell();

        }
        GenerateAllTiles();
        PieceManager._instance.InitializePieceManager();
        /*        StartCoroutine(Delay());
        */
    }


    void GenerateAllTiles()
    {

        firstPos = bTile.transform.position;
        firstPos.y += 1;
        for (int i = 0; i < 8; i++)
        {


            firstPos.x = -8;
            firstPos.y -= 1;
            if (matColor)
            {
                matColor = false;
            }
            else if (!matColor)
            {
                matColor = true;
            }
            for (int j = 0; j < 8; j++)
            {

                firstPos.x += 1;
                var justMadeTile = Instantiate(bTile, firstPos, Quaternion.identity);
                justMadeTile.transform.parent = transform;
                justMadeTile.name = string.Format("(" + ((coordinates)(i ))+ (j+1).ToString() + ")");
                CellColumns[i].cellRows[j] = justMadeTile;

                if (matColor)
                {
                    matColor = false;
                    justMadeTile.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if (!matColor)
                {
                    justMadeTile.GetComponent<MeshRenderer>().material.color = Color.white;

                    matColor = true;
                }
            }

        }
        
    }
    [ContextMenu("Generate Grid")]
    public void Gernerate()
    {
        StartCoroutine(Delay());
    }
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        GenerateAllTiles();
    }
}
[System.Serializable]
public class Cell
{
    public Cell()
    {

    }
    public GameObject[] cellRows = new GameObject[8];

}