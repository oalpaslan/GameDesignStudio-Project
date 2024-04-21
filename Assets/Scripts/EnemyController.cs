using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform leftPoint, rightPoint;

    private bool movingRight;

    private Rigidbody2D rBody;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();


        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;
    }

    void Update()
    {
        if (movingRight)
        {
            rBody.velocity = new Vector2(moveSpeed, rBody.velocity.y);

            if (transform.position.x > rightPoint.position.x)
            {
                movingRight = false;
                spriteRenderer.flipX = false;
            }
        }
        else
        {

            rBody.velocity = new Vector2(-moveSpeed, rBody.velocity.y);
            if (transform.position.x < leftPoint.position.x)
            {
                movingRight = true;
                spriteRenderer.flipX = true;

            }

        }
    }
}
