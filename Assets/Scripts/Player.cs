using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // allows us to change the value of this var in any script & in our editor
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpSpeed = 23f;
    [SerializeField] float climbSpeed = 10f;
    Rigidbody2D body;
    Animator animator;
    BoxCollider2D boxCollider;
    PolygonCollider2D feet;

    float startingGravity;

    private float coyoteTime = 0.2f; // the longer the time, the longer the player can jump after
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

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

    private bool IsGrounded()
    {
        return feet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void Jump()
    {   
        bool jumpButtonDown = CrossPlatformInputManager.GetButtonDown("Jump");
        bool jumoButtonUp = CrossPlatformInputManager.GetButtonUp("Jump");

        if(IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if(jumpButtonDown)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if(coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);

            jumpBufferCounter = 0f;
        }

        if(jumoButtonUp && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
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
