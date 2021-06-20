using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameBoardManager : MonoBehaviour
{
    public int numPlayers;
    public int currentPlayer;
    public int totalTurns;
    public int currentTurn;
    public int dotSize;
    public GameObject dotPrefab;
    public GameObject gameShapePrefab;
    public GameObject scoreTextPrefab;
    public GameObject[,] gameBoard;
    public List<GameObject> gameShapes;
    public RectTransform panel;
    public ScoreBoardManager scoreBoard;

    public float dotScaleMultiplier;
    public float lineScaleMultiplier;

    private float spacing;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard(10, 10, 2);
    }

    public void CreateBoard(int width, int height, int players)
    {
        //destroy previous board if there was one
        if (!(gameBoard is null))
        {
            foreach (GameObject obj in gameBoard)
                Destroy(obj);
            foreach (GameObject obj in gameShapes)
                Destroy(obj);
        }

        //initialize game variables
        numPlayers = players;
        currentPlayer = 1;
        totalTurns = width * height;
        currentTurn = 1;
        gameBoard = new GameObject[height,width];
        gameShapes = new List<GameObject>();

        //finds the optimal spacing and scale between the dots that fits in the screen space
        spacing = math.min(panel.rect.width / (width+1), panel.rect.height / (height+1));
        float scale = spacing / (dotScaleMultiplier * dotSize);

        //find offset from center
        float xOffset = panel.rect.width / 2;
        float yOffset = panel.rect.height / 2;

        //generate board
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject obj = Instantiate(dotPrefab, panel);
                obj.GetComponent<GameDot>().SetCoords((i, j));
                obj.transform.localPosition = new Vector3((j + 1) * spacing - xOffset, (i + 1) * spacing * -1 + yOffset, 0);
                obj.transform.localScale = new Vector3(scale, scale, 0);
                gameBoard[i, j] = obj;
            }
        }

        //initialize scores
        scoreBoard.Init(numPlayers);
    }

    public void ChooseDot((int,int) p1)
    {
        int turnScore = 0;
        (float, float) scorePosTotal = (0,0);
        int numScores = 0;

        GameDot chosenDot = gameBoard[p1.Item1, p1.Item2].GetComponent<GameDot>();
        chosenDot.SetPlayer(currentPlayer);

        for (int p2row = 0;p2row < gameBoard.GetLength(0);p2row++)
        {
            for (int p2col = 0;p2col < gameBoard.GetLength(1);p2col++)
            {
                (int, int) p2 = (p2row, p2col);

                //if the dot is not the same dot, but is the same player
                if(!(p1.Item1 == p2.Item1 && p1.Item2 == p2.Item2) && chosenDot.player 
                    == gameBoard[p2.Item1,p2.Item2].GetComponent<GameDot>().player)
                {
                    //find the next two dots
                    (int, int) p3 = (p2.Item1 + (p2.Item2 - p1.Item2), p2.Item2 + (p1.Item1 - p2.Item1));
                    (int, int) p4 = (p3.Item1 + (p1.Item1 - p2.Item1), p3.Item2 + (p1.Item2 - p2.Item2));

                    //if both points are in bounds, and they are the same color
                    if (p3.Item1 >= 0 && p3.Item1 < gameBoard.GetLength(0) &&
                        p3.Item2 >= 0 && p3.Item2 < gameBoard.GetLength(1) &&
                        p4.Item1 >= 0 && p4.Item1 < gameBoard.GetLength(0) &&
                        p4.Item2 >= 0 && p4.Item2 < gameBoard.GetLength(1) &&
                        chosenDot.player == gameBoard[p3.Item1, p3.Item2].GetComponent<GameDot>().player &&
                        chosenDot.player == gameBoard[p4.Item1, p4.Item2].GetComponent<GameDot>().player)
                    {
                        DrawSquare(p1, p2, p3, p4);
                        turnScore += (int)(math.pow(p2.Item1 - p1.Item1, 2) + math.pow(p2.Item2 - p1.Item2, 2));

                        //for calculating running average of center positions
                        scorePosTotal.Item1 += (gameBoard[p1.Item1, p1.Item2].transform.position.x + gameBoard[p3.Item1, p3.Item2].transform.position.x) / 2;
                        scorePosTotal.Item2 += (gameBoard[p1.Item1, p1.Item2].transform.position.y + gameBoard[p3.Item1, p3.Item2].transform.position.y) / 2;
                        numScores++;
                    }
                }
            }
        }

        //display the score above the board
        if (turnScore > 0)
        {
            GameObject scoreText = Instantiate(scoreTextPrefab, panel);
            scoreText.GetComponentInChildren<TMP_Text>().text = "+" + turnScore;
            scoreText.transform.position = new Vector3(scorePosTotal.Item1 / numScores, scorePosTotal.Item2 / numScores, 0);

            scoreBoard.AddScore(currentPlayer, turnScore);
        }

        //advance the player
        currentPlayer++;
        if (currentPlayer > numPlayers)
            currentPlayer = 1;

        //advance the turn counter
        currentTurn++;
        //check if the game is over
        if(currentTurn > totalTurns)
            scoreBoard.EndGame();
    }

    public void DrawSquare(params (int,int)[] points)
    {
        Vector2[] positions = System.Array.ConvertAll<(int, int), Vector2>(points, p => gameBoard[p.Item1, p.Item2].transform.position);
        GameObject obj = Instantiate(gameShapePrefab, panel);
        gameShapes.Add(obj);
        obj.GetComponent<GameShape>().DrawShape(currentPlayer, spacing / lineScaleMultiplier, positions);
    }
}
