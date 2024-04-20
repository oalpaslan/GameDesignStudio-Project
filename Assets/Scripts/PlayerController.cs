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
    // Start is called before the first frame update
    void Start()
    {
        pRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
