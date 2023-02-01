using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] float playerSpd = 10f;
    [SerializeField] float jumpSpd = 5f;
    [SerializeField] float wallSlidingSpd = 0f;
    [SerializeField] float wallJumpSpd =2f;

    Vector2 moveInput;
    Rigidbody2D playerBody;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D playerFeetCollider;
    SpriteRenderer playerSprite;

    // Start is called before the first frame update
    void Start()
    {
      //Retrieves rigidbody2d component
      playerBody = GetComponent<Rigidbody2D>();   
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<CapsuleCollider2D>();
      playerFeetCollider = GetComponent<BoxCollider2D>();
      playerSprite = GetComponent<SpriteRenderer>();
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
      bool playerWallSliding = playerAnimator.GetBool("isWallSliding");

      //Leaves method if player is not touching a layer labeled "Ground"
      if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
      {
        Debug.Log("Leaving OnJump");
        return; 
      }

      if(value.isPressed && playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        playerBody.velocity += new Vector2(0f, jumpSpd);

        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isRunning", false);
        Debug.Log("Jump");
      }

      if(value.isPressed && playerWallSliding == true)
      {
        playerBody.velocity += new Vector2(0f, -wallSlidingSpd);
        Debug.Log("WALL JUMP");

        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isWallSliding", false);
      }

    }

    void Movement()
    {
      if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        //Controls player movement
        Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpd, playerBody.velocity.y);
        playerBody.velocity = playerVelocity;

        bool playerRunning = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;

        playerAnimator.SetBool("isRunning", playerRunning);
      }

      if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && playerAnimator.GetBool("isJumping") == true)
      {
        //Controls player movement
        Vector2 playerVelocity = new Vector2 (moveInput.x * jumpSpd, playerBody.velocity.y);
        playerBody.velocity = playerVelocity;
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
      if (playerBody.velocity.y < -0.1 && playerAnimator.GetBool("isWallSliding") == false)
      {
        playerAnimator.SetBool("isFalling", true);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isWallSliding", false);
      }
      else
      {
        playerAnimator.SetBool("isFalling", false);
      }
    }

    void WallSliding()
    {
      //Detects if player is wall sliding
      if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Wall")) && !playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        Debug.Log("ON WALL");
        playerBody.velocity += new Vector2(0f, wallSlidingSpd);

        playerSprite.flipX = true;

        playerAnimator.SetBool("isWallSliding", true);
        playerAnimator.SetBool("isJumping", false);
        playerAnimator.SetBool("isRunning", false);
        playerAnimator.SetBool("isFalling", false);
      }
      else
      {
        playerAnimator.SetBool("isWallSliding", false);
      }
      
    }
}
