using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promotion : MonoBehaviour
{
    private void OnMouseDown()
    {
/*        print(gameObject.transform.parent.tag);
*/        if (gameObject.transform.parent.CompareTag("Promotion"))
        {
            if (gameObject.name == "WBishop ")
            {
                PieceManager._instance.Promotion(true, 5);
            }
            else if (gameObject.name == "WKnight ")
            {
                PieceManager._instance.Promotion(true, 6);

            }

            else if (gameObject.name == "WQueen ")
            {
                PieceManager._instance.Promotion(true, 4);

            }
            else if (gameObject.name == "BBishop")
            {
                PieceManager._instance.Promotion(false, 10);

            }
            else if (gameObject.name == "BKnight")
            {
                PieceManager._instance.Promotion(false, 9);

            }
            else if (gameObject.name == "BQueen")
            {
                PieceManager._instance.Promotion(false, 12);

            }
        }
        
    }
}
