using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePlayButton : MonoBehaviour
{
    public void ToggleFreePlay()
    {
        bool x = PieceManager._instance.freePlay;
        if (x)
        {
            x = false;
        }
        else
        {
            x = true;
        }
        PieceManager._instance.freePlay = x;
    }

}
