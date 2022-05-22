using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{

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
        PieceManager._instance.Move(gameObject);
        GameManager._instance.ChangeTurn();
    }
}
