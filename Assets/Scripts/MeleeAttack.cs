using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public int attackDamage;
    public float attackCooldown;
    private float nextAttackTime = 0f;

    private CapsuleCollider2D weaponCollider;
    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<CapsuleCollider2D>();
        weaponCollider.enabled = false; // Initially, the weapon collider is disabled
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("time if works");

            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1 works");

                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
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
            Debug.Log("Enemy is collided with claw");
            // Damage the enemy
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log("Enemy is not null!!");

                enemy.TakeDamage(attackDamage);
            }
        }
    }
}
