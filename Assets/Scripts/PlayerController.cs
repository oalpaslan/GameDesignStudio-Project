using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float pSpeed = 8f;
    [SerializeField]
    private float jumpForce = 5f;

    public Rigidbody2D rBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rBody.velocity = new Vector2(pSpeed * Input.GetAxis("Horizontal"), rBody.velocity.y);

        if(Input.GetButtonDown("Jump"))
        {

            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);

        }
    }
}
