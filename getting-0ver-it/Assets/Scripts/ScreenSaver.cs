using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 direction;
    private RectTransform rectTransform;
    private RectTransform parentRect;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        
        SetRandomPosition();
        
        int quadrant = Random.Range(0, 4);
        direction = new Vector2(
            quadrant < 2 ? 1f : -1f,
            quadrant % 2 == 0 ? 1f : -1f
        ).normalized;
    }

    private void SetRandomPosition()
    {
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;
        float objectWidth = rectTransform.rect.width;
        float objectHeight = rectTransform.rect.height;

        float maxX = parentWidth/2 - objectWidth/2;
        float maxY = parentHeight/2 - objectHeight/2;
        float minX = -maxX;
        float minY = -maxY;

        rectTransform.anchoredPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );
    }

    void Update()
    {
        float multiplier = 100f;
        Vector2 newPosition = rectTransform.anchoredPosition + direction * moveSpeed * Time.deltaTime * multiplier;
        
        CheckBoundaryCollision(ref newPosition);
        
        rectTransform.anchoredPosition = newPosition;
    }

    private void CheckBoundaryCollision(ref Vector2 position)
    {
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;
        
        float objectWidth = rectTransform.rect.width;
        float objectHeight = rectTransform.rect.height;
        
        if (position.x + objectWidth/2 > parentWidth/2 || position.x - objectWidth/2 < -parentWidth/2)
        {
            direction.x *= -1;  // 水平反射
        }
        
        if (position.y + objectHeight/2 > parentHeight/2 || position.y - objectHeight/2 < -parentHeight/2)
        {
            direction.y *= -1;  // 垂直反射
        }
        
        position.x = Mathf.Clamp(position.x, -parentWidth/2 + objectWidth/2, parentWidth/2 - objectWidth/2);
        position.y = Mathf.Clamp(position.y, -parentHeight/2 + objectHeight/2, parentHeight/2 - objectHeight/2);
    }
}
