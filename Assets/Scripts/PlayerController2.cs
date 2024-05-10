using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController2 : MonoBehaviour
{

    public static PlayerController2 instance;

    //Movement

    [SerializeField]
    private float baseSpeed, pSpeed = 5f;
    [SerializeField]
    private float jumpForce = 15f;
    [SerializeField]
    private float wallSlidingSpeed = 2f;
    [SerializeField]
    private float VSpeedTick = 3;

    [SerializeField]
    private float maxDoorHeight;

    [SerializeField]
    private float minHeightBeforeDeath;

    [SerializeField]
    public float bloodAmount, damage;

    private bool isButtonActive = false;
    public Rigidbody2D rBody;
    public CapsuleCollider2D pCollider;
    public SpriteRenderer pRenderer;
    private Animator anim;

    //Wall Slide and Jump

    public Transform groundCheckPoint, wallCheckPoint;
    public LayerMask whatIsGround, whatIsWall;

    private bool isOnGround, isOnWall, isWallSliding, isWallJumping;

    private float wallJumpingDirection, wallJumpingCounter,
                    wallJumpingTime = 0.2f,
                    wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    //Warp
    private GameObject curWarp;

    //Powers
    public bool isSpendingBlood = false,
                isVampVisEnabled = false,
                isVampSpdEnabled = false,
                hiddenLayerStatus = true;
    [SerializeField]
    private float vampSpeedCost;
    private float bloodInUse = 0;

    private GameObject hLayer;

    private bool isAscending, isDecending;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        pRenderer = gameObject.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pCollider = GetComponent<CapsuleCollider2D>();
        hLayer = GameObject.FindGameObjectWithTag("Hidden");
    }

    void Update()
    {
        //pSpeed = 5f;


        Movement();

        isOnGround = Physics2D.OverlapCircle(groundCheckPoint.position, .05f, whatIsGround); //OverlapCircle tells if a circle in a position overlaps with another collider

        isOnWall = Physics2D.OverlapCircle(wallCheckPoint.position, .1f, whatIsWall);

        WallSlide();
        WallJump();

        //OpenDoor();
        UseWarp();

        //power toogles
        //vVision();

        checkDeath(); // makes all death checks (height and HP)
        if (isVampSpdEnabled && isSpendingBlood)
        {
            pSpeed = 10f;
        }
        else if (!isVampSpdEnabled || !isSpendingBlood)
        {
            pSpeed = baseSpeed;
        }
    }

    private void Movement()
    {

        rBody.velocity = new Vector2(pSpeed * Input.GetAxis("Horizontal"), rBody.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(rBody.velocity.x));
        anim.SetBool("IsWallSlide", isWallSliding);
        anim.SetBool("IsOnGround", isOnGround);
        anim.SetBool("IsAscending", isAscending);
        anim.SetBool("IsDecending", isDecending);

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
        if (Input.GetButtonDown("Speed"))
        {
            isVampSpdEnabled = !isVampSpdEnabled;
            VampSpeed(isVampSpdEnabled);

        }
        if (bloodInUse > 0 && isSpendingBlood)
        {
            StartCoroutine(useBlood(bloodInUse));
        }

        if (rBody.velocity.y > 1)
        {
            isAscending = true;
            isDecending = false;
        }
        else if (rBody.velocity.y < -1)
        {
            isAscending = false;
            isDecending = true;
        }
        else
        {
            isAscending = false;
            isDecending = false;
        }
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void changeScene(string scene)
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        SceneManager.LoadScene(scene);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Object");
        Rigidbody2D rBodyObj = obj.GetComponent<Rigidbody2D>();

        if (collision.gameObject.CompareTag("Object"))
        {
            rBodyObj.isKinematic = true;
        }
        //else if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    bloodAmount -= damage;
        //}

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
            isButtonActive = true;
        }
        //else if (collision.gameObject.CompareTag("Soulmate"))
        //{
        //    //Trigger dialogue
        //    //Use Panel
        //}

        else if (collision.gameObject.CompareTag("Warp"))
        {
            curWarp = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("Hidden"))
        {
            toggleHidden(false);
        }
        if (collision.gameObject.CompareTag("Scene"))
        {

            changeScene(collision.gameObject.name);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Button"))
        {
            isButtonActive = false;
        }
        if (collision.CompareTag("Warp"))
        {
            if (collision.gameObject == curWarp)
            {
                curWarp = null;
            }
        }
        if (collision.gameObject.CompareTag("Hidden"))
        {
            toggleHidden(true);
        }
    }

    //private void OpenDoor()
    //{

    //    GameObject door = GameObject.FindGameObjectWithTag("Door");
    //    if (door != null)
    //    {
    //    Rigidbody2D rBodyDoor = door.GetComponent<Rigidbody2D>();
    //        if (isButtonActive && Input.GetButtonDown("Interact"))
    //        {
    //            Debug.Log("interacted");
    //            if (rBodyDoor.velocity.y <= 0 && door.transform.position.y < maxDoorHeight)
    //                rBodyDoor.velocity = new Vector2(0, 2);

    //        }
    //        else if (!isButtonActive)
    //        {
    //            rBodyDoor.velocity = Vector2.zero;

    //        }
    //        if (door.transform.position.y > maxDoorHeight)
    //        {
    //            rBodyDoor.velocity = Vector2.zero;
    //        }
    //    }
    //}

    private void UseWarp()
    {
        if (curWarp != null)
        {
            if (Input.GetButtonDown("Interact"))
            {
                transform.position = curWarp.GetComponent<getDest>().getD().position;
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

    private IEnumerator useBlood(float bSpent)
    {
        bloodInUse = 0;
        while (bloodAmount > bSpent && isSpendingBlood)
        {

            bloodAmount -= bSpent;

            //updateBloodUI();
            yield return new WaitForSeconds(0.1f);
        }
        isSpendingBlood = false;
    }

    private void checkDeath()
    {
        if (bloodAmount <= 0)
        {
            //Reset();
        }
        if (gameObject.transform.position.y < minHeightBeforeDeath)
            Reset();
    }
    //private void vVision()
    //{
    //    GameObject vvTrue = GameObject.FindGameObjectWithTag("vvTrue");

    //    if (Input.GetButtonDown("Vision"))
    //    {
    //        if (isVampVisEnabled == false)
    //        {
    //            if (useBlood(1))
    //            {  //determine how much blood will be used to open
    //                isVampVisEnabled = true;
    //                toggleHidden(false);
    //                vvTrue.SetActive(true);
    //            }
    //        }
    //        else
    //        {
    //            isVampVisEnabled = false;
    //            toggleHidden(true);
    //            vvTrue.SetActive(false);
    //        }
    //    }


    //}

    private void VampSpeed(bool vSpeedEnabled)
    {
        if (vSpeedEnabled && vampSpeedCost < bloodAmount)
        {

            isVampSpdEnabled = true;
            bloodInUse = vampSpeedCost;
            isSpendingBlood = true;
        }
        else
        {
            isVampSpdEnabled = false;
            bloodInUse = 0;
            isSpendingBlood = false;
        }

    }

    private void toggleHidden(bool hidden)
    {

        //hiddenLayerStatus true means it is showing (hiding the map)

        if (hiddenLayerStatus && !hidden) //hidden layer is active and there is a request to disable it
        {
            //hLayer.SetActive(false);
            hLayer.transform.GetComponent<TilemapRenderer>().enabled = false;
            hiddenLayerStatus = false;


            // I need to make an Update i think

            return;
        }

        if (!hiddenLayerStatus && hidden) //hidden layer is closed and there is a request to enable it
        {
            //hLayer.SetActive(true);
            hLayer.transform.GetComponent<TilemapRenderer>().enabled = true;

            hiddenLayerStatus = true;
            // I need to make an Update i think

            return;
        }
    }

}





