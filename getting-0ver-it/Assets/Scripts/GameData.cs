using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float time;
    public int score;
    public float[] pos;

    public GameData (Transform playerPos, Timer timer, ScoreManager scoreManager)
    {
        time = timer.getElapsedTime();
        score = scoreManager.getScore();
        pos = new float[3];
        pos[0] = playerPos.position.x;
        pos[1] = playerPos.position.y;
        pos[2] = playerPos.transform.position.z;
    }
}
