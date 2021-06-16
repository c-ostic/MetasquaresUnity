using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    //0 is the default, while 1+ represents player number
    public Sprite[] playerSprites;
    public Color[] playerColors;

    [Range(0,255)]
    public int fillOpacity;
}
