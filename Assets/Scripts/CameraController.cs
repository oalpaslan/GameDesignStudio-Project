using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField]
    private Transform farBackground;

    [SerializeField]
    private float minHeight, maxHeight;

    private float lastXPos, lastYPos;

    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(playerTransform.position.x, Mathf.Clamp(playerTransform.position.y, minHeight, maxHeight), transform.position.z);

        Vector3 amountToMove = transform.position - lastPos;


        lastPos = amountToMove;

        farBackground.position += amountToMove;
        lastPos = transform.position;

    }
}
