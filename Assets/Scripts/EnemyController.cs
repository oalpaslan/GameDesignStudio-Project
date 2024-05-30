using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float leftPoint, rightPoint, upPoint, downPoint;

    private float initialPos;

    private bool movingRight;

    private Rigidbody2D rBody;
    public SpriteRenderer spriteRenderer;

    public int health;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        initialPos = transform.position.x;
        movingRight = true;
    }

    void Update()
    {
        if (movingRight)
        {
            rBody.velocity = new Vector2(moveSpeed, rBody.velocity.y);
            if (transform.position.x > initialPos + rightPoint)
            {
                movingRight = false;
                spriteRenderer.flipX = false;
            }
        }
        else
        {

            rBody.velocity = new Vector2(-moveSpeed, rBody.velocity.y);
            if (transform.position.x < initialPos - leftPoint)
            {
                movingRight = true;
                spriteRenderer.flipX = true;

            }

        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the enemy or play death animation
        Destroy(gameObject);
    }
}
