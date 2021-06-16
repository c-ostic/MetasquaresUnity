using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShape : MonoBehaviour
{
    public LineRenderer lineRend;

    private ColorManager colorManager;

    public void DrawShape(int playerNum, float width, Vector2[] vertices2D)
    {
        colorManager = FindObjectOfType<ColorManager>();

        //get vector3 versions of vertices
        Vector3[] vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

        //make outline
        lineRend.startWidth = width;
        lineRend.startWidth = width;
        lineRend.positionCount = vertices2D.Length;
        lineRend.SetPositions(vertices3D);
        lineRend.startColor = colorManager.playerColors[playerNum];
        lineRend.endColor = colorManager.playerColors[playerNum];
    }
}
