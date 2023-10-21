using System;
using TMPro;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public TMP_InputField numPlayers;
    public TMP_InputField width;
    public TMP_InputField height;
    
    private GameBoardManager board;

    /// <summary>
    /// Called before the first frame update
    /// Finds the game board
    /// </summary>
    private void Start()
    {
        board = FindObjectOfType<GameBoardManager>();
    }

    /// <summary>
    /// Creates a new board using the specified width and height with the specified number of players
    /// </summary>
    public void CreateGame()
    {
        board.CreateBoard(Int32.Parse(width.text),Int32.Parse(height.text),Int32.Parse(numPlayers.text));
    }
}
