using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObject : MonoBehaviour
{
    [Header("2D-triggerable object")]
    public int score = 1;
    private Animator animator;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // while other object collide with this triggerable object at Player layer, add the score
        if(collision.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(score);

            // play the animation
            animator.SetTrigger("CoinsPickup");

            // delay 1 second to play the animation
            Destroy(gameObject, 3f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
