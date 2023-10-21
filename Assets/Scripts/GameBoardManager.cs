using TMPro;
using UnityEngine;

/// <summary>
/// Manages the entire game, including creating the game board, setting the scores, and determining when to end the game
/// </summary>
public class GameBoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreTextPrefab;
    //public GameBoard gameBoardPrefab; //TODO

    private int numPlayers;
    private int currentPlayer;
    private int totalTurns;
    private int currentTurn;
    private ScoreBoardManager scoreBoard;
    private GameBoard gameBoard;

    /// <summary>
    /// Called before the first frame update
    /// Finds the scoreboard
    /// Creates a default board
    /// </summary>
    private void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoardManager>();
        gameBoard = FindObjectOfType<GameBoard>();
        CreateBoard(10, 10, 2);
    }

    /// <summary>
    /// Creates a new board with the specified parameters
    /// </summary>
    /// <param name="width">the width of the board</param>
    /// <param name="height">the height of the board</param>
    /// <param name="players">how many players will play on the board</param>
    public void CreateBoard(int width, int height, int players)
    {
        //initialize game variables
        numPlayers = players;
        currentPlayer = 1;
        totalTurns = width * height; // the total turns is limited by the size of the board
        currentTurn = 1;

        gameBoard.CreateBoard(width, height);

        scoreBoard.Init(numPlayers);
    }

    /// <summary>
    /// Selects the dot at the given coordinates
    /// </summary>
    /// <param name="coordinate">The dot to be selected</param>
    public void ChooseDot(Vector2Int coordinate)
    {
        int turnScore = gameBoard.ChooseDot(coordinate, currentPlayer);

        //display the score above the board
        if (turnScore > 0)
        {
            GameObject scoreText = Instantiate(scoreTextPrefab, transform);
            scoreText.GetComponentInChildren<TMP_Text>().text = "+" + turnScore;
            scoreText.transform.position = gameBoard.LastScorePosition;

            scoreBoard.AddScore(currentPlayer, turnScore);
        }

        //advance the player
        currentPlayer++;
        if (currentPlayer > numPlayers)
            currentPlayer = 1;
        scoreBoard.ChangePlayer(currentPlayer);

        //advance the turn counter
        currentTurn++;

        //check if the game is over
        if(currentTurn > totalTurns)
            scoreBoard.EndGame();
    }
}
