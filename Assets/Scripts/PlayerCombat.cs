using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float playerHealth = 1000f;
    [SerializeField] float hitDelay = 0.3f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 2f);
    [SerializeField] float attackDuration = 0.30f;

    Animator playerAnimator;
    BoxCollider2D playerCollider;
    Rigidbody2D playerBody;

    float attackTimer = 0f;
    float invulerableTimer =  0f;
    bool playerAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<BoxCollider2D>();
      playerBody = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
      TakeDmg();
      HealthCheck();
      AttackCheck();
    }

    void TakeDmg()
    { 
      invulerableTimer += Time.deltaTime;
      Debug.Log(invulerableTimer);
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
}
