using System.Collections;
using UnityEngine;

public class gravityShift : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float flipCooldown = 0.5f;

    private Rigidbody2D rb;
    private bool canFlip = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleGravityFlip();
    }
    void handleMovement()
    {
        // Horizontal movement is always the same regardless of gravity
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }
    void handleGravityFlip()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canFlip)
        {
            FlipGravity();
        }
    }

    void FlipGravity()
    {
        // Flip the gravity scale (positive to negative or negative to positive)
        rb.gravityScale *= -1;

        // Rotate the player 180 degrees so they're always "standing" on surfaces
        transform.Rotate(0, 0, 180);

        // Optional: Add cooldown to prevent spamming
        StartCoroutine(FlipCooldown());

        // Optional: Add visual/audio feedback
        Debug.Log("Gravity flipped! New gravity: " + rb.gravityScale);
    }

    IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }
}
