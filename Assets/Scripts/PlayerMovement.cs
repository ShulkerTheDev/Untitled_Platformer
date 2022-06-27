using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] float playerSpd = 10f;
    [SerializeField] float jumpSpd = 5f;
    [SerializeField] float wallSlidingSpd = 3f;
    [SerializeField] float wallJumpSpd =2f;

    Vector2 moveInput;
    Rigidbody2D playerBody;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
      //Retrieves rigidbody2d component
      playerBody = GetComponent<Rigidbody2D>();   
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FlipSprite();
        Falling();
        WallSliding();
    }

    void OnMove(InputValue value)
    {
      //Takes the vector value of the player input when OnMove is called in unity
      moveInput = value.Get<Vector2>();
      Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
      //Leaves method if player is not touching a layer labeled "Ground"
      if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        return;
      }

      if(value.isPressed && playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        playerBody.velocity += new Vector2(0f, jumpSpd);

        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isRunning", false);
      }

      Debug.Log("Jump");
    }

    void Movement()
    {
      if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
      {
        //Controls player movement
        Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpd, playerBody.velocity.y);
        playerBody.velocity = playerVelocity;

        bool playerRunning = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;

        playerAnimator.SetBool("isRunning", playerRunning);
      }

    }

    void FlipSprite()
    {
      //Value is true if the velocity player is greater than Mathf.Epsilon
      bool playerMovingHori = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;

      if(playerMovingHori) 
      { //Flips player's sprite if bool is true
        transform.localScale = new Vector3 (Mathf.Sign(playerBody.velocity.x), 1f);
      }
    }

    void Falling()
    { //Detect if player is falling
      if (playerBody.velocity.y < -0.1)
      {
        playerAnimator.SetBool("isFalling", true);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isRunning", false);
      }
      else
      {
        playerAnimator.SetBool("isFalling", false);
      }
    }

    void WallSliding()
    {
      //Detects if player is wall sliding
      if(!playerCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
      {
        playerAnimator.SetBool("isWallSliding", false);
      }
      
      if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Wall")) && !playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        playerBody.velocity = new Vector2(playerBody.velocity.x, Mathf.Clamp(playerBody.velocity.y, -wallSlidingSpd, float.MaxValue));

        playerAnimator.SetBool("isWallSliding", true);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isRunning", false);
      }
      
    }
}
