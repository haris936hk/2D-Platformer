using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed =5f;
    public float runSpeed =8f;
    public float airWalkSpeed = 3f;
    private float jumpImpulse= 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;

  public float CurrentMoveSpeed
{
    get
    {
        if(CanMove){
        // Check if the player is moving
        if (IsMoving)
        {
            // If touching the wall and moving towards it, halt or reduce speed
            if (touchingDirections.IsOnWall && moveInput.x != 0)
            {
                return 0; // You can set a reduced speed here if desired
            }

            // If the player is not grounded, use airWalkSpeed
            if (!touchingDirections.IsGrounded)
            {
                return airWalkSpeed;
            }

            // Default movement: running or walking
            return IsRunning ? runSpeed : walkSpeed;
        }
        else
        {
            return 0; // Not moving
        }
        }
        else{
            return 0;
        }
    }
}



    [SerializeField]
    public bool _isMoving = false;

    public bool IsMoving { get
    {
    return _isMoving; 
    }   
    private set
    {
        _isMoving =value;
        animator.SetBool(AnimationStrings.isMoving,value);
    }
    }

    [SerializeField]
    private bool _isRunning=false;

    public bool IsRunning{
        get{
            return _isRunning;
        }
        set{
            _isRunning=value;;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight { get{ return _isFacingRight;} private set{
        if(_isFacingRight!=value){
            transform.localScale*= new Vector2(-1,1);
        }
        _isFacingRight=value;
    } }

    public bool CanMove{ get
    {
        return animator.GetBool(AnimationStrings.canMove);
    }
    }

    Rigidbody2D rb;

    Animator animator;
    

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator =GetComponent<Animator>();
        touchingDirections =GetComponent<TouchingDirections>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate(){
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

public void OnMove(InputAction.CallbackContext context){
moveInput = context.ReadValue<Vector2>();
IsMoving = moveInput != Vector2.zero;

SetFacingDirection(moveInput);
}

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight){
            IsFacingRight= true;
        }
        else if(moveInput.x <0 && IsFacingRight){
            IsFacingRight=false;
        }
    }

    public void OnRun(InputAction.CallbackContext context){

    if(context.started){
        IsRunning = true;
    } else if (context.canceled){
        IsRunning = false;
    }

    }
    public void OnJump(InputAction.CallbackContext context){
        // check if alive
        if(context.started && touchingDirections.IsGrounded){
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

}

