using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PieceManager;

public class Dot : MonoBehaviour
{
    public string info;
    

    private void OnMouseEnter()
    {
        gameObject.transform.localScale *= 1.5f;
    }

    private void OnMouseExit()
    {
        gameObject.transform.localScale /= 1.5f;

    }


    private void OnMouseDown()
    {
        if (info == "")
        {
            PieceManager._instance.Move(gameObject);
            GameManager._instance.ChangeTurn();
        }
        else if (info == castling.left.ToString())
        {
            PieceManager._instance.LeftCastling(gameObject);
        }
        else if (info == castling.right.ToString())
        {
            PieceManager._instance.RightCastling(gameObject);

        }

    }
}
