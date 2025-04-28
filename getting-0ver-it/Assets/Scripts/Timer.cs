using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime;

    public float getElapsedTime() { return elapsedTime; }

    public void setElapsedTime(float time) { elapsedTime = time; }

    void Update()
    {
        // update newer time
        elapsedTime += Time.deltaTime;

        string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00.00");

        timerText.text = "Time: " + minutes + ":" + seconds;
    }
}
