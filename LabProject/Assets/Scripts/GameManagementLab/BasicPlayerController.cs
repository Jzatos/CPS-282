using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicPlayerController : MonoBehaviour {
    // player motion
    public float moveSpeed = 3f;
    public float jumpForce = 100f;
    private Rigidbody2D rb;
    float vx; //velocity of x-axis
    float vy; //velocity of x-axis

    public AudioClip jumpSFX;
    private AudioSource audio;

    // the layer that the player is on (setup in Awake)
    int playerLayer;
    // the layer that Platforms are on (setup in Awake)
    int platformLayer;

    //Awake is called when game starts, even before Start function
    private void Awake()
    {
        //Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) // if Rigidbody is missing
            Debug.LogError("Rigidbody2D component is missing from this gameobject");

        //Get the AudioSource component
        audio = GetComponent<AudioSource>();
        if (audio == null)
        { // if AudioSource is missing
            Debug.LogWarning("AudioSource component is missing from this gameobject. Adding one.");
            // add the AudioSource component dynamically
            audio = gameObject.AddComponent<AudioSource>();
        }
        // set up the layer for player and platform
        playerLayer = this.gameObject.layer;
        //For Platforms, player can pass thru when jumping upward, but stand on when falling downward
        platformLayer = LayerMask.NameToLayer("Platform");
    }
    private void Update()
    {
        // If the jump button is pressed --> jump
        if (Input.GetButtonDown("Jump"))
        {
            vy = 0f;// reset current vertical motion to 0 prior to jump
            // add a force to the up direction
            rb.AddForce(this.transform.up * jumpForce);
            if(jumpSFX) // play the jump sound if it's set
                audio.PlayOneShot(jumpSFX);
        }

        // Get horizontal input (x-axis) and use it as horizontal velocity
        vx = Input.GetAxisRaw("Horizontal");
        // get the current vertical velocity from the rigidbody component
        vy = rb.linearVelocity.y;

        // Change the actual velocity on the rigidbody (this is how the player gets velocity)
        rb.linearVelocity = new Vector2(vx * moveSpeed, vy);

        // if moving up then don't collide with platform layer
        // this allows the player to jump up through things on the platform layer
        // NOTE: requires the platforms to be on a layer named "Platform"
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, (vy > 0.0f));
    }
}

