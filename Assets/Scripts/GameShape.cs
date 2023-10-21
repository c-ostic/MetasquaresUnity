using UnityEngine;

/// <summary>
/// Draws the shape overlay for any completed shapes during the game
/// </summary>
public class GameShape : MonoBehaviour
{
    /// <summary>
    /// Draws the specified shape over the gameboard
    /// </summary>
    /// <param name="playerNum">Determines the color of the shape</param>
    /// <param name="width">How wide the lines should be</param>
    /// <param name="vertices2D">The verticies of the shape</param>
    public void DrawShape(int playerNum, float width, Vector2[] vertices2D)
    {
        ColorManager colorManager = FindObjectOfType<ColorManager>();
        LineRenderer lineRend = GetComponent<LineRenderer>();

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
