using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            collision.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Crumble"))
        {
            StartCoroutine(WaitBeforeCrumble(collision));


        }
    }

    IEnumerator WaitBeforeCrumble(Collider2D col)
    {
        yield return new WaitForSeconds(1);
        col.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        col.gameObject.SetActive(true);
    }
}
