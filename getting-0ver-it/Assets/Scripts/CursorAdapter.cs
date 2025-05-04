using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class CursorAdapter : MonoBehaviour
{
    public PlayerControl playerControl;
    public Vector2 centreOfCircle;
    public int normalizedLength;

    private Vector2 normalizedVector;

    // Start is called before the first frame update
    void Start()
    {
        normalizedVector = new Vector2(1, 0);
    }
    public void SetCentreOfCircle(int x, int y, int z = 0)
    {
        centreOfCircle.x = x;
        centreOfCircle.y = y;
    }
    public void SetCentreOfCircle(Vector2 centre)
    {
        centreOfCircle = centre;
    }
    public void SetCentreOfCircle(Vector3 centre)
    {
        centreOfCircle.x = centre.x;
        centreOfCircle.y = centre.y;
    }
    public void SetRadiusLength(int length)
    {
        if (length <= 0)
        {
            Debug.LogWarning("Invalid Length");
            return;
        }

        normalizedLength = length;
    }
    public void UpdatePosition(Vector3 position)
    {
        UpdatePosition(new Vector2(position.x, position.y));
    }
    public void UpdatePosition(Vector2 position)
    {
        Vector2 vec = position - centreOfCircle;

        // Get the radius by calculating the ratio of vector magnitude to the length of normalized vector
        // note the value of radius must be smaller than 1
        float radius = Mathf.Min(1.0f, vec.magnitude / normalizedLength);

        // Given vector A and B, we have AB = |A||B|cos(theta),
        // and we need to get theta = acos(AB/|A||B|)
        // note that vector B is actually a normalized vector
        vec = vec.normalized;
        float dotProduct = Vector2.Dot(vec, normalizedVector);
        float angle = Mathf.Acos(dotProduct);

        // Convert from radian measure to degree measure
        angle = angle * Mathf.Rad2Deg;

        // Reverse the degree while vector is on the quadrant III or quadrant IV
        if (vec.y <= 0) angle = -angle;

        Debug.Log(position.x + " " + position.y);

        playerControl.SetAngle(angle);
        playerControl.SetRadius(radius);
    }
}