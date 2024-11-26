using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.XR;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    private EnemyState enemyState, newState;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.Chasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed; 

            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);
        }  
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
           if (player == null)
           {
                player = collision.transform;
           }
           ChangeState(EnemyState.Chasing);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
       if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.idle);
        }
    }

    void ChangeState(EnemyState newState)
    {
        //exit the current animation
        if(enemyState == EnemyState.idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);

        //Update our current state
        enemyState = newState;

        //Update the new animation
        if(enemyState == EnemyState.idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);

    }
}

public enum EnemyState 
{
    idle,
    Chasing,
}