using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField] float enemySpeed = 4f;

    Rigidbody2D rb;
    Animation animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFacingLeft())
        {
            rb.velocity = new Vector2(-enemySpeed, 0f);
        }
        else
            rb.velocity = new Vector2(enemySpeed, 0f);
    }

    // when another object leaves a trigger collider attached to this object 
    private void OnTriggerExit2D(Collider2D other)
    {
        FlipSprite();
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    private bool IsFacingLeft()
    {
        return transform.localScale.x > 0;
    }

}

