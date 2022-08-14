using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // allows us to change the value of this var in any script & in our editor
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 8f;
    Rigidbody2D body;
    Animator animator;
    BoxCollider2D boxCollider;
    PolygonCollider2D feet;
    public float lowJumpMultiplier = 2f;

    float startingGravity;

    // Start is called before the first frame update
    void Start()
    {
        // getting references
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        feet = GetComponent<PolygonCollider2D>();

        startingGravity = body.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        Climbing();
    }

    private void Climbing()
    {
        bool isClimbing = body.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");

        if(controlThrow > Mathf.Epsilon && isClimbing)
        {
            Vector2 climbVelocity = new Vector2(body.velocity.x, controlThrow * climbSpeed);
            body.velocity = climbVelocity;

            body.gravityScale = 0f;
            animator.SetBool("Climbing", true);
        }
        else
        {
            body.gravityScale = startingGravity; 
            animator.SetBool("Climbing", false);
        }

    }

    private void Jump()
    {
        if(!feet.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}
        
        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");

        if(isJumping)
        {
            Vector2 jumpVelocity = new Vector2(body.velocity.x, jumpSpeed);
            body.velocity = jumpVelocity;
        }
        
    }

    private void Run()
    {
        // contains Horizontal axis value changes
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 playerVelocity = new Vector2(controlThrow * playerSpeed, body.velocity.y);
        body.velocity = playerVelocity;

        // changes our sprite's direction
        FlipSprite();
        RunningState();
    }

    private void RunningState()
    {
        bool isRunning = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;

        // updates animation if T or F
        animator.SetBool("Running", isRunning);
    }

    // flip sprite when velocity, t.f. direction, is negative
    private void FlipSprite()
    {
        // checking if moving in general
        bool isRunning = Mathf.Abs(body.velocity.x) > Mathf.Epsilon; // T or F value is bigger than smallest possible value (if F, not moving)
        
        if(isRunning)
            transform.localScale = new Vector2(Mathf.Sign(body.velocity.x), 1f); // will affect our flip if velocity is in neg direction
    }
}
