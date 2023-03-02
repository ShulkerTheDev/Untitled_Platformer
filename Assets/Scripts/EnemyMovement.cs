using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D turncapsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        turncapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        
        if(turncapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))
        {
          FlipEnemyFacing();
        }
        
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
      transform.localScale = new Vector3 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
}
