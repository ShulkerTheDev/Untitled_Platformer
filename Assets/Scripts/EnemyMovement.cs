using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Serialized field so values can be changed in editor to see what feels/flows better
    [SerializeField] float moveSpeed = 1f;

    bool enemyAttacking;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D turncapsuleCollider;
    Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        turncapsuleCollider = GetComponent<CapsuleCollider2D>();
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
      }    
      
      //if enemy touches wall and is touching the ground turn around
      if(turncapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Wall")) || turncapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
