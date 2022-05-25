﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PieceManager;

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
                if (GameManager._instance.turn && gameObject.CompareTag(pieceColor.White.ToString()))
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                    StartCoroutine(DelayForScaleUp());
                }
                if (!GameManager._instance.turn && gameObject.CompareTag(pieceColor.Black.ToString()))
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                    StartCoroutine(DelayForScaleUp());
                }
            }

            else if (PieceManager._instance.check)
            {
                if (GameManager._instance.turn && gameObject.CompareTag(pieceColor.White.ToString()) && gameObject.name == "WKing(Clone)")
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                    StartCoroutine(DelayForScaleUp());
                }
                if (!GameManager._instance.turn && gameObject.CompareTag(pieceColor.Black.ToString()) && gameObject.name == "BKing(Clone)")
                {
                    gameObject.transform.localScale *= 2f;
                    PieceManager._instance.GetMove(gameObject, gameObject.name);
                    StartCoroutine(DelayForScaleUp());
                }

            }
        }
        else
        {
            gameObject.transform.localScale *= 2f;
            PieceManager._instance.GetMove(gameObject, gameObject.name);
            StartCoroutine(DelayForScaleUp());
        }

    }

    public IEnumerator DelayForScaleUp()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.transform.localScale /= 2f;

    }

    public void DoneFirstMove()
    {
        firstMove = true;
    }
}
