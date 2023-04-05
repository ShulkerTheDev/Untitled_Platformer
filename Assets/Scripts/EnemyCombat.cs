using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] float enemyHealth = 100f;
    [SerializeField] float hitDelay = 0.3f;
    [SerializeField] Vector2 knockBack = new Vector2(0f, 1f);

    //float attackTimer = 0f;
    float invulerableTimer =  0f;

    Animator enemyAnimator;
    BoxCollider2D enemyCollider;
    Rigidbody2D enemyBody;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyBody = GetComponent<Rigidbody2D>();
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
}
