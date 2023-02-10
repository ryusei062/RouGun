using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    private int bulletDamage = 10;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        this.lastVelocity = this.rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            PlayerController player = coll.gameObject.GetComponent<PlayerController>();

            player.KnockBack(transform.position);
            player.DamagePlayer(bulletDamage);

            Destroy(this.gameObject);
        }
        else if (coll.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
