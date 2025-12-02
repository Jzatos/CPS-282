using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    //moveSpeed, used for player movement control
    [Range(0.0f, 10.0f)] // create a slider in the editor in order to set moveSpeed
    public float moveSpeed = 3f;//3 will be the default. Can be set using the slider
    public float jumpForce = 600f;

    // LayerMask to determine what is considered ground for the player
    public LayerMask whatIsGround;
    // Transform just below feet for checking if player is grounded
    public Transform groundCheck;

    // A flag denoting whether the player can move or not
    // Make it public so other scripts can access it but we don't want to show in editor as it might confuse designer
    [HideInInspector]
    public bool playerCanMove = true;

    // player motion
    float vx;
    float vy;

    // player physical state tracking
    public bool facingRight = true;
    bool isGrounded = false;
    bool isWalking = false;

    // SFXs
    public AudioClip deathSFX;
    public AudioClip fallSFX;
    public AudioClip jumpSFX;
    public AudioClip victorySFX;

    // Private variables for storing references to components of the gameObject
    private Transform transform;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audio;

    // the layer that the player is on (setup in Awake)
    int playerLayer;
    // the layer that Platforms are on (setup in Awake)
    int platformLayer;

    // Awake is called before Start(), for variables initialization
    void Awake()
    {
        // get a reference to corresponding components 
        transform = GetComponent<Transform>();

        rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody == null) // if Rigidbody is missing
            Debug.LogError("Rigidbody2D component is missing from this gameobject");

        animator = GetComponent<Animator>();
        if (animator == null) // if Animator is missing
            Debug.LogError("Animator component is missing from this gameobject");

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame (i.e. always  executed repeatedly during the game play)
    void Update()
    {
        // exit  if player cannot move or game is paused
        if (!playerCanMove || (Time.timeScale == 0f))
            return;
        
        // Get horizontal input (x-axis) and use it as horizontal velocity
        vx = Input.GetAxisRaw("Horizontal");

        // Determine whether is running based on the horizontal movement
        if (vx != 0)      
            isWalking = true;
        else
            isWalking = false;
        // set the Running animation state (This is how the user input affects the animation)
        animator.SetBool("isWalking", isWalking);
        
        
        // get the current vertical velocity from the rigidbody component
        vy = rigidbody.velocity.y;
        /* Use Linecast function to check whether the player is grounded
         * The invisible line will be casted from the middle of the player down to the groundCheck position
         * If this line intersects any GameObjects on the whatIsGround layer, then we know the player is grounded*/
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);

            // Set the grounded animation states. Without this communication, the animator does not know
        animator.SetBool("isGrounded", isGrounded);

        // If the player is grounded AND jump button is pressed --> jump
        if (isGrounded && Input.GetButtonDown("Jump")) 
            DoJump();
        if (isGrounded && ! Input.GetButtonDown("Jump"))
            animator.SetBool("isJumping", false);

        // If the player stops jumping (Jump button is released) and player is not yet falling
        // --> set the vertical velocity to 0 so that he will start to fall due to gravity
        if (Input.GetButtonUp("Jump") && vy > 0f)
            vy = 0f;

        // Change the actual velocity on the rigidbody (this is how the player gets velocity)
        rigidbody.velocity = new Vector2(vx * moveSpeed, vy);

        // if moving up then don't collide with platform layer
        // this allows the player to jump up through things on the platform layer
        // NOTE: requires the platforms to be on a layer named "Platform"
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, (vy > 0.0f));
    }

    // Checking to see if the sprite should be flipped (left <--|-->right)
    // this is done in LateUpdate since the Animator may override the localScale
    // this code will flip the player even if the animator is controlling scale
    void LateUpdate()
    {
        // get the current scale
        Vector3 localScale = transform.localScale;

        if (vx > 0) // vx > 0 means moving to the right --> is facing right
            facingRight = true;
        else if (vx < 0) //moving left --> facing left
            facingRight = false;

        // if the actual direction (in localScale) is not the same as the flag facingRight denotes,
        // Flip it by multiplying -1 to x
        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        // update the scale
        transform.localScale = localScale;
    }

    // When the player collides with other game object
    void OnCollisionEnter2D(Collision2D otherObject)
    {
        //If the other object is tagged as MovingPlatform
        if (otherObject.gameObject.tag == "MovingPlatform")
            //set itself as a child of the other object so it would move with the other object
            this.transform.parent = otherObject.transform;//Go for a ride!
    }
    // When the player is done colliding with other object 
    void OnCollisionExit2D(Collision2D other)
    {
        //If the other object is MovingPlatform (i.e., moved away from platform)
        if (other.gameObject.tag == "MovingPlatform")
            this.transform.parent = null;//unchild it 
    }
    /*The jump action
     * The jumping is done using AddForce funtion which is not affected by velocity */
    void DoJump()
    {
        animator.SetBool("isJumping", true);

        vy = 0f;// reset current vertical motion to 0 prior to jump        
        rigidbody.AddForce(this.transform.up * jumpForce);// add a force to the up direction
        // play the jump sound
        audio.PlayOneShot(jumpSFX);
    }
    // Do whatever needed so as to freeze the player
    void FreezeMotion()
    {
        playerCanMove = false;
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.isKinematic = true;
    }
    // unfreeze the player
    void UnFreezeMotion()
    {
        playerCanMove = true;
        rigidbody.isKinematic = false;
    }
}
