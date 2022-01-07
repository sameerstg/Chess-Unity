using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{

     public static PieceManager _instance;
    public bool freePlay;
 
    //      Piece Prefab List
    public List<GameObject> whitePiecesList;
     public List<GameObject> blackPiecesList;

    public DCell[] dCellColumns;
     public PCell[] pCellColumns;
    //      List of White Pieces
    public List<GameObject> whitePieces;
    //      List of Black Pieces
    public List<GameObject> blackPieces;
    //      Count Of Pieces
    public int bPiecesCount;
    public int wPiecesCount;

    //      Selected Piece
    public GameObject SelectedPiece;
    //      Dot To be Instantiated
    public GameObject dot;
    //      Instatiated Dots list
    public List<GameObject> allDots;



    //      Turn Decision
    //      If(true)Whites Turn;else black turns
    public bool turn;

    public bool check;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            pCellColumns = new PCell[i];
            dCellColumns = new DCell[i];
        }
        whitePieces = new List<GameObject>();
        blackPieces = new List<GameObject>();
        bPiecesCount = 0; wPiecesCount = 0;

        dot = Resources.Load<GameObject>("Dot");
        allDots = new List<GameObject>();

        turn = true;
        check = false;
    }
    
    //     Generating All Pieces

    public void Set()
    {

        //      Generating White Pieces
        var k = 0;
        for (int i = 7; i > 5; i--)
        {
            for (int j = 7; j > -1; j--)
            {
                var l = Instantiate(whitePiecesList[k], BoardGenerator._instance.CellColumns[i].cellRows[j].transform.position, Quaternion.identity);
                SetPieceDimension(l);
                pCellColumns[i].pcellRows[j] = l.gameObject;
                k++;
            }
        }
        //      Generating Black Pieces
        k = 0;
        for (int m = 1; m > -1; m--)
        {
            for (int n = 7; n > -1; n--)
            {
                var o = Instantiate(blackPiecesList[k], BoardGenerator._instance.CellColumns[m].cellRows[n].transform.position, Quaternion.identity);
                SetPieceDimension(o);
                pCellColumns[m].pcellRows[n] = o.gameObject;
                k++;
            }
        }
        gameObject.transform.position = Vector3.forward * -1; ;

    }
    public void SetPieceDimension(GameObject whatToRotate)
    {
        whatToRotate.transform.Rotate(Vector3.up * 90);
        whatToRotate.transform.parent = transform;
    }
    public void GetMove(GameObject PieceGameObject,String pieceName)
    {
        Deselect();
        SelectedPiece = PieceGameObject;
        GetPiecePos(PieceGameObject, pieceName);

    }
    public void GetPiecePos(GameObject obj,String nameOfPiece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pCellColumns[i].pcellRows[j] == null)
                {
                    continue;
                }
                else if (pCellColumns[i].pcellRows[j].gameObject == obj)
                {
                    ShowAllMoves(obj, i, j, nameOfPiece);
                }
            }
        }
    }

    public void ShowAllMoves(GameObject piece,int xAxisPos,int yAxisPos,String pieceName)
    {
        if (piece.name == "WPawn (Clone)")
        {
            if (pCellColumns[xAxisPos - 1].pcellRows[yAxisPos] == null)
            {
                GenerateDot(xAxisPos - 1, yAxisPos);
            }
            if (!piece.GetComponent<Piece>().firstMove)
            {
                if (pCellColumns[xAxisPos - 2].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos - 2, yAxisPos);
                }

            }

            //      Checking Taking Position
            for (int i = -1; i < 2; i += 2)
            {
                if (yAxisPos + i<7 && yAxisPos + i>-1 )
                {
                    if (pCellColumns[xAxisPos - 1].pcellRows[yAxisPos + i] != null)
                    {
                        GenerateDot(xAxisPos - 1, yAxisPos + i);
                    }

                }
            }
        }
        else if (piece.name == "BPawn(Clone)")
        {
            if (pCellColumns[xAxisPos + 1].pcellRows[yAxisPos] == null)
            {
                GenerateDot(xAxisPos + 1, yAxisPos);
            }
            if (!piece.GetComponent<Piece>().firstMove)
            {
                if (pCellColumns[xAxisPos + 2].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos + 2, yAxisPos);

                }
            }
            //      Checking Taking Position
            for (int i = -1; i < 2; i += 2)
            {
                if (yAxisPos + i < 7 && yAxisPos + i > -1)
                {
                    if (pCellColumns[xAxisPos + 1].pcellRows[yAxisPos + i] != null && pCellColumns[xAxisPos + 1].pcellRows[yAxisPos + i].gameObject.CompareTag("White"))
                    {
                        GenerateDot(xAxisPos + 1, yAxisPos + i);
                    }

                }
            }

        }
        else if (piece.name == "BKnight(Clone)")
        {
            var l = -2;
            for (int i = 2; i > -3; i -= 1)
            {
                if (i == 0)
                {
                    continue;
                }

                for (int j = 1; j > -2; j -= 2)
                {
                    if (xAxisPos + i >= 0 && xAxisPos + i <= 7 && yAxisPos + j >= 0 && yAxisPos + j <= 7 && i != 1 && i != -1)
                    {
                        GenerateDot(xAxisPos + i, yAxisPos + j);


                    }
                    if (i == 1 || i == -1)
                    {
                        if (xAxisPos + i >= 0 && xAxisPos + i <= 7 && yAxisPos + l >= 0 && yAxisPos + l <= 7)
                        {
                        GenerateDot(xAxisPos + i, yAxisPos + l);
                           
                        }
                        l = -2;
                    }


                }
            }

        }
        else if (piece.name == "WKnight (Clone)")
        {
            var l = -2;
            for (int i = 2; i > -3; i -= 1)
            {
                if (i == 0)
                {
                    continue;
                }

                for (int j = 1; j > -2; j -= 2)
                {
                    if (xAxisPos + i >= 0 && xAxisPos + i <= 7 && yAxisPos + j >= 0 && yAxisPos + j <= 7 && i != 1 && i != -1)
                    {
                        GenerateDot(xAxisPos + i, yAxisPos + j);


                    }
                    if (i == 1 || i == -1)
                    {
                        if (xAxisPos + i >= 0 && xAxisPos + i <= 7 && yAxisPos + l >= 0 && yAxisPos + l <= 7)
                        {
                            GenerateDot(xAxisPos + i, yAxisPos + l);

                        }
                        l = -2;
                    }


                }
            }

        }
        else if (piece.name == "WBishop (Clone)")
        {
            var pieceXPos1 = xAxisPos;
            var pieceYPos1 = yAxisPos;
            var pieceXPos2 = xAxisPos;
            var pieceYPos2 = yAxisPos;
            var pieceXPos3 = xAxisPos;
            var pieceYPos3 = yAxisPos;
            var pieceXPos4 = xAxisPos;
            var pieceYPos4 = yAxisPos;

            for (int i = 0; i < 8; i++)
            {
                //      For Diagonal Left Upward
                if (xAxisPos == 0 || yAxisPos == 0)
                {
                    break;
                }
                pieceXPos1 += 7;
                pieceYPos1 -= 1;
                if (pieceXPos1 > 7)
                {
                    pieceXPos1 -= 8;
                }
                if (pieceXPos1 > -1 && pieceXPos1 < 8 && pieceYPos1 > -1 && pieceYPos1 < 8)
                {
                    if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] == null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        if (pieceXPos1 == 0 || pieceYPos1 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] != null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        break;
                    }


                }
            }
            for (int j = 0; j < 8; j++)
            {
                //      For Diagonal Right Downward
                if (xAxisPos == 7 || yAxisPos == 7)
                {
                    break;
                }
                pieceXPos2 -= 7;
                pieceYPos2 += 1;
                if (pieceXPos2 < 0)
                {
                    pieceXPos2 += 8;
                }
                if (pieceXPos2 > -1 && pieceXPos2 < 8 && pieceYPos2 > -1 && pieceYPos2 < 8)
                {
                    if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] == null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        if (pieceXPos2 == 7 || pieceYPos2 == 7)
                        {
                            break;
                        }

                    }
                    else if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] != null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        break;
                    }


                }

            }
            for (int k = 0; k < 8; k++)
            {
                //      For Diagonal Right Upwards
                if (xAxisPos == 0 || yAxisPos == 7)
                {
                    break;
                }

                pieceXPos3 -= 1;
                pieceYPos3 -= 7;
                if (pieceYPos3 < 0)
                {
                    pieceYPos3 += 8;
                }
                else if (pieceYPos3 == 0)
                {
                    break;
                }
                if (pieceXPos3 > -1 && pieceXPos3 < 8 && pieceYPos3 > -1 && pieceYPos3 < 8)
                {
                    if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] == null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        if (pieceXPos3 == 0 || pieceYPos3 == 7)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] != null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        break;
                    }
                }

            }
            for (int l = 0; l < 8; l++)
            {
                //      For Diagonal Left Downwards
                if (xAxisPos == 7 || yAxisPos == 0)
                {
                    break;
                }

                pieceXPos4 += 1;
                pieceYPos4 += 7;
                if (pieceYPos4 > 7)
                {
                    pieceYPos4 -= 8;
                }
                if (pieceXPos4 > -1 && pieceXPos4 < 8 && pieceYPos4 > -1 && pieceYPos4 < 8)
                {
                    if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] == null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        if (pieceXPos4 == 7 || pieceYPos4 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] != null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        break;
                    }

                }


            }

        }
        else if (piece.name == "BBishop(Clone)")
        {
            var pieceXPos1 = xAxisPos;
            var pieceYPos1 = yAxisPos;
            var pieceXPos2 = xAxisPos;
            var pieceYPos2 = yAxisPos;
            var pieceXPos3 = xAxisPos;
            var pieceYPos3 = yAxisPos;
            var pieceXPos4 = xAxisPos;
            var pieceYPos4 = yAxisPos;

            for (int i = 0; i < 8; i++)
            {
                //      For Diagonal Left Upward
                if (xAxisPos == 0 || yAxisPos == 0)
                {
                    break;
                }
                pieceXPos1 += 7;
                pieceYPos1 -= 1;
                if (pieceXPos1 > 7)
                {
                    pieceXPos1 -= 8;
                }
                if (pieceXPos1 > -1 && pieceXPos1 < 8 && pieceYPos1 > -1 && pieceYPos1 < 8)
                {
                    if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] == null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        if (pieceXPos1 == 0 || pieceYPos1 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] != null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        break;
                    }


                }
            }
            for (int j = 0; j < 8; j++)
            {
                //      For Diagonal Right Downward
                if (xAxisPos == 7 || yAxisPos == 7)
                {
                    break;
                }
                pieceXPos2 -= 7;
                pieceYPos2 += 1;
                if (pieceXPos2 < 0)
                {
                    pieceXPos2 += 8;
                }
                if (pieceXPos2 > -1 && pieceXPos2 < 8 && pieceYPos2 > -1 && pieceYPos2 < 8)
                {
                    if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] == null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        if (pieceXPos2 == 7 || pieceYPos2 == 7)
                        {
                            break;
                        }

                    }
                    else if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] != null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        break;
                    }


                }

            }
            for (int k = 0; k < 8; k++)
            {
                //      For Diagonal Right Upwards
                if (xAxisPos == 0 || yAxisPos == 7)
                {
                    break;
                }

                pieceXPos3 -= 1;
                pieceYPos3 -= 7;
                if (pieceYPos3 < 0)
                {
                    pieceYPos3 += 8;
                }
                else if (pieceYPos3 == 0)
                {
                    break;
                }
                if (pieceXPos3 > -1 && pieceXPos3 < 8 && pieceYPos3 > -1 && pieceYPos3 < 8)
                {
                    if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] == null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        if (pieceXPos3 == 0 || pieceYPos3 == 7)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] != null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        break;
                    }
                }

            }
            for (int l = 0; l < 8; l++)
            {
                //      For Diagonal Left Downwards
                if (xAxisPos == 7 || yAxisPos == 0)
                {
                    break;
                }

                pieceXPos4 += 1;
                pieceYPos4 += 7;
                if (pieceYPos4 > 7)
                {
                    pieceYPos4 -= 8;
                }
                if (pieceXPos4 > -1 && pieceXPos4 < 8 && pieceYPos4 > -1 && pieceYPos4 < 8)
                {
                    if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] == null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        if (pieceXPos4 == 7 || pieceYPos4 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] != null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        break;
                    }

                }


            }

        }
        else if (piece.name == "BRook(Clone)")
        {
            var xAxisPos1 = xAxisPos;
            var xAxisPos2 = xAxisPos;
            var yAxisPos1 = yAxisPos;
            var yAxisPos2 = yAxisPos;
            //      Upward Movement
            for (int i = 0; i < 8; i++)
            {

                xAxisPos1 -= 1;
                if (xAxisPos1 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos1].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos1, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos1].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos1, yAxisPos);
                }
            }

            //      Downward Movement
            for (int j = 0; j < 8; j++)
            {
                xAxisPos2 += 1;
                if (xAxisPos2 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos2].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos2, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos2].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos2, yAxisPos);
                }
            }
            //      Left Movement
            for (int k = 0; k < 8; k++)
            {
                yAxisPos1 -= 1;
                if (yAxisPos1 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos1] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos1);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos1] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos1);
                }
            }
            //      Right Movement
            for (int l = 0; l < 8; l++)
            {
                yAxisPos2 += 1;
                if (yAxisPos2 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos2] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos2);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos2] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos2);
                }
            }
        }
        else if (piece.name == "WRook (Clone)")
        {
            var xAxisPos1 = xAxisPos;
            var xAxisPos2 = xAxisPos;
            var yAxisPos1 = yAxisPos;
            var yAxisPos2 = yAxisPos;
            //      Upward Movement
            for (int i = 0; i < 8; i++)
            {

                xAxisPos1 -= 1;
                if (xAxisPos1 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos1].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos1, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos1].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos1, yAxisPos);
                }
            }

            //      Downward Movement
            for (int j = 0; j < 8; j++)
            {
                xAxisPos2 += 1;
                if (xAxisPos2 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos2].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos2, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos2].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos2, yAxisPos);
                }
            }
            //      Left Movement
            for (int k = 0; k < 8; k++)
            {
                yAxisPos1 -= 1;
                if (yAxisPos1<0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos1] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos1);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos1] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos1);
                }
            }
            //      Right Movement
            for (int l = 0; l < 8; l++)
            {
                yAxisPos2 += 1;
                if (yAxisPos2 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos2] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos2);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos2] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos2);
                }
            }
        }
        else if (piece.name == "WQueen (Clone)")
        {
            var pieceXPos1 = xAxisPos;
            var pieceYPos1 = yAxisPos;
            var pieceXPos2 = xAxisPos;
            var pieceYPos2 = yAxisPos;
            var pieceXPos3 = xAxisPos;
            var pieceYPos3 = yAxisPos;
            var pieceXPos4 = xAxisPos;
            var pieceYPos4 = yAxisPos;

            for (int i = 0; i < 8; i++)
            {
                //      For Diagonal Left Upward
                if (xAxisPos == 0 || yAxisPos == 0)
                {
                    break;
                }
                pieceXPos1 += 7;
                pieceYPos1 -= 1;
                if (pieceXPos1 > 7)
                {
                    pieceXPos1 -= 8;
                }
                if (pieceXPos1 > -1 && pieceXPos1 < 8 && pieceYPos1 > -1 && pieceYPos1 < 8)
                {
                    if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] == null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        if (pieceXPos1 == 0 || pieceYPos1 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] != null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        break;
                    }


                }
            }
            for (int j = 0; j < 8; j++)
            {
                //      For Diagonal Right Downward
                if (xAxisPos == 7 || yAxisPos == 7)
                {
                    break;
                }
                pieceXPos2 -= 7;
                pieceYPos2 += 1;
                if (pieceXPos2 < 0)
                {
                    pieceXPos2 += 8;
                }
                if (pieceXPos2 > -1 && pieceXPos2 < 8 && pieceYPos2 > -1 && pieceYPos2 < 8)
                {
                    if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] == null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        if (pieceXPos2 == 7 || pieceYPos2 == 7)
                        {
                            break;
                        }

                    }
                    else if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] != null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        break;
                    }


                }

            }
            for (int k = 0; k < 8; k++)
            {
                //      For Diagonal Right Upwards
                if (xAxisPos == 0 || yAxisPos == 7)
                {
                    break;
                }

                pieceXPos3 -= 1;
                pieceYPos3 -= 7;
                if (pieceYPos3 < 0)
                {
                    pieceYPos3 += 8;
                }
                else if (pieceYPos3 == 0)
                {
                    break;
                }
                if (pieceXPos3 > -1 && pieceXPos3 < 8 && pieceYPos3 > -1 && pieceYPos3 < 8)
                {
                    if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] == null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        if (pieceXPos3 == 0 || pieceYPos3 == 7)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] != null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        break;
                    }
                }

            }
            for (int l = 0; l < 8; l++)
            {
                //      For Diagonal Left Downwards
                if (xAxisPos == 7 || yAxisPos == 0)
                {
                    break;
                }

                pieceXPos4 += 1;
                pieceYPos4 += 7;
                if (pieceYPos4 > 7)
                {
                    pieceYPos4 -= 8;
                }
                if (pieceXPos4 > -1 && pieceXPos4 < 8 && pieceYPos4 > -1 && pieceYPos4 < 8)
                {
                    if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] == null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        if (pieceXPos4 == 7 || pieceYPos4 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] != null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        break;
                    }
                }
            }
            var xAxisPos5 = xAxisPos;
            var xAxisPos6 = xAxisPos;
            var yAxisPos5 = yAxisPos;
            var yAxisPos6 = yAxisPos;
            //      Upward Movement
            for (int m = 0; m < 8; m++)
            {

                xAxisPos5 -= 1;
                if (xAxisPos5 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos5].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos5, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos5].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos5, yAxisPos);
                }
            }

            //      Downward Movement
            for (int n = 0; n < 8; n++)
            {
                xAxisPos6 += 1;
                if (xAxisPos6 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos6].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos6, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos6].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos6, yAxisPos);
                }
            }
            //      Left Movement
            for (int k = 0; k < 8; k++)
            {
                yAxisPos5 -= 1;
                if (yAxisPos5 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos5] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos5);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos5] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos5);
                }
            }
            //      Right Movement
            for (int l = 0; l < 8; l++)
            {
                yAxisPos6 += 1;
                if (yAxisPos6 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos6] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos6);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos6] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos6);
                }
            }


        }
        else if (piece.name == "BQueen(Clone)")
        {
            var pieceXPos1 = xAxisPos;
            var pieceYPos1 = yAxisPos;
            var pieceXPos2 = xAxisPos;
            var pieceYPos2 = yAxisPos;
            var pieceXPos3 = xAxisPos;
            var pieceYPos3 = yAxisPos;
            var pieceXPos4 = xAxisPos;
            var pieceYPos4 = yAxisPos;

            for (int i = 0; i < 8; i++)
            {
                //      For Diagonal Left Upward
                if (xAxisPos == 0 || yAxisPos == 0)
                {
                    break;
                }
                pieceXPos1 += 7;
                pieceYPos1 -= 1;
                if (pieceXPos1 > 7)
                {
                    pieceXPos1 -= 8;
                }
                if (pieceXPos1 > -1 && pieceXPos1 < 8 && pieceYPos1 > -1 && pieceYPos1 < 8)
                {
                    if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] == null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        if (pieceXPos1 == 0 || pieceYPos1 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos1].pcellRows[pieceYPos1] != null)
                    {
                        GenerateDot(pieceXPos1, pieceYPos1);
                        break;
                    }


                }
            }
            for (int j = 0; j < 8; j++)
            {
                //      For Diagonal Right Downward
                if (xAxisPos == 7 || yAxisPos == 7)
                {
                    break;
                }
                pieceXPos2 -= 7;
                pieceYPos2 += 1;
                if (pieceXPos2 < 0)
                {
                    pieceXPos2 += 8;
                }
                if (pieceXPos2 > -1 && pieceXPos2 < 8 && pieceYPos2 > -1 && pieceYPos2 < 8)
                {
                    if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] == null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        if (pieceXPos2 == 7 || pieceYPos2 == 7)
                        {
                            break;
                        }

                    }
                    else if (pCellColumns[pieceXPos2].pcellRows[pieceYPos2] != null)
                    {
                        GenerateDot(pieceXPos2, pieceYPos2);
                        break;
                    }


                }

            }
            for (int k = 0; k < 8; k++)
            {
                //      For Diagonal Right Upwards
                if (xAxisPos == 0 || yAxisPos == 7)
                {
                    break;
                }

                pieceXPos3 -= 1;
                pieceYPos3 -= 7;
                if (pieceYPos3 < 0)
                {
                    pieceYPos3 += 8;
                }
                else if (pieceYPos3 == 0)
                {
                    break;
                }
                if (pieceXPos3 > -1 && pieceXPos3 < 8 && pieceYPos3 > -1 && pieceYPos3 < 8)
                {
                    if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] == null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        if (pieceXPos3 == 0 || pieceYPos3 == 7)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos3].pcellRows[pieceYPos3] != null)
                    {
                        GenerateDot(pieceXPos3, pieceYPos3);
                        break;
                    }
                }

            }
            for (int l = 0; l < 8; l++)
            {
                //      For Diagonal Left Downwards
                if (xAxisPos == 7 || yAxisPos == 0)
                {
                    break;
                }

                pieceXPos4 += 1;
                pieceYPos4 += 7;
                if (pieceYPos4 > 7)
                {
                    pieceYPos4 -= 8;
                }
                if (pieceXPos4 > -1 && pieceXPos4 < 8 && pieceYPos4 > -1 && pieceYPos4 < 8)
                {
                    if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] == null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        if (pieceXPos4 == 7 || pieceYPos4 == 0)
                        {
                            break;
                        }
                    }
                    else if (pCellColumns[pieceXPos4].pcellRows[pieceYPos4] != null)
                    {
                        GenerateDot(pieceXPos4, pieceYPos4);
                        break;
                    }
                }
            }
            var xAxisPos5 = xAxisPos;
            var xAxisPos6 = xAxisPos;
            var yAxisPos5 = yAxisPos;
            var yAxisPos6 = yAxisPos;
            //      Upward Movement
            for (int m = 0; m < 8; m++)
            {

                xAxisPos5 -= 1;
                if (xAxisPos5 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos5].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos5, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos5].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos5, yAxisPos);
                }
            }

            //      Downward Movement
            for (int n = 0; n < 8; n++)
            {
                xAxisPos6 += 1;
                if (xAxisPos6 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos6].pcellRows[yAxisPos] != null)
                {
                    GenerateDot(xAxisPos6, yAxisPos);
                    break;
                }
                else if (pCellColumns[xAxisPos6].pcellRows[yAxisPos] == null)
                {
                    GenerateDot(xAxisPos6, yAxisPos);
                }
            }
            //      Left Movement
            for (int k = 0; k < 8; k++)
            {
                yAxisPos5 -= 1;
                if (yAxisPos5 < 0)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos5] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos5);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos5] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos5);
                }
            }
            //      Right Movement
            for (int l = 0; l < 8; l++)
            {
                yAxisPos6 += 1;
                if (yAxisPos6 > 7)
                {
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos6] != null)
                {
                    GenerateDot(xAxisPos, yAxisPos6);
                    break;
                }
                else if (pCellColumns[xAxisPos].pcellRows[yAxisPos6] == null)
                {
                    GenerateDot(xAxisPos, yAxisPos6);
                }
            }

        }
        else if (piece.name == "BKing(Clone)")
        {
            GenerateDot(xAxisPos - 1, yAxisPos);
            GenerateDot(xAxisPos + 1, yAxisPos);
            GenerateDot(xAxisPos, yAxisPos + 1);
            GenerateDot(xAxisPos, yAxisPos - 1);
            GenerateDot(xAxisPos + 1, yAxisPos + 1);
            GenerateDot(xAxisPos - 1, yAxisPos - 1);
            GenerateDot(xAxisPos - 1, yAxisPos + 1);
            GenerateDot(xAxisPos + 1, yAxisPos - 1);
        }
        else if (piece.name == "WKing(Clone)")
        {
            GenerateDot(xAxisPos - 1, yAxisPos);
            GenerateDot(xAxisPos + 1, yAxisPos);
            GenerateDot(xAxisPos, yAxisPos + 1);
            GenerateDot(xAxisPos, yAxisPos - 1);
            GenerateDot(xAxisPos+1, yAxisPos + 1);
            GenerateDot(xAxisPos-1, yAxisPos - 1);
            GenerateDot(xAxisPos-1, yAxisPos + 1);
            GenerateDot(xAxisPos+1, yAxisPos - 1);
        }

    }
    public void Move(GameObject dot)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (dCellColumns[i].dcellRows[j] == dot)
                {
                    EliminatingPieceFromArray(SelectedPiece);
                    if (pCellColumns[i].pcellRows[j] != null)
                    {
                        Take(pCellColumns[i].pcellRows[j], i, j);
                    }
                    var whereToSend = BoardGenerator._instance.CellColumns[i].cellRows[j].transform.position;
                    whereToSend.z -= 2;
                    SelectedPiece.transform.position = whereToSend;
                    pCellColumns[i].pcellRows[j] = SelectedPiece;
                    SelectedPiece.GetComponent<Piece>().DoneFirstMove();
                }
            }
        }
        
        Deselect();
    }
    void Generating()
    {
        /*         wr2 = Instantiate(whiteRook, BoardGenerator._instance.CellColumns[8 - 1].cellRows[8-1].transform.position, Quaternion.identity);
        SetPieceDimension(wr2);
        wkn2 = Instantiate(whiteKnight, BoardGenerator._instance.CellColumns[8 - 1].cellRows[7 - 1].transform.position, Quaternion.identity);
        SetPieceDimension(wkn2);
        wb2 = Instantiate(whiteBishop, BoardGenerator._instance.CellColumns[8 - 1].cellRows[6 - 1].transform.position, Quaternion.identity);
        SetPieceDimension(wb2);
         wk = Instantiate(whiteKing, BoardGenerator._instance.CellColumns[8-1].cellRows[5-1].transform.position, Quaternion.identity);
        SetPieceDimension(wk);
         wq = Instantiate(whiteQueen, BoardGenerator._instance.CellColumns[8-1].cellRows[4-1].transform.position, Quaternion.identity);
        SetPieceDimension(wq);
         wb1 = Instantiate(whiteBishop, BoardGenerator._instance.CellColumns[8-1].cellRows[3-1].transform.position, Quaternion.identity);
        SetPieceDimension(wb1);
        wkn1 = Instantiate(whiteKnight, BoardGenerator._instance.CellColumns[8-1].cellRows[2-1].transform.position, Quaternion.identity);
        SetPieceDimension(wkn1);
         wr1 = Instantiate(whiteRook, BoardGenerator._instance.CellColumns[8-1].cellRows[1-1].transform.position, Quaternion.identity);
        SetPieceDimension(wr1);
         wp8 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7-1].cellRows[8-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp8);
         wp7 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[7-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp7);
         wp6 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[6-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp6);
         wp5 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[5-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp5);
         wp4 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[4-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp4);
         wp3 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[3-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp3);
         wp2 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[2-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp2);
        wp1 = Instantiate(whitePawn, BoardGenerator._instance.CellColumns[7 - 1].cellRows[1-1].transform.position, Quaternion.identity);
        SetPieceDimension(wp1);
*/
        /*        bp8 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[8 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp8);
                bp7 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[7 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp7);
                bp6 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[6 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp6);
                bp5 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[5 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp5);
                bp4 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[4 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp4);
                bp3 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[3 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp3);
                bp2 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[2 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp2);
                bp1 = Instantiate(blackPawn, BoardGenerator._instance.CellColumns[2 - 1].cellRows[1 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bp1);
                br2 = Instantiate(blackRook, BoardGenerator._instance.CellColumns[1 - 1].cellRows[8 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(br2);
                bkn2 = Instantiate(blackKnight, BoardGenerator._instance.CellColumns[1 - 1].cellRows[7 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bkn2);
                bb2 = Instantiate(blackBishop, BoardGenerator._instance.CellColumns[1 - 1].cellRows[6 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bb2);
                bq = Instantiate(blackQueen, BoardGenerator._instance.CellColumns[1 - 1].cellRows[5 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bq);
                bk = Instantiate(blackKing, BoardGenerator._instance.CellColumns[1 - 1].cellRows[4 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bk);
                bb1 = Instantiate(blackBishop, BoardGenerator._instance.CellColumns[1 - 1].cellRows[3 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bb1);
                bkn1 = Instantiate(blackKnight, BoardGenerator._instance.CellColumns[1 - 1].cellRows[2 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(bkn1);
                br1 = Instantiate(blackRook, BoardGenerator._instance.CellColumns[1 - 1].cellRows[1 - 1].transform.position, Quaternion.identity);
                SetPieceDimension(br1);
        */

        /*        //      Setting White Pieces in List
                whitePieces.Add(wp1);
                whitePieces.Add(wp2);
                whitePieces.Add(wp3);
                whitePieces.Add(wp4);
                whitePieces.Add(wp5);
                whitePieces.Add(wp8);
                whitePieces.Add(wp6);
                whitePieces.Add(wp7);
                whitePieces.Add(wr1);
                whitePieces.Add(wkn1);
                whitePieces.Add(wb1);
                whitePieces.Add(wq);
                whitePieces.Add(wk);
                whitePieces.Add(wb2);
                whitePieces.Add(wkn2);
                whitePieces.Add(wr2);
                //      Setting Black Pieces in List
                blackPieces.Add(br1);
                blackPieces.Add(bkn1);
                blackPieces.Add(bb1);
                blackPieces.Add(bq);
                blackPieces.Add(bk);
                blackPieces.Add(bb2);
                blackPieces.Add(bkn2);
                blackPieces.Add(br2);
                blackPieces.Add(bp1);
                blackPieces.Add(bp2);
                blackPieces.Add(bp3);
                blackPieces.Add(bp4);
                blackPieces.Add(bp5);
                blackPieces.Add(bp6);
                blackPieces.Add(bp7);
                blackPieces.Add(bp8);
        */
        /*        SettingBlackPieces();
                SettingWhitePieces();
        */

    }
    public void Deselect()
    {
        for (int i = 0; i < allDots.Count; i++)
        {
            Destroy(allDots[i]);
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                dCellColumns[i].dcellRows[j] = null;
            }
        }
        SelectedPiece = null;
    }
    public void EliminatingPieceFromArray(GameObject piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pCellColumns[i].pcellRows[j] != null)
                {
                    if (pCellColumns[i].pcellRows[j] == piece)
                    {

                        pCellColumns[i].pcellRows[j] = null;

                    }
                }
            }
        }
    }
    public void Take(GameObject piece,int x,int y)
    {
        Destroy(piece);
        pCellColumns[x].pcellRows[y] = null;
    }
    public void Check()
    {
        //////////////////////////////////////////          White King Check Check
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pCellColumns[i].pcellRows[j] == null)
                {
                    continue;
                }

                if (pCellColumns[i].pcellRows[j].name == "WKing(Clone)")
                {


                    //      Right Bottom
                    var x_x3 = i;
                    var y_y3 = j;
                    for (int aa3 = 0; aa3 < 8; aa3++)
                    {
                        if (x_x3 > 7 || x_x3 < 0 || y_y3 > 7 || y_y3 < 0)
                        {
                            break;
                        }
                        x_x3 += 1;
                        y_y3 += 1;

                        if (x_x3 > 7 || x_x3 < 0 || y_y3 > 7 || y_y3 < 0)
                        {
                            break;
                        }


                        if (pCellColumns[x_x3].pcellRows[y_y3] == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (pCellColumns[x_x3].pcellRows[y_y3].CompareTag("White"))
                            {
                                break;
                            }
                            else if (pCellColumns[x_x3].pcellRows[y_y3].name == "BQueen(Clone)" || pCellColumns[x_x3].pcellRows[y_y3].name == "BBishop(Clone)")
                            {
                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                            else
                            {
                                check = false;
                                Background._instance.CheckColor();
                                break;
                            }

                        }
                        check = false;
                        Background._instance.CheckColor();
                    }
                    //      Left Bottom
                    var x_x4 = i;
                    var y_y4 = j;
                    for (int aa4 = 0; aa4 < 8; aa4++)
                    {
                        if (x_x4 > 7 || x_x4 < 0 || y_y4 > 7 || y_y4 < 0)
                        {
                            break;
                        }
                        x_x4 += 1;
                        y_y4 -= 1;

                        if (x_x4 > 7 || x_x4 < 0 || y_y4 > 7 || y_y4 < 0)
                        {
                            break;
                        }


                        if (pCellColumns[x_x4].pcellRows[y_y4] == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (pCellColumns[x_x4].pcellRows[y_y4].CompareTag("White"))
                            {
                                break;
                            }
                            else if (pCellColumns[x_x4].pcellRows[y_y4].name == "BQueen(Clone)" || pCellColumns[x_x4].pcellRows[y_y4].name == "BBishop(Clone)")
                            {
                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                            else
                            {
                                check = false;
                                Background._instance.CheckColor();
                                break;
                            }

                        }
                        check = false;
                        Background._instance.CheckColor();
                    }

                    //      Right Above
                    var x_x1 = i;
                    var y_y1 = j;
                    for (int aa1 = 0; aa1 < 8; aa1++)
                    {
                        if (x_x1 > 7 || x_x1 < 0 || y_y1 > 7 || y_y1 < 0)
                        {
                            break;
                        }
                        x_x1 -= 1;
                        y_y1 += 1;

                        if (x_x1 > 7 || x_x1 < 0 || y_y1 > 7 || y_y1 < 0)
                        {
                            break;
                        }
                        if (pCellColumns[x_x1].pcellRows[y_y1] == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (pCellColumns[x_x1].pcellRows[y_y1].CompareTag("White"))
                            {
                                break;
                            }
                            else if (pCellColumns[x_x1].pcellRows[y_y1].name == "BQueen(Clone)" || pCellColumns[x_x1].pcellRows[y_y1].name == "BBishop(Clone)")
                            {
                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                            else
                            {
                                check = false;

                            }
                        }
                        check = false;
                        Background._instance.CheckColor();
                    }
                    //      Left Above
                    var x_x2 = i;
                    var y_y2 = j;
                    for (int aa2 = 0; aa2 < 8; aa2++)
                    {
                        if (x_x2 > 7 || x_x2 < 0 || y_y2 > 7 || y_y2 < 0)
                        {
                            break;
                        }
                        x_x2 -= 1;
                        y_y2 -= 1;

                        if (x_x2 > 7 || x_x2 < 0 || y_y2 > 7 || y_y2 < 0)
                        {
                            break;
                        }
                        if (pCellColumns[x_x2].pcellRows[y_y2] == null)
                        {
                            continue;
                        }

                        else
                        {

                            if (pCellColumns[x_x2].pcellRows[y_y2].CompareTag("White"))
                            {
                                break;
                            }
                            else if (pCellColumns[x_x2].pcellRows[y_y2].name == "BQueen(Clone)" || pCellColumns[x_x2].pcellRows[y_y2].name == "BBishop(Clone)")
                            {



                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                            else
                            {
                                check = false;
                                Background._instance.CheckColor();
                                break;
                            }
                        }
                        check = false;
                        Background._instance.CheckColor();

                    }


                    //      Upward Check
                    var y2y = i;
                    for (int k2k = 0; k2k < 8; k2k++)
                    {
                        y2y -= 1;
                        if (pCellColumns[y2y].pcellRows[j] != null)
                        {
                            if (pCellColumns[y2y].pcellRows[j].tag == "Black")
                            {

                                if (pCellColumns[y2y].pcellRows[j].name == "BRook(Clone)" || pCellColumns[y2y].pcellRows[j].name == "BQueen(Clone)")
                                {
                                    check = true;
                                    Background._instance.CheckColor();
                                    return;

                                }
                                else
                                {
                                    check = false;
                                }
                            }
                            break;

                        }
                        else if (pCellColumns[y2y].pcellRows[j] == null)
                        {
                            continue;
                        }
                        check = false;
                        Background._instance.CheckColor();



                    }
                    //      Downward Check
                    var y1 = i;
                    for (int l2l = 0; l2l < 8; l2l++)
                    {
                        y1 += 1;
                        if (y1 > 7)
                        {
                            break;
                        }
                        if (pCellColumns[y1].pcellRows[j] == null)
                        {
                            continue;
                        }

                        if (pCellColumns[y1].pcellRows[j] != null)
                        {
                            if (pCellColumns[y1].pcellRows[j].tag == "Black")
                            {

                                if (pCellColumns[y1].pcellRows[j].name == "BRook(Clone)" || pCellColumns[y1].pcellRows[j].name == "BQueen(Clone)")
                                {
                                    check = true;
                                    Background._instance.CheckColor();
                                    return;

                                }
                                else
                                {
                                    check = false;
                                }
                            }
                            break;

                        }
                        check = false;
                        Background._instance.CheckColor();
                    }
                    //      Left Check
                    var y2 = j;
                    for (int m = 0; m < 8; m++)
                    {
                        y2 -= 1;
                        if (y2 < 0)
                        {
                            break;
                        }
                        if (pCellColumns[i].pcellRows[y2] == null)
                        {
                            continue;
                        }

                        if (pCellColumns[i].pcellRows[y2] != null)
                        {
                            if (pCellColumns[i].pcellRows[y2].tag == "Black")
                            {

                                if (pCellColumns[i].pcellRows[y2].name == "BRook(Clone)" || pCellColumns[i].pcellRows[y2].name == "BQueen(Clone)")
                                {
                                    check = true;
                                    Background._instance.CheckColor();
                                    return;

                                }
                                else
                                {
                                    check = false;
                                }
                            }
                            break;

                        }
                        check = false;
                        Background._instance.CheckColor();
                    }
                    //      Right Check
                    var y3 = j;
                    for (int n = 0; n < 8; n++)
                    {
                        y3 += 1;
                        if (y3 > 7)
                        {
                            break;
                        }
                        if (pCellColumns[i].pcellRows[y3] == null)
                        {
                            continue;
                        }

                        if (pCellColumns[i].pcellRows[y3] != null)
                        {
                            if (pCellColumns[i].pcellRows[y3].tag == "Black")
                            {

                                if (pCellColumns[i].pcellRows[y3].name == "BRook(Clone)" || pCellColumns[i].pcellRows[y3].name == "BQueen(Clone)")
                                {
                                    check = true;
                                    Background._instance.CheckColor();
                                    return;

                                }

                            }


                        }
                        check = false;
                        Background._instance.CheckColor();
                    }
                    //////////////////////////////////////////          Black King Check Check
                    for (int zz = 0; zz < 8; zz++)
                    {
                        for (int jj = 0; jj < 8; jj++)
                        {
                            if (pCellColumns[zz].pcellRows[jj] == null)
                            {
                                continue;
                            }

                            else if (pCellColumns[zz].pcellRows[jj].name == "BKing(Clone)")
                            {

                                //      Right Bottom
                                var xx3 = zz;
                                var yy3 = jj;
                                for (int aa3 = 0; aa3 < 8; aa3++)
                                {
                                    if (xx3 > 7 || xx3 < 0 || yy3 > 7 || yy3 < 0)
                                    {
                                        break;
                                    }
                                    xx3 += 1;
                                    yy3 += 1;

                                    if (xx3 > 7 || xx3 < 0 || yy3 > 7 || yy3 < 0)
                                    {
                                        break;
                                    }


                                    if (pCellColumns[xx3].pcellRows[yy3] == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (pCellColumns[xx3].pcellRows[yy3].CompareTag("Black"))
                                        {
                                            break;
                                        }
                                        else if (pCellColumns[xx3].pcellRows[yy3].name == "WQueen (Clone)" || pCellColumns[xx3].pcellRows[yy3].name == "WBishop (Clone)")
                                        {
                                            check = true;
                                            Background._instance.CheckColor();
                                            return;
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }

                                    }
                                    check = false;
                                    Background._instance.CheckColor();
                                }
                                //      Left Bottom
                                var xx4 = zz;
                                var yy4 = jj;
                                for (int aa4 = 0; aa4 < 8; aa4++)
                                {
                                    if (xx4 > 7 || xx4 < 0 || yy4 > 7 || yy4 < 0)
                                    {
                                        break;
                                    }
                                    xx4 += 1;
                                    yy4 -= 1;

                                    if (xx4 > 7 || xx4 < 0 || yy4 > 7 || yy4 < 0)
                                    {
                                        break;
                                    }


                                    if (pCellColumns[xx4].pcellRows[yy4] == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (pCellColumns[xx4].pcellRows[yy4].CompareTag("Black"))
                                        {
                                            break;
                                        }
                                        else if (pCellColumns[xx4].pcellRows[yy4].name == "WQueen (Clone)" || pCellColumns[xx4].pcellRows[yy4].name == "WBishop (Clone)")
                                        {
                                            check = true;
                                            Background._instance.CheckColor();
                                            return;
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }

                                    }
                                    check = false;
                                    Background._instance.CheckColor();
                                }
                                //      Right Above
                                var xx1 = zz;
                                var yy1 = jj;
                                for (int aa1 = 0; aa1 < 8; aa1++)
                                {
                                    if (xx1 > 7 || xx1 < 0 || yy1 > 7 || yy1 < 0)
                                    {
                                        break;
                                    }
                                    xx1 -= 1;
                                    yy1 += 1;

                                    if (xx1 > 7 || xx1 < 0 || yy1 > 7 || yy1 < 0)
                                    {
                                        break;
                                    }
                                    if (pCellColumns[xx1].pcellRows[yy1] == null)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (pCellColumns[xx1].pcellRows[yy1].CompareTag("Black"))
                                        {
                                            break;
                                        }
                                        else if (pCellColumns[xx1].pcellRows[yy1].name == "WQueen (Clone)" || pCellColumns[xx1].pcellRows[yy1].name == "WBishop (Clone)")
                                        {
                                            check = true;
                                            Background._instance.CheckColor();
                                            return;
                                        }
                                        else
                                        {
                                            check = false;

                                        }
                                    }
                                    check = false;
                                    Background._instance.CheckColor();
                                }
                                //      Left Above
                                var xx2 = zz;
                                var yy2 = jj;
                                for (int aa2 = 0; aa2 < 8; aa2++)
                                {
                                    if (xx2 > 7 || xx2 < 0 || yy2 > 7 || yy2 < 0)
                                    {
                                        break;
                                    }
                                    xx2 -= 1;
                                    yy2 -= 1;

                                    if (xx2 > 7 || xx2 < 0 || yy2 > 7 || yy2 < 0)
                                    {
                                        break;
                                    }
                                    if (pCellColumns[xx2].pcellRows[yy2] == null)
                                    {
                                        continue;
                                    }

                                    else
                                    {

                                        if (pCellColumns[xx2].pcellRows[yy2].CompareTag("Black"))
                                        {
                                            break;
                                        }
                                        else if (pCellColumns[xx2].pcellRows[yy2].name == "WQueen (Clone)" || pCellColumns[xx2].pcellRows[yy2].name == "WBishop (Clone)")
                                        {



                                            check = true;
                                            Background._instance.CheckColor();
                                            return;
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }
                                    }
                                    check = false;
                                    Background._instance.CheckColor();

                                }
                                //      Left Check
                                var oo = jj;
                                for (int m = 0; m < 8; m++)
                                {

                                    oo -= 1;
                                    if (oo < 0)
                                    {
                                        break;
                                    }
                                    if (pCellColumns[zz].pcellRows[oo] == null)
                                    {
                                        continue;
                                    }

                                    if (pCellColumns[zz].pcellRows[oo] != null)
                                    {
                                        if (pCellColumns[zz].pcellRows[oo].tag == "White")
                                        {

                                            if (pCellColumns[zz].pcellRows[oo].name == "WRook (Clone)" || pCellColumns[zz].pcellRows[oo].name == "WQueen (Clone)")
                                            {
                                                check = true;
                                                Background._instance.CheckColor();
                                                return;

                                            }
                                            else
                                            {
                                                check = false;
                                                Background._instance.CheckColor();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }

                                    }
                                    
                                }
                                //      right Check
                                var oo1 = jj;
                                for (int m1 = 0; m1 < 8; m1++)
                                {

                                    oo1 += 1;
                                    if (oo1 > 7)
                                    {
                                        break;
                                    }
                                    if (pCellColumns[zz].pcellRows[oo1] == null)
                                    {
                                        continue;
                                    }

                                    if (pCellColumns[zz].pcellRows[oo1] != null)
                                    {
                                        if (pCellColumns[zz].pcellRows[oo1].tag == "White")
                                        {

                                            if (pCellColumns[zz].pcellRows[oo1].name == "WRook (Clone)" || pCellColumns[zz].pcellRows[oo1].name == "WQueen (Clone)")
                                            {
                                                check = true;
                                                Background._instance.CheckColor();
                                                return;

                                            }
                                            else
                                            {
                                                check = false;
                                                Background._instance.CheckColor();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }

                                    }

                                }
                                //      Up Check
                                var oo2 = zz;
                                for (int m2 = 0; m2 < 8; m2++)
                                {

                                    oo2 -= 1;
                                    if (oo2 < 0)
                                    {
                                        break;
                                    }
                                    if (pCellColumns[oo2].pcellRows[jj] == null)
                                    {
                                        continue;
                                    }

                                    if (pCellColumns[oo2].pcellRows[jj] != null)
                                    {
                                        if (pCellColumns[oo2].pcellRows[jj].tag == "White")
                                        {

                                            if (pCellColumns[oo2].pcellRows[jj].name == "WRook (Clone)" || pCellColumns[oo2].pcellRows[jj].name == "WQueen (Clone)")
                                            {
                                                check = true;
                                                Background._instance.CheckColor();
                                                return;

                                            }
                                            else
                                            {
                                                check = false;
                                                Background._instance.CheckColor();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }

                                    }

                                }
                                //      down Check
                                var oo3 = zz;
                                for (int m2 = 0; m2 < 8; m2++)
                                {

                                    oo3 += 1;
                                    if (oo3 > 7)
                                    {
                                        break;
                                    }
                                    if (pCellColumns[oo3].pcellRows[jj] == null)
                                    {
                                        continue;
                                    }

                                    if (pCellColumns[oo3].pcellRows[jj] != null)
                                    {
                                        if (pCellColumns[oo3].pcellRows[jj].tag == "White")
                                        {

                                            if (pCellColumns[oo3].pcellRows[jj].name == "WRook (Clone)" || pCellColumns[oo3].pcellRows[jj].name == "WQueen (Clone)")
                                            {
                                                check = true;
                                                Background._instance.CheckColor();
                                                return;

                                            }
                                            else
                                            {
                                                check = false;
                                                Background._instance.CheckColor();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            check = false;
                                            Background._instance.CheckColor();
                                            break;
                                        }

                                    }

                                }




                            }
                        }
                    }
                }
            }
        }
        Background._instance.CheckColor();
    }

    void GenerateDot(int x,int y)
    {
        if (x>-1&&x<8&&y>-1&&y<8)
        {
            if (pCellColumns[x].pcellRows[y] != null)
            {
                if (SelectedPiece.tag != pCellColumns[x].pcellRows[y].tag)
                {
                    var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                    allDots.Add(newDot);
                    dCellColumns[x].dcellRows[y] = newDot.gameObject;
                }
            }
            else if (pCellColumns[x].pcellRows[y] == null)
            {
                var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                var collider = newDot.GetComponent<BoxCollider>();
                var colliderCenter = collider.center;
                colliderCenter.z += 5f;
                allDots.Add(newDot);
                dCellColumns[x].dcellRows[y] = newDot.gameObject;


            }}
        }
    }



[System.Serializable]
public class PCell
{
    public GameObject[] pcellRows = new GameObject[8];
}
[System.Serializable]
public class DCell
{
    public GameObject[] dcellRows = new GameObject[8];
}


