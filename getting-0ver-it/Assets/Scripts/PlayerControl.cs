using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public Transform hammerHead;
    public Transform body;

    public float maxRange = 1.5f;

    // Smooth factor for hammer
    [Range(1f, 20f)] public float radiusSmoothFactor = 8f;
    [Range(1f, 20f)] public float angleSmoothFactor = 10f;

    // Current radius and angle for hammer (polar coordinates)
    private float currentRadius;
    private float currentAngle;

    // Rigidbody2D and Collider2D components
    private Rigidbody2D bodyRb;
    private Rigidbody2D hammerRb;
    private Collider2D bodyCollider;
    private Collider2D hammerCollider;

    // Start is called before the first frame update
    void Start() {
        // Get the Rigidbody2D and Collider2D components
        bodyRb = body.GetComponent<Rigidbody2D>();
        hammerRb = hammerHead.GetComponent<Rigidbody2D>();
        bodyCollider = body.GetComponent<Collider2D>();
        hammerCollider = hammerHead.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(hammerCollider, bodyCollider);

        Vector3 initialOffset = hammerHead.position - body.position;
        currentRadius = Mathf.Min(initialOffset.magnitude, maxRange);
        // Mathf.Atan2 can be used to compute the angle in radians
        // and then converted to degrees
        currentAngle = Mathf.Atan2(initialOffset.y, initialOffset.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3 relativeMousePos = mouseWorldPos     - body.position;

        float targetRadius = Mathf.Min(relativeMousePos.magnitude, maxRange);
        float targetAngle = Mathf.Atan2(relativeMousePos.y, relativeMousePos.x) * Mathf.Rad2Deg;

        // Smooth the radius and angle using Lerp
        currentRadius = Mathf.Lerp(currentRadius, targetRadius, Time.fixedDeltaTime * radiusSmoothFactor);
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.fixedDeltaTime * angleSmoothFactor);

        // Convert polar coordinates back to Cartesian coordinates
        float smoothedXOffset = currentRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float smoothedYOffset = currentRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 smoothedOffsetVector = new Vector3(smoothedXOffset, smoothedYOffset, 0);

        // Check if hammer head is collided with scene objects
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = LayerMask.GetMask("Default");
        Collider2D[] results = new Collider2D[5];
        int collisionCount = hammerRb.OverlapCollider(contactFilter, results);
        if (collisionCount > 0)  // If collided with scene objects
        {
            // Update body pos
            Vector3 targetBodyPos = hammerHead.position - smoothedOffsetVector;

            Vector3 force = (targetBodyPos - body.position) * 80.0f;
            bodyRb.AddForce(force);

            bodyRb.velocity = Vector2.ClampMagnitude(bodyRb.velocity, 6);
        }

        // Compute new hammer pos
        Vector3 newHammerPos = body.position + smoothedOffsetVector;
        Vector3 movementVector = newHammerPos - hammerHead.position;
        newHammerPos = hammerHead.position + movementVector * 0.2f;

        // Update hammer pos
        hammerRb.MovePosition(newHammerPos);

        // Update hammer rotation
        hammerHead.rotation = Quaternion.FromToRotation(Vector3.right, newHammerPos - body.position);
    }
}
