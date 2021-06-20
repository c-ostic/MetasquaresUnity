using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public Button[] playerIcons;
    public int[] playerScores;
    public TMP_Text[] playerScoreLabels;
    public TMP_Text endGameLabel;
    public ColorManager colorManager;

    private int numPlayers;

    public void Init(int numPlay)
    {
        numPlayers = numPlay;

        for(int i = 0;i < playerIcons.Length;i++)
        {
            if (i < numPlayers)
            {
                playerIcons[i].image.sprite = colorManager.playerSprites[i + 1];
            }
            else
            {
                playerIcons[i].image.sprite = colorManager.playerSprites[0];
            }
        }

        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i] = 0;
            playerScoreLabels[i].fontStyle = FontStyles.Normal;
            playerScoreLabels[i].color = Color.white;
        }
    }

    public void Update()
    {
        for (int i = 0;i < playerScoreLabels.Length;i++)
            playerScoreLabels[i].text = playerScores[i] + "";
    }

    public void AddScore(int player, int score)
    {
        playerScores[player - 1] += score;
    }

    public void EndGame()
    {
        int maxValue = playerScores.Max();
        List<int> winnerList = new List<int>();
        for (int i = 0; i < numPlayers; i++)
            if (playerScores[i] == maxValue)
            {
                //if a player has a winning score, add them to the win list and make their score bold
                winnerList.Add(i + 1);
                playerScoreLabels[i].fontStyle = FontStyles.Bold;
                playerScoreLabels[i].color = Color.green;
            }

        int[] winners = winnerList.ToArray();

        string winText;
        if (winners.Length == 1)
            winText = "Player " + (winners[0]) + " won with " + maxValue + "!";
        else
        {
            winText = "Players ";
            for (int i = 0; i < winners.Length - 1; i++)
                winText += winners[i] + ", ";
            winText += "and " + winners[winners.Length - 1];
            winText += " tied with " + maxValue + "!";
        }

        endGameLabel.text = winText;
    }
}
