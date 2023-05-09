using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Serialized field so values can be changed in editor to see what feels/flows better
    [SerializeField] float moveSpeed = 1f;

    bool enemyAttacking;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D turnCapsuleCollider;
    BoxCollider2D boxCapsuleCollider;
    Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        turnCapsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCapsuleCollider = GetComponent<BoxCollider2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      //Checks if enemy is attacking
      enemyAttacking = enemyAnimator.GetBool("isAttacking");


      //if enemy isn't attacking move enermy
      if(enemyAttacking == false)
      {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        enemyAnimator.SetBool("isMoving", true);
      }    
      
      //if front of enemy touches wall or the ground turn around
      if(turnCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Wall")) || turnCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
      {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
      }
        
    }

    //Flips direction enemy is facing
    void FlipEnemyFacing()
    {
      transform.localScale = new Vector3 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
}
