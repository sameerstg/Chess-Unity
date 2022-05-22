using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background _instance;
    public MeshRenderer mesh;
     void Start()
    {
        _instance = this;
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.cyan;

    }
    public void OnMouseDown()
    {
        PieceManager._instance.Deselect();
    }
    public void CheckColor()
    {
        if (PieceManager._instance.check)
        {
            mesh.material.color = Color.red;
        }
        else
        {
            mesh.material.color = Color.cyan;
        }
    }
}
