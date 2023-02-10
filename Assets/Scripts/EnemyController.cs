using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rb; //���̕ϐ�
    private Animator enemyAnim; //�A�j���ϐ�

    [SerializeField]
    private float moveSpeed, waitTime, walkTime;    //�ړ����x�A�҂����ԁA�������ԕϐ�

    private float waitCounter, moveCounter; //�^�C�}�[�p�̊֐�

    private Vector2 moveDir;    //���������̕ϐ�

    [SerializeField, Tooltip("�v���C���[��ǂ������邩")]
    private bool chase; //�ǂ������锻��̕ϐ�

    private bool isChaseing; //�ǂ������Ă邩����p�̕ϐ�

    [SerializeField]
    private float chaseSpeed, rangeToChase; //�ǂ������鑬�x�A�C�t���͈͂̕ϐ�

    private Transform target;   //player�̈ʒu���̕ϐ� Transform�͈ʒu������������

    [SerializeField]
    private float waitAfterHitting; //�U���̊Ԋu�̕ϐ�

    [SerializeField]
    public int currentHealth;//���݂�hp�ϐ�
    [SerializeField]
    private int attackDamage;//�U���͂̕ϐ�

    private bool isKnockingBack;   //�m�b�N�o�b�N�����ϐ�

    [SerializeField]
    private float knockBackTime, knockBackForce; //�m�b�N�o�b�N���ԂƗ͂̕ϐ�
    private float knockBackCounter; //��̃^�C�}�[�悤  �ϐ�

    private Vector2 knockDir; //������ԕ����ϐ�

    [SerializeField]
    private GameObject dropItem1; //�h���b�v�A�C�e���p�̕ϐ�
    [SerializeField]
    private GameObject dropItem2; //�h���b�v�A�C�e���p�̕ϐ�
    [SerializeField]
    private GameObject dropItem3; //�h���b�v�A�C�e���p�̕ϐ�
    [SerializeField]
    private GameObject rareDropItem;
    [SerializeField]
    private float healthDropChance; //�h���b�v�m���p�̕ϐ�
    [SerializeField]
    private float rareDropChance;

    [SerializeField]
    private GameObject enemyBullet;
    [SerializeField]
    private float shotInterval, bossInterval;
    private float timeCount;
    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private int enemyType, bossType;

    [SerializeField]
    private int exp;

    private GameObject collisionDetector;
    BossRoomCollision bossRoomCollision;

    // Start is called before the first frame update
    void Start()
    {
        collisionDetector = GameObject.Find("CollisionDetector");
        if (PlayerStatusTakeOver.floorLevel % 10 == 0)
        {
            bossRoomCollision = collisionDetector.GetComponent<BossRoomCollision>();
        }


        rb = GetComponent<Rigidbody2D>();    //�R���|�[�l���g���擾
        enemyAnim = GetComponent<Animator>(); //�A�j���[�^�擾

        waitCounter = waitTime; //�҂����Ԑݒ�

        target = GameObject.FindGameObjectWithTag("Player").transform;  //player�̈ʒu���̎擾

        currentHealth = currentHealth + PlayerStatusTakeOver.floorLevel * 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        

        //�m�b�N�o�b�N����
        if(isKnockingBack)
        {
            //�܂��������ł��邩
            if(knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime; //���Ԃ����炷
                rb.velocity = knockDir * knockBackForce; //������ѕ�
            }
            else
            {
                rb.velocity = Vector2.zero; //��~

                isKnockingBack = false;    //�m�b�N�o�b�N���Ă��邩��false�ɂ���
            }

            return; //�ȉ��̃R�[�h�͎��s����Ȃ�
        }

        //�ǂ������Ă���Œ�������
        if (!isChaseing)
        {
            if(PlayerStatusTakeOver.floorLevel % 10 == 0)
            {
                if (timeCount > bossInterval)
                {
                    if (bossType == 1)
                    {
                        timeCount = 0;
                        Boss1Shot();
                    }
                    else if (bossType == 2)
                    {
                        timeCount = 0;
                        Boss2Shot();
                    }

                }
            }

            //waitCounter��0���傫��������
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;  //�t���[�����̐��l������ Time.deltaTime�ŕb�����擾
                rb.velocity = Vector2.zero; //�X�s�[�h������(�Ƃ߂�)

                //waitCounter��0��菬�����Ȃ�����
                if (waitCounter <= 0)
                {
                    moveCounter = walkTime; //Enemy�𓮂���

                    enemyAnim.SetBool("moving", true); //�A�j���[�V�����̕ύX

                    moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));  //�����_���ɓ����ݒ�
                    moveDir.Normalize();  //���K��
                }
            }
            //�����Ă���Ƃ�
            else
            {

                moveCounter -= Time.deltaTime;  //�t���[�����̐��l������ Time.deltaTime�ŕb�����擾

                rb.velocity = moveDir * moveSpeed;

                if (moveCounter <= 0)
                {
                    
                    enemyAnim.SetBool("moving", false);

                    waitCounter = waitTime;
                }
            }

            //�ǂ������Ă��邩����
            if (chase)
            {
                //Vector3.Distance(a,b)��a��b�̋������o��
                //�������߂���RangeToChase���߂���
                if (Vector3.Distance(transform.position, target.transform.position) < rangeToChase)
                {
                    if (timeCount >= shotInterval && enemyType == 1)
                    {
                        timeCount = 0;
                        Enemy1Shot();
                    }
                    else if (timeCount > shotInterval && enemyType == 2)
                    {
                        timeCount = 0;
                        Enemy2Shot();
                    }

                    //�ǂ������Ă���
                    isChaseing = true;
                }
            }
        }
        else
        {
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    enemyAnim.SetBool("moving", true);
                }
            }
            else
            {
                moveDir = target.transform.position - transform.position;   //target(player)�̈ʒu�Ɍ�����
                moveDir.Normalize();    //���K��   

                rb.velocity = moveDir * chaseSpeed; //�ǂ������鑬�x�������đ�������
            }
                //������x���������ꂽ��ǂ������Ȃ�
            if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
            {
                
                isChaseing = false; //�ǂ������Ȃ�����

                waitCounter = waitTime;

                enemyAnim.SetBool("moving", false); //�A�j���[�V����off
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player�ɂԂ�������
        if (collision.gameObject.tag == "Player")
        {
            //�ǂ������Ă���Ƃ��ɂԂ�������
            if(isChaseing)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);

                waitCounter = waitAfterHitting; //�U����̑ҋ@����

                enemyAnim.SetBool("moving", false);
            }
        }
    }

    /// <summary>
    /// Enemy�̃m�b�N�o�b�N�p�֐�
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        isKnockingBack = true;  //�m�b�N�o���N��true
        knockBackCounter = knockBackTime;   //�m�b�N�o�b�N�̎��Ԃ�ݒ�

        knockDir = transform.position - position;   //Enemy�̃|�W�V���� -�@���̃|�W�V�����Ő�����΂�
        knockDir.Normalize();

        enemyAnim.SetBool("moving", false);
    }

    public void TakeDamage(int damage,Vector3 position)
    {
        currentHealth -= damage;    //Hp�����炷

        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        Vector3 rarepos = new Vector3(transform.position.x, transform.position.y, transform.position.z);


        if (currentHealth <= 0)
        {
            PlayerStatusTakeOver.currenExp += exp;

            //�����_���őI�΂ꂽ�����̓h���b�v����菬���������|�[�V�������ݒ肳��Ă���Ƃ�
            if(Random.Range(0,100) < healthDropChance && dropItem1)
            {
                if(Random.Range(0,2) == 0)
                {
                    Instantiate(dropItem1, pos, transform.rotation);   //Instantiate(����,�΂���,����)��GameObject�𐶐��ł���
                }
                else if(Random.Range(0,2) == 1 )
                {
                    Instantiate(dropItem2, pos, transform.rotation);   //Instantiate(����,�΂���,����)��GameObject�𐶐��ł���
                }
                else
                {
                    Instantiate(dropItem3, pos, transform.rotation);   //Instantiate(����,�΂���,����)��GameObject�𐶐��ł���
                }
            }
            if(Random.Range(0,100) < rareDropChance && rareDropItem)
            {
                Instantiate(rareDropItem, rarepos, transform.rotation);
            }

            Destroy(gameObject);    //���ꂪ�A�^�b�`����Ă���Q�[���I�u�W�F�N�g����
        }

        KnockBack(position);    //KnockBack��position��n���Ăяo��
    }

    public void Enemy1Shot()
    {
        //�G�̍��W��ϐ�pos�ɕۑ�
        var pos = this.gameObject.transform.position;
        //�e�̃v���n�u���쐬
        var t = Instantiate(enemyBullet) as GameObject;
        //�e�̃v���n�u�̈ʒu��G�̈ʒu�ɂ���
        t.transform.position = pos;
        //�G����v���C���[�Ɍ������x�N�g��������
        //�v���C���[�̈ʒu����G�̈ʒu�i�e�̈ʒu�j������
        Vector2 vec = target.transform.position - pos;
        vec.Normalize();
        //�e��RigidBody2D�R���|�l���g��velocity�ɐ�����߂��x�N�g�������ė͂�������
        t.GetComponent<Rigidbody2D>().velocity = vec * bulletSpeed;
    }

    public void Enemy2Shot()
    {
        var t = new GameObject[4];
        for(int i = 0;i <= 2;i++)
        {
            //�G�̍��W��ϐ�pos�ɕۑ�
            var pos = this.gameObject.transform.position;
            //�e�̃v���n�u���쐬
            t[i] = Instantiate(enemyBullet) as GameObject;
            //�e�̃v���n�u�̈ʒu��G�̈ʒu�ɂ���
            t[i].transform.position = pos;
            //�G����v���C���[�Ɍ������x�N�g��������
            //�v���C���[�̈ʒu����G�̈ʒu�i�e�̈ʒu�j������
            Vector2 vec = new Vector2(target.transform.position.x - pos.x - 20 + 20 * i, target.transform.position.y - 20 + 10 * i).normalized;
            //�e��RigidBody2D�R���|�l���g��velocity�ɐ�����߂��x�N�g�������ė͂�������
            t[i].GetComponent<Rigidbody2D>().velocity = vec * bulletSpeed;

        }
    }

    public void Boss1Shot()
    {
        //�G�̍��W��ϐ�pos�ɕۑ�
        var pos = this.gameObject.transform.position;
        //�e�̃v���n�u���쐬
        var t = Instantiate(enemyBullet) as GameObject;
        //�e�̃v���n�u�̈ʒu��G�̈ʒu�ɂ���
        t.transform.position = pos;
        //�G����v���C���[�Ɍ������x�N�g��������
        //�v���C���[�̈ʒu����G�̈ʒu�i�e�̈ʒu�j������
        Vector2 vec = target.transform.position - pos;
        vec.Normalize();
        //�e��RigidBody2D�R���|�l���g��velocity�ɐ�����߂��x�N�g�������ė͂�������
        t.GetComponent<Rigidbody2D>().velocity = vec * bulletSpeed;
    }

    public void Boss2Shot()
    {
        for (int i = 0; i < 360; i += 72)
        {
            var rad = i * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);
            var pos = this.gameObject.transform.position + new Vector3(cos, sin, 0).normalized;
            GameObject t = Instantiate(enemyBullet, transform.position, Quaternion.identity);
            Vector2 vec = target.transform.position - pos;
            vec.Normalize();
            t.GetComponent<Rigidbody2D>().velocity = vec * bulletSpeed;
        }
    }
}
