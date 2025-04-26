using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObject : MonoBehaviour
{
    [Header("2D-triggerable object")]
    public int score = 1;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // while other object collide with this triggerable object at Player layer, add the score
        if(collision.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(score);
            Destroy(gameObject);
        }
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
