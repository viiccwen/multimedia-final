using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
       
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public int getScore() { return score; }
    public void setScore(int score) { 
        this.score = score;
        scoreText.text = "Score: " + score;
    }
    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }
}
