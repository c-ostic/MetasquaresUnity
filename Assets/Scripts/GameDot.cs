using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDot : MonoBehaviour
{
    public (int,int) point;
    public int player; //0 for no player, 1+ for player
    public Button parentButton;

    private GameBoardManager board;
    private ColorManager colorManager;

    // Start is called before the first frame update
    private void Start()
    {
        board = FindObjectOfType<GameBoardManager>();
        colorManager = FindObjectOfType<ColorManager>();
        parentButton.image.sprite = colorManager.playerSprites[0];
        parentButton.image.color = colorManager.playerColors[0];
        player = 0;
    }

    public void SetCoords((int,int) newPoint)
    {
        point = newPoint;
    }

    public void SetPlayer(int playerNum)
    {
        player = playerNum;
        parentButton.image.sprite = colorManager.playerSprites[playerNum];
    }

    //this method is what is called when the button is pressed
    public void ClickDot()
    {
        board.ChooseDot(point);
        
        //once the dot is chosen, make it non-interactable
        parentButton.interactable = false;
    }
}
