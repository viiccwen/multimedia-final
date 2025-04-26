using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private bool isRunning = true;
    private float startTime;
    private float prevTime;
    void Start()
    {
        // initialize time
        startTime = Time.time;
        prevTime = startTime;
    }

    void Update()
    {
        if (!isRunning) return;

        // update newer time
        float elapsedTime = Time.time - startTime;

        string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00.00");

        timerText.text = "Time: " + minutes + ":" + seconds;
    }
    public void PauseTimer()
    {
        isRunning = false;
        prevTime = Time.time - startTime;
    }
    public void ResumeTimer()
    {
        isRunning = true;
        startTime = prevTime;
    }
}
