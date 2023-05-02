using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    //Serialized field so values can be changed in editor to see what feels/flows better
    [SerializeField] float playerHealth = 100f;
    [SerializeField] float hitDelay = 0.3f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 2f);
    [SerializeField] float attackDuration = 0.30f;
    [SerializeField] float attackDmg = 10f;
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float attackTimer = 0f;
    [SerializeField] float invulerableTimer =  0f;
    [SerializeField] float HazardDmg = 0.5f;


    Animator playerAnimator;
    BoxCollider2D playerCollider;
    Rigidbody2D playerBody;
    CircleCollider2D attackCollider;

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
      HealthCheck();
      AttackCheck();
      HazardCollideCheck();
    }

    //Checks when last player was hit, deals dmg and allows a delay inbetween
    public void TakeDmg(float damage)
    { 
      invulerableTimer += Time.deltaTime;

      if(invulerableTimer > hitDelay)
      {
        invulerableTimer = 0;
        playerAnimator.SetBool("isHit", true);

        playerHealth = playerHealth - damage;
        
      }
      else
      {
        playerAnimator.SetBool("isHit", false);
      }
      
    }

    //Checks if player has collided with with any object marked as a hazard and deals damage
    void HazardCollideCheck ()
    {
      if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) || playerCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
      {
        TakeDmg(HazardDmg);
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

    //Attack method set to a button in the input system
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

    //Triggers when something enters a collider marked as trigger
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
