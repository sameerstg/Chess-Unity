using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{

    public static PieceManager _instance;
    public PCellC[] history;
    //      To play without turns
    public bool freePlay;

    //      Piece Prefab List
    public List<GameObject> whitePiecesList;
    public List<GameObject> blackPiecesList;

    public DCell[] dCellColumns;
    public PCell[] pCellColumns;


    //      List of White Pieces
    List<GameObject> whitePieces;
    //      List of Black Pieces
    List<GameObject> blackPieces;
    //      Count Of Pieces
    int bPiecesCount;
    int wPiecesCount;

    //      Selected Piece
    public GameObject SelectedPiece;
    //      Dot To be Instantiated
    public GameObject dot;
    //      Instatiated Dots list
    public List<GameObject> allDots;



    //      Turn Decision
    //      If(true)Whites Turn;else black turns
    public bool turn;
    //      Check Boolean
    public bool check, checkmate, futureCheck;
    //      move text
    public Text moveText, totalMoveText;
    int move;
    public int totalMoves;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        
            //      Instantiating All Classes 8 times
            pCellColumns = new PCell[8];
            dCellColumns = new DCell[8];

       
            history = new PCellC[300];
        
        MoveText(0);


        whitePieces = new List<GameObject>();
        blackPieces = new List<GameObject>();
        bPiecesCount = 0; wPiecesCount = 0;

        dot = Resources.Load<GameObject>("Dot");
        allDots = new List<GameObject>();

        turn = true;
        check = false;
        checkmate = false;
        futureCheck = false;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GoToMove(0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && move - 1 >= 0)
        {
            GoToMove(move - 1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && move + 1 <= totalMoves)
        {
            GoToMove(move + 1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GoToMove(totalMoves);
        }


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
        gameObject.transform.position = Vector3.forward * -1;
        SaveMove();
    }
    public void GoToMove(int move)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {

                if (pCellColumns[i].pcellRows[j] != null)
                {
                    pCellColumns[i].pcellRows[j].SetActive(false);
                    pCellColumns[i].pcellRows[j] = null;
                }
                if (history[move].pCellColumns[i].pcellRows[j] != null)
                {

                    pCellColumns[i].pcellRows[j] = history[move].pCellColumns[i].pcellRows[j];

                    pCellColumns[i].pcellRows[j].gameObject.transform.position = BoardGenerator._instance.CellColumns[i].cellRows[j].transform.position;
                    SetPieceUp(pCellColumns[i].pcellRows[j].gameObject);
                }
            }
        }
        for (int ii = 0; ii < 8; ii++)
        {
            for (int jj = 0; jj < 8; jj++)
            {
                if (pCellColumns[ii].pcellRows[jj] != null)
                {
                    pCellColumns[ii].pcellRows[jj].SetActive(true);
                }
            }
        }
        Check();

        MoveText(move);
    }
    public void MoveText(int arg)
    {
        move = arg;
        moveText.text = "Move: " + move.ToString();
    }
    public void SetPieceDimension(GameObject whatToRotate)
    {
        whatToRotate.transform.Rotate(Vector3.up * 90);
        whatToRotate.transform.parent = transform;
    }
    public void SetPieceUp(GameObject piece)
    {
        piece.SetActive(true);
        var x = piece.transform.position; x.z -= 1f;
        piece.transform.position = x;

    }
    public void GetMove(GameObject PieceGameObject, String pieceName)
    {
        Deselect();
        SelectedPiece = PieceGameObject;
        GetPiecePos(PieceGameObject, pieceName);

    }
    public void GetPiecePos(GameObject obj, String nameOfPiece)
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

    public void ShowAllMoves(GameObject piece, int xAxisPos, int yAxisPos, String pieceName)
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
                if (yAxisPos + i < 7 && yAxisPos + i > -1)
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
            for (int k = 2; k > -3; k--)
            {

                if (k == 0)
                {
                    continue;
                }
                else if (k == 2 || k == -2)
                {
                    GenerateDot(xAxisPos + k, yAxisPos + 1);
                    GenerateDot(xAxisPos + k, yAxisPos - 1);

                }
                else if (k == 1 || k == -1)
                {
                    GenerateDot(xAxisPos + k, yAxisPos + 2);
                    GenerateDot(xAxisPos + k, yAxisPos - 2);
                }

            }

        }
        else if (piece.name == "WKnight (Clone)")
        {
            for (int k = 2; k > -3; k--)
            {

                if (k == 0)
                {
                    continue;
                }
                else if (k == 2 || k == -2)
                {
                    GenerateDot(xAxisPos + k, yAxisPos + 1);
                    GenerateDot(xAxisPos + k, yAxisPos - 1);

                }
                else if (k == 1 || k == -1)
                {
                    GenerateDot(xAxisPos + k, yAxisPos + 2);
                    GenerateDot(xAxisPos + k, yAxisPos - 2);
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
            for (int i = 1; i > -2; i--)
            {
                for (int j = -1; j < 2; j++)
                {

                    GenerateDot(xAxisPos + i, yAxisPos + j);
                }
            }

        }
        else if (piece.name == "WKing(Clone)")
        {
            for (int i = 1; i > -2; i--)
            {
                for (int j = -1; j < 2; j++)
                {
                    GenerateDot(xAxisPos + i, yAxisPos + j);
                }
            }
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
        NewMove();
        Deselect();
    }
    public void SaveMove()
    {

        for (int xx = 0; xx < 8; xx++)
        {
            for (int yy = 0; yy < 8; yy++)
            {
                if (pCellColumns[xx].pcellRows[yy] != null)
                {

                    history[move].pCellColumns[xx].pcellRows[yy] = pCellColumns[xx].pcellRows[yy].gameObject;
                }

            }
        }
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
    public void NewMove()
    {
        MoveText(move + 1);
        totalMoves += 1;
        totalMoveText.text = "Total Move: " + totalMoves;
        SaveMove();
    }
    public void Take(GameObject piece, int x, int y)
    {
        piece.SetActive(false);
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
                    WhiteKingCheck(i, j);
                    if (check == true)
                    {
                        Background._instance.CheckColor();
                        return;
                    }
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
                            BlackKingCheck(zz, jj);
                            if (check == true)
                            {
                                Background._instance.CheckColor();
                                return;
                            }
                        }
                    }
                }
            }
        }


    }

    public void WhiteKingCheck(int i, int j)
    {
        // Pawn check
        if (i - 1 >= 0 && i - 1 <= 7 && j + 1 >= 0 && j + 1 <= 7)
        {
            if (pCellColumns[i - 1].pcellRows[j + 1] != null)
            {
                if (pCellColumns[i - 1].pcellRows[j + 1].name == "BPawn(Clone)")
                {
                    check = true;
                    Background._instance.CheckColor();
                    return;
                }
                else
                {
                    Background._instance.CheckColor();

                }
            }
        }
        if (i - 1 >= 0 && i - 1 <= 7 && j - 1 >= 0 && j - 1 <= 7)
        {
            if (pCellColumns[i - 1].pcellRows[j - 1] != null)
            {
                if (pCellColumns[i - 1].pcellRows[j - 1].name == "BPawn(Clone)")
                {
                    check = true;
                    Background._instance.CheckColor();
                    return;
                }
                else
                {
                    Background._instance.CheckColor();

                }
            }
        }

        // Knights check
        for (int k2k = 2; k2k > -3; k2k--)
        {

            if (k2k == 0)
            {
                continue;
            }


            else if (k2k == 2 || k2k == -2)
            {
                if (i + k2k <= 7 && i + k2k >= 0 && j + 1 >= 0 && j + 1 <= 7)
                {
                    if (pCellColumns[i + k2k].pcellRows[j + 1] != null)
                    {
                        if (pCellColumns[i + k2k].pcellRows[j + 1].name == "BKnight(Clone)")
                        {
                            check = true;
                            Background._instance.CheckColor();
                            return;
                        }
                    }

                }
                if (i + k2k <= 7 && i + k2k >= 0 && j - 1 >= 0 && j - 1 <= 7)
                {
                    if (pCellColumns[i + k2k].pcellRows[j - 1] != null)
                    {
                        if (pCellColumns[i + k2k].pcellRows[j - 1].name == "BKnight(Clone)")
                        {
                            check = true;
                            Background._instance.CheckColor();
                            return;
                        }
                    }
                }


                else
                {
                    Background._instance.CheckColor();
                    continue;
                }


            }
            else if (k2k == 1 || k2k == -1)
            {
                if (i + k2k >= 0 && i + k2k <= 7)
                {
                    if (j - 2 >= 0 && j - 2 <= 7)
                    {
                        if (pCellColumns[i + k2k].pcellRows[j - 2] != null)
                        {
                            if (pCellColumns[i + k2k].pcellRows[j - 2].name == "BKnight(Clone)")
                            {
                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                        }

                    }
                    if (j + 2 >= 0 && j + 2 <= 7)
                    {
                        if (pCellColumns[i + k2k].pcellRows[j + 2] != null)
                        {
                            if (pCellColumns[i + k2k].pcellRows[j + 2].name == "BKnight(Clone)")
                            {
                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                        }

                    }


                }

            }
            else
            {
                Background._instance.CheckColor();
                continue;

            }

        }


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
                    Background._instance.CheckColor();
                    break;

                }
            }

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

            if (y2y < 0)
            {
                break;
            }
            else if (pCellColumns[y2y].pcellRows[j] == null)
            {
                continue;
            }
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
    }
    public void BlackKingCheck(int zz, int jj)
    {
        // Pawn check
        if (zz + 1 >= 0 && zz + 1 <= 7 && jj + 1 >= 0 && jj + 1 <= 7)
        {
            if (pCellColumns[zz + 1].pcellRows[jj + 1] != null)
            {
                if (pCellColumns[zz + 1].pcellRows[jj + 1].name == "WPawn (Clone)")
                {


                    check = true;
                    Background._instance.CheckColor();
                    return;
                }
                else
                {
                    check = false;
                    Background._instance.CheckColor();
                }
            }
        }
        if (zz + 1 >= 0 && zz + 1 <= 7 && jj - 1 >= 0 && jj - 1 <= 7)
        {
            if (pCellColumns[zz + 1].pcellRows[jj - 1] != null)
            {
                if (pCellColumns[zz + 1].pcellRows[jj - 1].name == "WPawn (Clone)")
                {

                    check = true;
                    Background._instance.CheckColor();
                    return;
                }
                else
                {
                    check = false;
                    Background._instance.CheckColor();
                }
            }
        }
        // Knights check
        for (int k2k = 2; k2k > -3; k2k--)
        {

            if (k2k == 0)
            {
                continue;
            }


            else if (k2k == 2 || k2k == -2)
            {
                if (zz + k2k <= 7 && zz + k2k >= 0 && jj + 1 >= 0 && jj + 1 <= 7)
                {
                    if (pCellColumns[zz + k2k].pcellRows[jj + 1] != null)
                    {
                        if (pCellColumns[zz + k2k].pcellRows[jj + 1].name == "WKnight (Clone)")
                        {
                            check = true;
                            Background._instance.CheckColor();
                            return;
                        }
                    }

                }
                if (zz + k2k <= 7 && zz + k2k >= 0 && jj - 1 >= 0 && jj - 1 <= 7)
                {
                    if (pCellColumns[zz + k2k].pcellRows[jj - 1] != null)
                    {
                        if (pCellColumns[zz + k2k].pcellRows[jj - 1].name == "WKnight (Clone)")
                        {
                            check = true;
                            Background._instance.CheckColor();
                            return;
                        }
                    }
                }


                else
                {
                    Background._instance.CheckColor();
                    continue;
                }


            }
            else if (k2k == 1 || k2k == -1)
            {
                if (zz + k2k >= 0 && zz + k2k <= 7)
                {
                    if (jj - 2 >= 0 && jj - 2 <= 7)
                    {
                        if (pCellColumns[zz + k2k].pcellRows[jj - 2] != null)
                        {
                            if (pCellColumns[zz + k2k].pcellRows[jj - 2].name == "WKnight (Clone)")
                            {
                                check = true;
                                Background._instance.CheckColor();
                                return;
                            }
                        }

                    }
                    if (jj + 2 >= 0 || jj + 2 <= 7)
                    {
                        if (jj + 2 <= 7 || jj + 2 >= 0)
                        {
                            if ((jj + 2) >= 0 && (jj + 2) <= 7)
                            {
                                if (pCellColumns[zz + k2k].pcellRows[jj + 2] != null)
                                {
                                    if (pCellColumns[zz + k2k].pcellRows[jj + 2].name == "WKnight (Clone)")
                                    {
                                        check = true;
                                        Background._instance.CheckColor();
                                        return;
                                    }
                                }
                            }

                        }
                    }


                }

            }
            else
            {
                Background._instance.CheckColor();
                continue;

            }

        }
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
    void GenerateDot(int x, int y)
    {
        if (x > 7 || x < 0 || y > 7 || y < 0)
        {
            return;
        }
        if (x > -1 && x < 8 && y > -1 && y < 8)
        {
            if (pCellColumns[x].pcellRows[y] != null)
            {
                if (SelectedPiece.tag != pCellColumns[x].pcellRows[y].tag)
                {
                    if (pCellColumns[x].pcellRows[y].name != "WKing(Clone)" || pCellColumns[x].pcellRows[y].name != "BKing(Clone)")
                    {
                        if (SelectedPiece.name == "WKing(Clone)" || SelectedPiece.name == "BKing(Clone)")
                        {
                            WhiteKingCheck(x, y);
                            if (check)
                            {
                                check = false;
                                Background._instance.CheckColor();
                            }
                            else
                            {
                                var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                                allDots.Add(newDot);
                                dCellColumns[x].dcellRows[y] = newDot.gameObject;

                            }
                        }
                        else
                        {
                            var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                            allDots.Add(newDot);
                            dCellColumns[x].dcellRows[y] = newDot.gameObject;
                        }
                    }
                }

            }
            else if (pCellColumns[x].pcellRows[y] == null)
            {
                if (SelectedPiece.name == "WKing(Clone)")
                {
                    WhiteKingCheck(x, y);
                    if (check)
                    {
                        check = false;
                        Background._instance.CheckColor();
                    }
                    else
                    {
                        var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                        allDots.Add(newDot);
                        dCellColumns[x].dcellRows[y] = newDot.gameObject;

                    }
                }
                else if (SelectedPiece.name == "BKing(Clone)")
                {
                    BlackKingCheck(x, y);
                    if (check)
                    {
                        check = false;
                        Background._instance.CheckColor();
                    }
                    else
                    {
                        var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                        allDots.Add(newDot);
                        dCellColumns[x].dcellRows[y] = newDot.gameObject;

                    }
                }
                else
                {
                    var newDot = Instantiate(dot, BoardGenerator._instance.CellColumns[x].cellRows[y].transform.position, Quaternion.identity);
                    allDots.Add(newDot);
                    dCellColumns[x].dcellRows[y] = newDot.gameObject;
                }



            }


        }



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
[System.Serializable]
public class PCellC
{
    public PCell[] pCellColumns = new PCell[8];
}

