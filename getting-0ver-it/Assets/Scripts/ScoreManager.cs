using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    public int score = 0;
       
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void AddScore(int amount)
    {
        Awake();
        score += amount;
        scoreText.text = "Score: " + score;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
