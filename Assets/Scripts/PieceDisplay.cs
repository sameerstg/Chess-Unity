using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceDisplay : MonoBehaviour
{
    public Text text;
    public static PieceDisplay _instance;
    private void Awake()
    {
        _instance = this;
    }


    public void ShowingText(string pieceName)
    {
        text.text = pieceName.ToString();
    }
}
