using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public TMP_InputField numPlayers;
    public TMP_InputField width;
    public TMP_InputField height;
    public GameBoardManager board;

    public void CreateGame()
    {
        board.CreateBoard(Int32.Parse(width.text),Int32.Parse(height.text),Int32.Parse(numPlayers.text));
    }
}
