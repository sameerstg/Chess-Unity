﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private void OnMouseDown()
    {
        PieceManager._instance.Deselect();
    }
}
