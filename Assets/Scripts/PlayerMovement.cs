using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Serialized field so values can be changed in editor to see what feels/flows better
    [SerializeField] float playerSpd = 10f;
    [SerializeField] float jumpSpd = 5f;
    [SerializeField] float wallSlidingSpd = 0f;
    [SerializeField] float fallDmg = 25f;
    //[SerializeField] float wallJumpSpd =2f;

    Vector2 moveInput;
    Vector2 safeLocation;
    Rigidbody2D playerBody;
    Animator playerAnimator;
    CapsuleCollider2D playerFeetCollider;
    BoxCollider2D playerCollider;
    SpriteRenderer playerSprite;
    PlayerInputSystem playerInput;
    PlayerCombat playerCombat;

    // Start is called before the first frame update
    void Awake()
    {
      //Retrieves Components
      playerBody = GetComponent<Rigidbody2D>();   
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<BoxCollider2D>();
      playerFeetCollider = GetComponent<CapsuleCollider2D>();
      playerSprite = GetComponent<SpriteRenderer>();
      playerInput = new PlayerInputSystem();
      playerCombat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FlipSprite();
        Falling();
        WallSliding();
        UpdatePlayerSafeLocation();
    }

    public void Jump()
    {
      bool playerWallSliding = playerAnimator.GetBool("isWallSliding");

      //Leaves method if player is not touching a layer labeled "Ground", if they aren't touching the ground player can't jump
      if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
      {
        return; 
      }

      //Cheeks if collider for player's feet is touching the ground
      if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        playerBody.velocity += new Vector2(0f, jumpSpd);

        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isRunning", false);
      }

      if(playerWallSliding == true)
      {
        playerBody.velocity += new Vector2(0f, -wallSlidingSpd);
        Debug.Log("WALL JUMP");

        playerAnimator.SetBool("isJumping", true);
        playerAnimator.SetBool("isWallSliding", false);
      }

    }

    public void Movement()
    {
      moveInput = playerInput.Player_Input.Movement.ReadValue<Vector2>();

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

    private void UpdatePlayerSafeLocation()
    {
      if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        safeLocation = playerBody.transform.position;
        Debug.Log(safeLocation);
      }      

    }

    //Enable player input
    private void OnEnable() 
    {
      playerInput.Player_Input.Enable(); 
    }

    private void OnDisable()
    {
      playerInput.Player_Input.Disable();
    }

    private void TakeFallDmg()
    {
      playerCombat.currentPlayerHealth = playerCombat.currentPlayerHealth - fallDmg;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
       if (other.gameObject.name == "Tilemap Grid")
       {
          TakeFallDmg();

          if(playerCombat.playerLives > 1 || playerCombat.currentPlayerHealth > 0)
          {
            playerBody.transform.position = safeLocation;
          }
       }
    }
}
