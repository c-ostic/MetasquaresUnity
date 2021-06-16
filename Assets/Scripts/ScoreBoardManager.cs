using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public Button[] playerIcons;
    public TMP_Text[] playerScores;
    public ColorManager colorManager;

    public void Init(int numPlayers)
    {
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

        foreach (TMP_Text t in playerScores)
            t.text = "0";
    }

    public void AddScore(int player, int score)
    {
        int currentScore = Int32.Parse(playerScores[player - 1].text);
        currentScore += score;
        playerScores[player - 1].text = currentScore + "";
    }
}
