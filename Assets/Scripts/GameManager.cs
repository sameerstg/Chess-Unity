using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public bool turn;
    public Text turnText;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        turn = true;
        turnText.text = "White Turn";


/*        BoardGenerator._instance.Gernerate();
*/    }
    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeTurn()
    {
        if (turn)
        {
            turn = false;
            turnText.text = "Black Turn";
        }
        else
        {
            turn = true;
            turnText.text = "White Turn";
        }
        PieceManager._instance.Check();
    }
}
