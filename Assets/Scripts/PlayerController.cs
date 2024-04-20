using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float pSpeed = 8f;
    [SerializeField]
    private float jumpForce = 5f;

    private bool isOnGround;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;


    public Rigidbody2D rBody;

    public SpriteRenderer pRenderer;

    public float collisionRayLength = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        pRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollision();
        rBody.velocity = new Vector2(pSpeed * Input.GetAxis("Horizontal"), rBody.velocity.y);

        isOnGround = Physics2D.OverlapCircle(groundCheckPoint.position, .05f, whatIsGround); //OverlapCircle tells if a circle in a position overlaps with another collider


        if (Input.GetButtonDown("Jump") && isOnGround)
        {

            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);

        }

        if(Input.GetAxis("Horizontal") < 0)
        {
            pRenderer.flipX = true;
        }
        else if(Input.GetAxis("Horizontal") > 0)
        {
            pRenderer.flipX = false;
        }

    }

    private void DetectCollision()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, collisionRayLength);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, collisionRayLength);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, collisionRayLength);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, collisionRayLength);

        if (hitLeft.collider != null && hitLeft.collider.CompareTag("Wall"))
        {
            Debug.Log("Collided with left side of the tile");
        }

        if (hitRight.collider != null && hitRight.collider.CompareTag("Wall"))
        {
            Debug.Log("Collided with right side of the tile");
        }

        if (hitUp.collider != null && hitUp.collider.CompareTag("Wall"))
        {
            Debug.Log("Collided with top side of the tile");
        }

        if (hitDown.collider != null && hitDown.collider.CompareTag("Wall"))
        {
            Debug.Log("Collided with bottom side of the tile");
        }
    }
}
