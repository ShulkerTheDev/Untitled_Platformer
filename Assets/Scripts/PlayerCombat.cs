using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float playerHealth = 1000f;
    [SerializeField] float hitDelay = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);

    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<CapsuleCollider2D>();
      playerBody = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        TakeDmg();
        HealthCheck();
    }

    void TakeDmg()
    {
      float lastHit =  0f;
      float nextHit = 1f;
      if(playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) && nextHit > lastHit)
      {
        lastHit = Time.time;
        nextHit = Time.time + hitDelay;

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
      Debug.Log(playerHealth);

      if (playerHealth <= 0)
      {
        playerAnimator.SetBool("isDying", true);
        playerBody.velocity = deathKick;

        //SceneManager.LoadScene(2);
      }
    }
}
