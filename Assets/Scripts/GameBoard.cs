using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// The object that represents the game board, including dots and the shapes drawn on the board
/// </summary>
public class GameBoard : MonoBehaviour
{
    public Vector2 LastScorePosition { get; private set; }

    [SerializeField]
    private GameObject gameDotPrefab;
    [SerializeField]
    private GameObject gameShapePrefab;
    [SerializeField]
    private float dotScaleMultiplier;
    [SerializeField]
    private float lineScaleMultiplier;

    private GameDot[,] gameBoard;
    private List<GameShape> gameShapes;
    private RectTransform boardContainer;
    private int width, height;
    private float spacing;

    /// <summary>
    /// Called on scene load
    /// </summary>
    private void Awake()
    {
        boardContainer = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Creates a new board of dots given the width and height
    /// </summary>
    /// <param name="width">Width of the new board</param>
    /// <param name="height">Height of the new board</param>
    public void CreateBoard(int width, int height)
    {
        //destroy previous board if there was one
        if (!(gameBoard is null))
        {
            foreach (GameDot obj in gameBoard)
                Destroy(obj.gameObject);
            foreach (GameShape obj in gameShapes)
                Destroy(obj.gameObject);
        }

        this.width = width;
        this.height = height;
        gameBoard = new GameDot[height, width];
        gameShapes = new List<GameShape>();

        //generate board
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameDot obj = Instantiate(gameDotPrefab, this.transform).GetComponent<GameDot>();
                obj.SetCoords(new Vector2Int(i, j));
                gameBoard[i, j] = obj;
            }
        }

        ScaleBoard();
    }

    /// <summary>
    /// Called whenever the window size changes
    /// </summary>
    public void OnRectTransformDimensionsChange()
    {
        if (!(gameBoard is null))
            ScaleBoard();
    }

    /// <summary>
    /// Scales the board appropriately to fit the window
    /// </summary>
    public void ScaleBoard()
    {
        // Find correct spacing and scale based on size
        spacing = Mathf.Min(boardContainer.rect.width / (width + 1), boardContainer.rect.height / (height + 1));
        float dotScale = spacing * dotScaleMultiplier;

        // Find offset from center
        float xOffset = boardContainer.rect.width / 2;
        float yOffset = boardContainer.rect.height / 2;

        // Apply spacing and offset to dots
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                gameBoard[i, j].transform.localPosition = new Vector3((j + 1) * spacing - xOffset, (i + 1) * spacing * -1 + yOffset, 0);
                gameBoard[i, j].transform.localScale = new Vector3(dotScale, dotScale, 0);
            }
        }
    }

    /// <summary>
    /// Chooses a dot on the board, determining any shapes made and the total score achieved
    /// </summary>
    /// <param name="coordinate">The coordinate of the dot chosen</param>
    /// <param name="player">The player that chose the dot</param>
    /// <returns></returns>
    public int ChooseDot(Vector2Int coordinate, int player)
    {
        // TODO: place the dot, calculate any correct shapes, return score
        int turnScore = 0, numScores = 0;
        Vector2 scorePositionTotal = Vector2.zero;

        GameDot chosenDot = gameBoard[coordinate.x, coordinate.y];
        chosenDot.SetPlayer(player);

        Vector2Int p1 = chosenDot.Coordinate;

        for (int p2row = 0;p2row < gameBoard.GetLength(0);p2row++)
        {
            for (int p2col = 0;p2col < gameBoard.GetLength(1);p2col++)
            {
                Vector2Int p2 = new Vector2Int(p2row, p2col);

                //if the dot is not the same dot, but is the same player
                if(!p1.Equals(p2) && chosenDot.Player == gameBoard[p2.x, p2.y].Player)
                {
                    //find the next two dots
                    Vector2Int p3 = new Vector2Int(p2.x + (p2.y - p1.y), p2.y + (p1.x - p2.x));
                    Vector2Int p4 = new Vector2Int(p3.x + (p1.x - p2.x), p3.y + (p1.y - p2.y));

                    //if both points are in bounds, and they are the same color
                    if (p3.x >= 0 && p3.x < gameBoard.GetLength(0) &&
                        p3.y >= 0 && p3.y < gameBoard.GetLength(1) &&
                        p4.x >= 0 && p4.x < gameBoard.GetLength(0) &&
                        p4.y >= 0 && p4.y < gameBoard.GetLength(1) &&
                        chosenDot.Player == gameBoard[p3.x, p3.y].Player &&
                        chosenDot.Player == gameBoard[p4.x, p4.y].Player)
                    {
                        DrawShape(player, p1, p2, p3, p4);
                        turnScore += (int)(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));

                        Debug.Log(p1 + "+" + p2 + "+" + p3 + "+" + p4);

                        //for calculating running average of center positions
                        scorePositionTotal.x += (gameBoard[p1.x, p1.y].transform.position.x + gameBoard[p3.x, p3.y].transform.position.x) / 2;
                        scorePositionTotal.y += (gameBoard[p1.x, p1.y].transform.position.y + gameBoard[p3.x, p3.y].transform.position.y) / 2;
                        numScores++;
                    }
                }
            }
        }

        LastScorePosition = scorePositionTotal / numScores;

        return turnScore;
    }

    /// <summary>
    /// Draws a shape onto the board at the given points
    /// </summary>
    /// <param name="player">The current player, which determines the color</param>
    /// <param name="points">The list of coordinate points to draw the shape onto</param>
    private void DrawShape(int player, params Vector2Int[] points)
    {
        // Convert local game board coordinates to world positions
        Vector2[] positions = Array.ConvertAll<Vector2Int, Vector2>(points, p => gameBoard[p.x, p.y].transform.position);
        
        // Create the game shape and add it to the list of shapes
        GameShape obj = Instantiate(gameShapePrefab, boardContainer).GetComponent<GameShape>();
        gameShapes.Add(obj);
        obj.DrawShape(player, spacing * lineScaleMultiplier, positions);
    }
}
