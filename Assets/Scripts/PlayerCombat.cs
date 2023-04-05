using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float playerHealth = 100f;
    [SerializeField] float hitDelay = 0.3f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 2f);
    [SerializeField] float attackDuration = 0.30f;
    [SerializeField] float attackDmg = 10f;
    [SerializeField] float knockbackForce = 10f;

    Animator playerAnimator;
    BoxCollider2D playerCollider;
    Rigidbody2D playerBody;
    CircleCollider2D attackCollider;

    float attackTimer = 0f;
    float invulerableTimer =  0f;
    bool playerAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<BoxCollider2D>();
      playerBody = GetComponent<Rigidbody2D>();   
      attackCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
      TakeDmg();
      HealthCheck();
      AttackCheck();
    }

    //Checks when last player was hit, deals dmg and allows a delay inbetween
    void TakeDmg()
    { 
      invulerableTimer += Time.deltaTime;

      if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) && invulerableTimer > hitDelay)
      {
        invulerableTimer = 0;
        playerAnimator.SetBool("isHit", true);

        playerHealth = playerHealth - 0.5f;
        
      }
      else
      {
        playerAnimator.SetBool("isHit", false);
      }
      
    }

    //Checks to see if player health is 0 
    void HealthCheck()
    {
      if (playerHealth <= 0)
      {
        playerAnimator.SetBool("isDying", true);
        //playerBody.velocity = deathKick;

        //SceneManager.LoadScene(2);
      }
    }

    public void  LightAttack ()
    {
        playerAnimator.SetBool("isAttacking", true);
    }

    //Checks when the player last attacked and puts an delay inbetween attacks
    void AttackCheck()
    {
      playerAttacking = playerAnimator.GetBool("isAttacking");
      if(playerAttacking == true)
      {        
        attackTimer += Time.deltaTime;

        if (attackTimer > attackDuration)
        {
          attackTimer = 0;
          playerAnimator.SetBool("isAttacking", false);
        }
      }
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
      if ((playerAttacking == true) && (other.tag == "Attackable"))
      {
        //Retrieves the EnemyCombat script attached to an enemy
        EnemyCombat enemyCombat = other.GetComponent<EnemyCombat>();
        Rigidbody2D enemyRigidBody = other.GetComponent<Rigidbody2D>();

        if (enemyCombat != null)
        {
          enemyCombat.TakeDmg(attackDmg);

          // Calculate knockback force direction
          Vector3 knockbackDirection = other.transform.position - gameObject.transform.position;
          knockbackDirection.Normalize();

          if (enemyRigidBody != null)
          {
            enemyRigidBody.AddForce(knockbackDirection * knockbackForce);
          }
        }

      }
    }
}
