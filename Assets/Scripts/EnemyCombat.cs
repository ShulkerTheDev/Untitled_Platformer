using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] float enemyHealth = 100f;
    [SerializeField] float hitDelay = 0.3f;
    [SerializeField] Vector2 knockBack = new Vector2(0f, 1f);
    [SerializeField] float attackTimer = 0f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float invulerableTimer =  0f;
    [SerializeField] float attackDmg = 10f;
    [SerializeField] float knockbackForce = 10f;

    float lastAttackTime = 0f;
    bool enemyAttacking = false;
    
    Animator enemyAnimator;
    BoxCollider2D enemyCollider;
    Rigidbody2D enemyBody;
    GameObject playerObject;
    Transform playerLocation;
    PlayerCombat playerHealth;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyBody = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
      enemyAttacking = enemyAnimator.GetBool("isAttacking");

      HealthCheck();
      AttackCheck();
    }

    //Checks when last enemy was hit, deals dmg and allows a delay inbetween
    public void TakeDmg(float damage)
    { 
      invulerableTimer += Time.deltaTime;
      if(invulerableTimer > hitDelay)
      {
        invulerableTimer = 0;
        enemyHealth = enemyHealth - damage;

        enemyAnimator.SetBool("isHit", true);
        
      }
      else
      {
        enemyAnimator.SetBool("isHit", false);
      }
      
    }

    //Checks to see if player health is 0 
    void HealthCheck()
    {
      if (enemyHealth <= 0)
      {
        Destroy(gameObject);
      }
    }

    void AttackCheck ()
    {
      playerLocation = playerObject.transform;
      //Calculated distance between player and enemy
      Vector2 directionToPlayer = playerLocation.position - transform.position;

      //Sets y axis to 0 as for now enemies only move left and right, so we don't need the y axis
      directionToPlayer.y = 0f;

      //Returns x axis as a positive value only
      directionToPlayer.x = Math.Abs(directionToPlayer.x);

      float  distanceToPlayer = directionToPlayer.magnitude;

      attackTimer = Time.time;

      float frontDistanceToPlayer = Vector2.Dot(transform.right, directionToPlayer.normalized);
  

      // If the player is infront of player and within the attack range and delay time has passed since the last attack
      if (distanceToPlayer <= attackRange && (attackTimer - lastAttackTime) >= attackDelay)
      {
          // Set the IsAttacking parameter to true to play the attack animation
          enemyAnimator.SetBool("isAttacking", true);

          // Attack the player
          playerObject.GetComponent<PlayerCombat>().TakeDmg(attackDmg);

          // Update the last attack time
          lastAttackTime = Time.time;
      }
      else
      {
          // Set the IsAttacking parameter to false to stop the attack animation
          enemyAnimator.SetBool("isAttacking", false);
      }
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
      if ((enemyAttacking == true) && (other.tag == "Player"))
      {
        //Retrieves the EnemyCombat script attached to an enemy
        PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
        Rigidbody2D playerRigidBody = other.GetComponent<Rigidbody2D>();

        if (playerCombat != null)
        {
          playerCombat.TakeDmg(attackDmg);

          // Calculate knockback force direction
          Vector3 knockbackDirection = other.transform.position - gameObject.transform.position;
          knockbackDirection.Normalize();

          if (playerRigidBody != null)
          {
            playerRigidBody.AddForce(knockbackDirection * knockbackForce);
          }
        }

      }
    }
}
