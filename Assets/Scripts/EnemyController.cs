using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D rb; //剛体変数
    private Animator enemyAnim; //アニメ変数

    [SerializeField]
    private float moveSpeed, waitTime, walkTime;    //移動速度、待ち時間、歩く時間変数

    private float waitCounter, moveCounter; //タイマー用の関数

    private Vector2 moveDir;    //動く方向の変数

    [SerializeField, Tooltip("プレイヤーを追いかけるか")]
    private bool chase; //追いかける判定の変数

    private bool isChaseing; //追いかけてるか判定用の変数

    [SerializeField]
    private float chaseSpeed, rangeToChase; //追いかける速度、気付く範囲の変数

    private Transform target;   //playerの位置情報の変数 Transformは位置情報を扱うもの

    [SerializeField]
    private float waitAfterHitting; //攻撃の間隔の変数

    [SerializeField]
    public int currentHealth;//現在のhp変数
    [SerializeField]
    private int attackDamage;//攻撃力の変数

    private bool isKnockingBack;   //ノックバック中か変数

    [SerializeField]
    private float knockBackTime, knockBackForce; //ノックバック時間と力の変数
    private float knockBackCounter; //上のタイマーよう  変数

    private Vector2 knockDir; //吹き飛ぶ方向変数

    [SerializeField]
    private GameObject dropItem1; //ドロップアイテム用の変数
    [SerializeField]
    private GameObject dropItem2; //ドロップアイテム用の変数
    [SerializeField]
    private GameObject dropItem3; //ドロップアイテム用の変数
    [SerializeField]
    private GameObject rareDropItem;
    [SerializeField]
    private float healthDropChance; //ドロップ確率用の変数
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


        rb = GetComponent<Rigidbody2D>();    //コンポーネントを取得
        enemyAnim = GetComponent<Animator>(); //アニメータ取得

        waitCounter = waitTime; //待ち時間設定

        target = GameObject.FindGameObjectWithTag("Player").transform;  //playerの位置情報の取得

        currentHealth = currentHealth + PlayerStatusTakeOver.floorLevel * 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        

        //ノックバック中か
        if(isKnockingBack)
        {
            //まだ吹き飛んでいるか
            if(knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime; //時間を減らす
                rb.velocity = knockDir * knockBackForce; //吹き飛び方
            }
            else
            {
                rb.velocity = Vector2.zero; //停止

                isKnockingBack = false;    //ノックバックしているかをfalseにする
            }

            return; //以下のコードは実行されない
        }

        //追いかけている最中か判定
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

            //waitCounterが0より大きいか判定
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;  //フレーム分の数値を引く Time.deltaTimeで秒数を取得
                rb.velocity = Vector2.zero; //スピードを消す(とめる)

                //waitCounterが0より小さくなったか
                if (waitCounter <= 0)
                {
                    moveCounter = walkTime; //Enemyを動かす

                    enemyAnim.SetBool("moving", true); //アニメーションの変更

                    moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));  //ランダムに動く設定
                    moveDir.Normalize();  //正規化
                }
            }
            //動いているとき
            else
            {

                moveCounter -= Time.deltaTime;  //フレーム分の数値を引く Time.deltaTimeで秒数を取得

                rb.velocity = moveDir * moveSpeed;

                if (moveCounter <= 0)
                {
                    
                    enemyAnim.SetBool("moving", false);

                    waitCounter = waitTime;
                }
            }

            //追いかけているか判定
            if (chase)
            {
                //Vector3.Distance(a,b)でaとbの距離を出す
                //距離が近いかRangeToChaseより近いか
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

                    //追いかけている
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
                moveDir = target.transform.position - transform.position;   //target(player)の位置に向かう
                moveDir.Normalize();    //正規化   

                rb.velocity = moveDir * chaseSpeed; //追いかける速度をかけて早くする
            }
                //ある程度距離が離れたら追いかけない
            if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
            {
                
                isChaseing = false; //追いかけなくする

                waitCounter = waitTime;

                enemyAnim.SetBool("moving", false); //アニメーションoff
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Playerにぶつかったら
        if (collision.gameObject.tag == "Player")
        {
            //追いかけているときにぶつかったら
            if(isChaseing)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);

                waitCounter = waitAfterHitting; //攻撃後の待機時間

                enemyAnim.SetBool("moving", false);
            }
        }
    }

    /// <summary>
    /// Enemyのノックバック用関数
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        isKnockingBack = true;  //ノックバンクをtrue
        knockBackCounter = knockBackTime;   //ノックバックの時間を設定

        knockDir = transform.position - position;   //Enemyのポジション -　剣のポジションで吹き飛ばす
        knockDir.Normalize();

        enemyAnim.SetBool("moving", false);
    }

    public void TakeDamage(int damage,Vector3 position)
    {
        currentHealth -= damage;    //Hpを減らす

        Vector3 pos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        Vector3 rarepos = new Vector3(transform.position.x, transform.position.y, transform.position.z);


        if (currentHealth <= 0)
        {
            PlayerStatusTakeOver.currenExp += exp;

            //ランダムで選ばれた数字はドロップ率より小さいかかつポーションが設定されているとき
            if(Random.Range(0,100) < healthDropChance && dropItem1)
            {
                if(Random.Range(0,2) == 0)
                {
                    Instantiate(dropItem1, pos, transform.rotation);   //Instantiate(もの,ばしょ,向き)はGameObjectを生成できる
                }
                else if(Random.Range(0,2) == 1 )
                {
                    Instantiate(dropItem2, pos, transform.rotation);   //Instantiate(もの,ばしょ,向き)はGameObjectを生成できる
                }
                else
                {
                    Instantiate(dropItem3, pos, transform.rotation);   //Instantiate(もの,ばしょ,向き)はGameObjectを生成できる
                }
            }
            if(Random.Range(0,100) < rareDropChance && rareDropItem)
            {
                Instantiate(rareDropItem, rarepos, transform.rotation);
            }

            Destroy(gameObject);    //これがアタッチされているゲームオブジェクトを壊す
        }

        KnockBack(position);    //KnockBackにpositionを渡し呼び出す
    }

    public void Enemy1Shot()
    {
        //敵の座標を変数posに保存
        var pos = this.gameObject.transform.position;
        //弾のプレハブを作成
        var t = Instantiate(enemyBullet) as GameObject;
        //弾のプレハブの位置を敵の位置にする
        t.transform.position = pos;
        //敵からプレイヤーに向かうベクトルをつくる
        //プレイヤーの位置から敵の位置（弾の位置）を引く
        Vector2 vec = target.transform.position - pos;
        vec.Normalize();
        //弾のRigidBody2Dコンポネントのvelocityに先程求めたベクトルを入れて力を加える
        t.GetComponent<Rigidbody2D>().velocity = vec * bulletSpeed;
    }

    public void Enemy2Shot()
    {
        var t = new GameObject[4];
        for(int i = 0;i <= 2;i++)
        {
            //敵の座標を変数posに保存
            var pos = this.gameObject.transform.position;
            //弾のプレハブを作成
            t[i] = Instantiate(enemyBullet) as GameObject;
            //弾のプレハブの位置を敵の位置にする
            t[i].transform.position = pos;
            //敵からプレイヤーに向かうベクトルをつくる
            //プレイヤーの位置から敵の位置（弾の位置）を引く
            Vector2 vec = new Vector2(target.transform.position.x - pos.x - 20 + 20 * i, target.transform.position.y - 20 + 10 * i).normalized;
            //弾のRigidBody2Dコンポネントのvelocityに先程求めたベクトルを入れて力を加える
            t[i].GetComponent<Rigidbody2D>().velocity = vec * bulletSpeed;

        }
    }

    public void Boss1Shot()
    {
        //敵の座標を変数posに保存
        var pos = this.gameObject.transform.position;
        //弾のプレハブを作成
        var t = Instantiate(enemyBullet) as GameObject;
        //弾のプレハブの位置を敵の位置にする
        t.transform.position = pos;
        //敵からプレイヤーに向かうベクトルをつくる
        //プレイヤーの位置から敵の位置（弾の位置）を引く
        Vector2 vec = target.transform.position - pos;
        vec.Normalize();
        //弾のRigidBody2Dコンポネントのvelocityに先程求めたベクトルを入れて力を加える
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
