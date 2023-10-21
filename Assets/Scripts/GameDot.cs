using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single dot on the game board
/// Holds current player and logical position data
/// </summary>
public class GameDot : MonoBehaviour
{
    public Vector2Int Coordinate { get; private set; }
    public int Player { get; private set; } //0 for no player, 1+ for player

    private Button dotButton;
    private ColorManager colorManager;

    /// <summary>
    /// Called when this object is created
    /// Gets the button attached to this object
    /// </summary>
    private void Awake()
    {
        dotButton = GetComponent<Button>();
    }

    /// <summary>
    /// Called before the frame update
    /// Finds the color manager
    /// Sets the default color
    /// </summary>
    private void Start()
    {
        colorManager = FindObjectOfType<ColorManager>();
        dotButton.image.sprite = colorManager.playerSprites[0];
        dotButton.image.color = colorManager.playerColors[0];
        Player = 0;
    }

    /// <summary>
    /// Sets the coordinates of this point
    /// </summary>
    /// <param name="newPoint">The coordinates to set to</param>
    public void SetCoords(Vector2Int newPoint)
    {
        Coordinate = newPoint;
    }

    /// <summary>
    /// Sets the player data and color
    /// </summary>
    /// <param name="playerNum">The player to change to</param>
    public void SetPlayer(int playerNum)
    {
        Player = playerNum;
        dotButton.image.sprite = colorManager.playerSprites[playerNum];
    }

    /// <summary>
    /// Called when this object is pressed
    /// </summary>
    public void ClickDot()
    {
        //once the dot is chosen, make it non-interactable
        dotButton.interactable = false;

        //find the game board and pass its location
        GameBoardManager gameBoardManager = FindObjectOfType<GameBoardManager>();
        gameBoardManager.ChooseDot(Coordinate);
    }
}
