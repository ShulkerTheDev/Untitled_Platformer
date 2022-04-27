using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] float playerSpd = 10f;
    [SerializeField] float jumpSpd = 5f;

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

      if(value.isPressed)
      {
        playerBody.velocity += new Vector2(0f, jumpSpd);

        playerAnimator.SetBool("isJumping", true);
      }
    }

    void Movement()
    {
      //Controls player movement
      Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpd, playerBody.velocity.y);
      playerBody.velocity = playerVelocity;

      bool playerRunning = Mathf.Abs(playerBody.velocity.x) > Mathf.Epsilon;

      playerAnimator.SetBool("isRunning", playerRunning);
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
      }
      else
      {
        playerAnimator.SetBool("isFalling", false);
      }
    }
}
