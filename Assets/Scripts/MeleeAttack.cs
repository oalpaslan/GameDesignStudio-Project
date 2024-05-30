using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public int attackDamage;
    public float attackCooldown;
    private float nextAttackTime = 0f;

    private bool isPlayerTurnedLeft, isOffsetNegative = false;

    private CapsuleCollider2D weaponCollider;
    [SerializeField]
    private float drainBlood;
    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<CapsuleCollider2D>();
        weaponCollider.enabled = false; // Initially, the weapon collider is disabled
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerTurnedLeft = PlayerController2.instance.pRenderer.flipX;
        if (Time.time >= nextAttackTime)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        if (isPlayerTurnedLeft && !isOffsetNegative)
        {
            isOffsetNegative = true;
            gameObject.GetComponent<CapsuleCollider2D>().offset *= -1;
        }
        else if (!isPlayerTurnedLeft && isOffsetNegative)
        {
            gameObject.GetComponent<CapsuleCollider2D>().offset *= -1;
            isOffsetNegative = false;
        }
    }
    void Attack()
    {
        StartCoroutine(EnableColliderTemporarily());
    }

    private IEnumerator EnableColliderTemporarily()
    {
        Debug.Log("Attack!!");
        weaponCollider.enabled = true;
        yield return new WaitForFixedUpdate(); // Wait for the next physics update
        weaponCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Damage the enemy
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                PlayerController2.instance.bloodAmount += drainBlood;
            }
        }
    }
}
