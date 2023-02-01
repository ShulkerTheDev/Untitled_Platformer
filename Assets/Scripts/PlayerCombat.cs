using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float playerHealth = 1000f;
    [SerializeField] float hitDelay = 5f;

    Animator playerAnimator;
    CapsuleCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
      playerAnimator = GetComponent<Animator>();
      playerCollider = GetComponent<CapsuleCollider2D>();
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
        SceneManager.LoadScene(2);
      }
    }
}
