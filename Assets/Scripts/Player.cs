using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // allows us to change the value of this var in any script & in our editor
    [SerializeField] float playerSpeed = 10f;
    Rigidbody2D body;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // getting references
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
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
