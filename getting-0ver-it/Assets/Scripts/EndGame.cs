using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;

    private void EndGameFunction()
    {
        Time.timeScale = 0;
        endGamePanel.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("EndGameFunction", 0.5f);
        }
    }
}
