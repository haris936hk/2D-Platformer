using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public float walkSpeed= 3f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    public enum WalkableDirection{ Right, Left}
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    private bool hasFlipped = false; 
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection;}
        set{ 
            if(_walkDirection != value)
            {
                gameObject.transform.localScale=new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                } else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection= value;}
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections=GetComponent<TouchingDirections>();
    }
    

private void FixedUpdate()
{
    if (!hasFlipped && touchingDirections.IsOnWall && touchingDirections.IsGrounded)
    {
        FlipDirection();
        hasFlipped = true;
    }
    else if (!touchingDirections.IsOnWall || !touchingDirections.IsGrounded)
    {
        hasFlipped = false; // Reset the flag if conditions are not met
    }
    rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
}

    private void FlipDirection()
    {
        if(WalkDirection== WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if(WalkDirection== WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } else
        {
            Debug.LogError("Current Walkable Direction is not set to legal values of right or left");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
