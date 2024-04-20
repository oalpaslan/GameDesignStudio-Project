using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float pSpeed = 8f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float wallSlidingSpeed = 2f;

    private bool isOnGround, isOnWall;
    private bool isWallSliding;
    public Transform groundCheckPoint, wallCheckPoint;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    private bool isFacingRight = true;


    public Rigidbody2D rBody;
    public CapsuleCollider2D pCollider;
    public SpriteRenderer pRenderer;

    public float collisionRayLength = 0.5f;

    private Animator anim;

    public int curBlood, maxBlood;
    public bool soulMate;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // Start is called before the first frame update
    void Start()
    {
        pRenderer = gameObject.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rBody.velocity = new Vector2(pSpeed * Input.GetAxis("Horizontal"), rBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(rBody.velocity.x));
        anim.SetBool("IsWallSlide", isWallSliding);
        anim.SetBool("IsOnGround", isOnGround);

        isOnGround = Physics2D.OverlapCircle(groundCheckPoint.position, .05f, whatIsGround); //OverlapCircle tells if a circle in a position overlaps with another collider

        isOnWall = Physics2D.OverlapCircle(wallCheckPoint.position, .1f, whatIsWall);

        WallSlide();
        WallJump();

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
        }

        if(Input.GetAxis("Horizontal") < 0)
        {
            pRenderer.flipX = true;
            wallCheckPoint.transform.position = new Vector2(gameObject.transform.position.x - 0.2f, transform.position.y);
            wallCheckPoint.transform.rotation = new Quaternion(0, 180, 0,0);
        }
        else if(Input.GetAxis("Horizontal") > 0)
        {
            pRenderer.flipX = false;
            wallCheckPoint.transform.position = new Vector2(gameObject.transform.position.x + 0.2f, transform.position.y);
            wallCheckPoint.transform.rotation = new Quaternion(0, 0, 0, 0);

        }

    }

    private void VampireSpeed()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Object");
        float oldSpeed = pSpeed;
        if (collision.gameObject.CompareTag("Object"))
        {
            
            pSpeed = pSpeed / 3;
            //obj.transform.Translate(transform.position.x * Time.deltaTime, obj.transform.position.y, 0);
            obj.transform.position = new Vector2(obj.transform.position.x + pSpeed * Time.deltaTime * 0.001f, obj.transform.position.y);
            
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        
    }
    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;  
            wallJumpingDirection = -transform.localRotation.x;
            wallJumpingCounter = wallJumpingTime;
            
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping= true;
            rBody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                pRenderer.flipX=!pRenderer.flipX;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping=false;
    }

    private void WallSlide()
    {
        if(isOnWall && !isOnGround)
        {
            isWallSliding = true;
            rBody.velocity = new Vector2(rBody.velocity.x, 0);
        }
        else
        {
            isWallSliding = false;
        }
    }

    //private void Flip()
    //{
    //    if(isFacingRight && Input.GetAxisRaw("Horizontal") < 0f || !isFacingRight && Input.GetAxisRaw("Horizontal") > 0f)
    //    {
    //        isFacingRight = !isFacingRight;
    //        Vector3 localScale = transform.localScale;
    //        localScale.x *= -1f;
    //        transform.localScale = localScale;
    //    }
    //}
}
