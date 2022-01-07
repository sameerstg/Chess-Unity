using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool firstMove;
    public Vector3 scale;
    private void Start()
    {
        firstMove = false;
        
    }
    private void OnMouseDown()
    {
        if (!PieceManager._instance.freePlay)
        {
            if (!PieceManager._instance.check)
            {
                if (GameManager._instance.turn && gameObject.CompareTag("White"))
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                }
                if (!GameManager._instance.turn && gameObject.CompareTag("Black"))
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                }
            }

            else if (PieceManager._instance.check)
            {
                if (GameManager._instance.turn && gameObject.CompareTag("White") && gameObject.name == "WKing(Clone)")
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                }
                if (!GameManager._instance.turn && gameObject.CompareTag("Black")&& gameObject.name == "BKing(Clone)")
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                }

            }
        }
        else
        {
            gameObject.transform.localScale *= 2f;
            PieceManager._instance.GetMove(gameObject, gameObject.name);
        }

    }
    private void OnMouseUp()
    {
        if (!PieceManager._instance.freePlay)
        {
            if (!PieceManager._instance.check)
            {
                if (GameManager._instance.turn && gameObject.CompareTag("White"))
                {
                    gameObject.transform.localScale /= 2f;
                }
                if (!GameManager._instance.turn && gameObject.CompareTag("Black"))
                {
                    gameObject.transform.localScale /= 2f;
                }
            }
                

            



        }
        else if (PieceManager._instance.check)
        {
            if (!PieceManager._instance.freePlay)
            {

                if (GameManager._instance.turn && gameObject.CompareTag("White") && gameObject.name == "WKing(Clone)" )
                {
                    gameObject.transform.localScale /= 2f;
                }
                if (!GameManager._instance.turn && gameObject.CompareTag("Black") && gameObject.name == "BKing(Clone)")
                {
                    gameObject.transform.localScale /= 2f;
                }
            }
        }

        else
        {
            gameObject.transform.localScale /= 2f;
        }

    }



    public void DoneFirstMove()
    {
        firstMove = true;
    }
}

