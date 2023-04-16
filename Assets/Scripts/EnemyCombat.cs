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
    [SerializeField] float attackDmg = 5f;

    float lastAttackTime = 0f;
    
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
      HealthCheck();
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
      Vector2 distanceToPlayer = playerLocation.position - transform.position;
      distanceToPlayer.y = 0f;
      distanceToPlayer.Normalize();

      // Calculate the dot product between the enemy's forward direction and the direction to the player
      float dotProduct = Vector2.Dot(transform.forward, distanceToPlayer);

      // If the player is infront of player and within the attack range and delay time has passed since the last attack
      if (dotProduct > 0.5f && distanceToPlayer.magnitude <= attackRange && Time.time - lastAttackTime >= attackDelay)
      {
          // Set the IsAttacking parameter to true to play the attack animation
          enemyAnimator.SetBool("IsAttacking", true);

          // Attack the player
          playerObject.GetComponent<PlayerCombat>().TakeDmg(attackDmg);

          // Update the last attack time
          lastAttackTime = Time.time;
      }
      else
      {
          // Set the IsAttacking parameter to false to stop the attack animation
          enemyAnimator.SetBool("IsAttacking", false);
      }
    }
}
