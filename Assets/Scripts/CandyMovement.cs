using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyMovement : MonoBehaviour
{


    Camera cam;
    Vector3 initialPosition;

    void Start()
    {
        cam = Camera.main;
        initialPosition = transform.position;
    }

    void OnMouseDrag()
    {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    void OnMouseUp()
    {
        transform.position = initialPosition;
    }
}
