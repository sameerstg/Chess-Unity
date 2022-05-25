using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshRenderer mesh;
    public Color defaultColor;
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        defaultColor = mesh.material.color;
    }
    private void OnMouseEnter()
    {
        mesh.material.color = Color.red;
        PieceDisplay._instance.ShowingText(gameObject.name);
    }
    private void OnMouseExit()
    {
        mesh.material.color = defaultColor;
    }

}
