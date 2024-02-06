using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        UpdateHighScoreText();
    }

    void UpdateHighScoreText()
    {
        // Получаем рекорд и отображаем его
        int highScore = HighScoreManager.instance.GetHighScore();
        highScoreText.text = "Max Score: " + highScore;
    }
}
