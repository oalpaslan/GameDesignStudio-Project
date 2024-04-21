using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float pSpeed = 8f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float wallSlidingSpeed = 2f;


    [SerializeField]
    private float maxDoorHeight;

    [SerializeField]
    private float minHeightBeforeDeath;

    [SerializeField]
    private float bloodAmount;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float spendBlood;

    private bool isButtonActive = false;
    public Rigidbody2D rBody;
    public CapsuleCollider2D pCollider;
    public SpriteRenderer pRenderer;
    private Animator anim;

    //Wall Slide and Jump

    public Transform groundCheckPoint, wallCheckPoint;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    private bool isOnGround, isOnWall;
    private bool isWallSliding;
    private bool isWallJumping;

    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    void Start()
    {
        pRenderer = gameObject.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        pSpeed = 5f;

        Movement();

        isOnGround = Physics2D.OverlapCircle(groundCheckPoint.position, .05f, whatIsGround); //OverlapCircle tells if a circle in a position overlaps with another collider

        isOnWall = Physics2D.OverlapCircle(wallCheckPoint.position, .1f, whatIsWall);

        WallSlide();
        WallJump();

        OpenDoor();

        if (gameObject.transform.position.y < minHeightBeforeDeath)
            Reset();
    }

    private void Movement()
    {

        rBody.velocity = new Vector2(pSpeed * Input.GetAxis("Horizontal"), rBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(rBody.velocity.x));
        anim.SetBool("IsWallSlide", isWallSliding);
        anim.SetBool("IsOnGround", isOnGround);

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            pRenderer.flipX = true;
            wallCheckPoint.transform.position = new Vector2(gameObject.transform.position.x - 0.2f, transform.position.y);
            wallCheckPoint.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            pRenderer.flipX = false;
            wallCheckPoint.transform.position = new Vector2(gameObject.transform.position.x + 0.2f, transform.position.y);
            wallCheckPoint.transform.rotation = new Quaternion(0, 0, 0, 0);

        }
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Object");
        Rigidbody2D rBodyObj = obj.GetComponent<Rigidbody2D>();

        if (collision.gameObject.CompareTag("Object"))
        {
            rBodyObj.isKinematic = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            bloodAmount -= damage;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Object");
        Rigidbody2D rBodyObj = obj.GetComponent<Rigidbody2D>();
        rBodyObj.isKinematic = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            Debug.Log("tru");

            isButtonActive = true;
        }
        else if (collision.gameObject.CompareTag("Soulmate"))
        {
            //Trigger dialogue
            //Use Panel
            //GameObject.FindGameObjectWithTag("SoulmateDialogue")
            //    {

            //}
        }
        else if (collision.gameObject.CompareTag("ArcheologistDialogue"))
        {

        }
        else if (collision.gameObject.CompareTag("ArcheologistDialogue"))
        {

        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            Debug.Log("false");

            isButtonActive = false;
        }
    }

    private void OpenDoor()
    {

        GameObject door = GameObject.FindGameObjectWithTag("Door");
        Rigidbody2D rBodyDoor = door.GetComponent<Rigidbody2D>();
        if (door != null)
        {
            if (isButtonActive && Input.GetButtonDown("Interact"))
            {
                Debug.Log("interacted");
                if (rBodyDoor.velocity.y <= 0 && door.transform.position.y < maxDoorHeight)
                    rBodyDoor.velocity = new Vector2(0, 2);

            }
            else if (!isButtonActive)
            {
                rBodyDoor.velocity = Vector2.zero;

            }
            if (door.transform.position.y > maxDoorHeight)
            {
                rBodyDoor.velocity = Vector2.zero;
            }
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
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
            isWallJumping = true;
            rBody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                pRenderer.flipX = !pRenderer.flipX;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void WallSlide()
    {
        if (isOnWall && !isOnGround)
        {
            isWallSliding = true;
            rBody.velocity = new Vector2(rBody.velocity.x, 0);
        }
        else
        {
            isWallSliding = false;
        }
    }

}
