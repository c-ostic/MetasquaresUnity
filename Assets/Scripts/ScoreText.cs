using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for temporary score indicators to show up above the board
/// After appearing, they slowly fade away
/// </summary>
public class ScoreText : MonoBehaviour
{
    public Image panel;
    public TMP_Text text;
    public float fadeMultiplier;
    public float waitTime;
    public float timeSoFar;

    /// <summary>
    /// Called every frame
    /// Makes the text slowly disappear until destroyed completely
    /// </summary>
    void Update()
    {
        timeSoFar += Time.deltaTime;

        if (timeSoFar > waitTime)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - (Time.deltaTime / fadeMultiplier));
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeMultiplier));
        }

        if (text.color.a <= 0)
            Destroy(gameObject);
    }
}
