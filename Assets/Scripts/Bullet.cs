using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    private int wallCount = 0;
    private int bulletDamage;
    private float bCount = 0;

    private bool bound = false;

    private GunStatus _NowGun = GameManager.instance.NowGun();

    private float cooldown;

    private string guname;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GunSetUp();
    }

    private void Update()
    {
        Debug.Log(bulletDamage);
        Debug.Log(bound);
        Debug.Log(cooldown);
        Debug.Log(guname);

        bCount += Time.deltaTime;

        if(bCount >= 0.3)
        {
            gameObject.layer = 9;
        }else
        {
            gameObject.layer = 8;
        }
    }

    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 refrectVec = Vector2.Reflect(this.lastVelocity, coll.contacts[0].normal);
        this.rb.velocity = refrectVec;

        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "BossEnemy")
        {
            coll.gameObject.GetComponent<EnemyController>().TakeDamage(bulletDamage, transform.position);  //剣のダメージと座標を渡しTakeDamageを呼び出す
            SoundManager.instance.PlaySE(3);
            Destroy(this.gameObject);
        }
        else if (coll.gameObject.tag == "Player")
        {
            PlayerController player = coll.gameObject.GetComponent<PlayerController>();

            player.KnockBack(transform.position);
            player.DamagePlayer(bulletDamage);

            Destroy(this.gameObject);
        }
        else
        {
            if(bound == false)
            {
                Destroy(gameObject);
            }
            wallCount += 1;
        }

        if (wallCount >= 3)
        {
            Destroy(this.gameObject);
        }
    }

    public void GunSetUp()
    {
        bulletDamage = _NowGun.GetDamage();
        bound = _NowGun.GetBulletBound();
        cooldown = _NowGun.GetShotCooldown();

        guname = _NowGun.GetGunName();


    }
}