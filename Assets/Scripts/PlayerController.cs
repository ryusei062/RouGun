using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("移動スピード")]    //これでエディタ上で数値を入れれる　Tooltip("説明")で説明を表示できる
    private int moveSpeed;  //移動スピード変数 privateは参照できなくなる（他のスクリプトから値を変えられなくなる）,publicはどこからでも参照でき変えれる

    [SerializeField]
    private Animator playerAnim;    //アニメーション用の変数

    public Rigidbody2D rb;  //リジッドボディの変数

    [SerializeField]
    private Animator weponAnim; //武器アニメーション用の変数

    [System.NonSerialized]  //これでエディタ上に表示されない


    private bool isknockingback;    //ノックバックしているか変数
    private Vector2 knockDir; //吹き飛ぶ方向変数

    [SerializeField]
    private float knockbackTime, knockbackForce;    //吹き飛ぶ時間と力変数
    private float knockbackCounter; //上のタイマー用変数

    [SerializeField]
    private float invincibilityTime;    //無敵時間変数
    private float invincibilityCounter; //上のタイマー用変数

    public float usuallyRecoverySpeed;   //スタミナ最大値、スタミナ回復速度の変数
    private float recoverySpeed, runRecoverySpeed = 0;
    [System.NonSerialized]

    [SerializeField]
    private OverrideLight2D light2d;
    [System.NonSerialized]
    private float ltTimer = 1f;

    [SerializeField]
    private float dashSpeed, dashLength, dashCost, runSpeed, runCost;    //回避の速度、長さの変数,スタミナの消費量変数

    private float dashCounter, activeMoveSpeed; //回避のタイマー、移動時にかける用の変数

    private float runTimer = 0f;
    [SerializeField]
    private float attackCounter;

    private bool Run = false, Dash = false;

    private Camera mainCam;
    private Vector2 mousePos, direction;

    [SerializeField]
    private GameObject bullet;
    private float bulletSpeed = 15.0f;

    private float shotCooldown;
    private float shotCooldownTime = 2f;

    private float reloadTime;

    private int maxAmmo;

    private bool fullAuto;

    private GunStatus _NowGun;

    [SerializeField]
    private float gunChangeTime = 0;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.UpdateHealthUI();  //static化でGameManagerの関数を簡単に呼び出せる

        GameManager.instance.UpdateStaminaUI();

        GameManager.instance.UpdateLTUI();

        GameManager.instance.UpdateLevel();

        GameManager.instance.UpdateAmmo();

        activeMoveSpeed = moveSpeed;

        GunSetUp();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.C))
        {
            GunSetUp();
            gunChangeTime = reloadTime;
        }

        if (PlayerStatusTakeOver.currenExp >= PlayerStatusTakeOver.needExp)
        {
            PlayerStatusTakeOver.currentLv += 1;
            PlayerStatusTakeOver.currentSP += 1;

            PlayerStatusTakeOver.currenExp = 0;
            PlayerStatusTakeOver.needExp = (int) (PlayerStatusTakeOver.needExp * 1.5f);
        }

        if (!GameManager.instance.isPaused)
        {
            recoverySpeed = usuallyRecoverySpeed;

            ltTimer -= Time.deltaTime;
            if (PlayerStatusTakeOver.currentLT > 0)
            {
                PlayerStatusTakeOver.currentLT = Mathf.Clamp(PlayerStatusTakeOver.currentLT - Time.deltaTime, 0, PlayerStatusTakeOver.maxHP);  //LTゲージ減少
                GameManager.instance.UpdateLTUI();
            }
            else if (PlayerStatusTakeOver.currentLT <= 0 && ltTimer <= 0)
            {
                PlayerStatusTakeOver.currentHP -= 1;
                ltTimer = 1f;
                GameManager.instance.UpdateHealthUI();
            }

            //無敵時間か
            if (invincibilityCounter > 0)
            {
                invincibilityCounter -= Time.deltaTime;
            }

            //ノックバック中か
            if (isknockingback)
            {
                knockbackCounter -= Time.deltaTime;
                rb.velocity = knockDir * knockbackForce;

                //ノックバック終わったか
                if (knockbackCounter <= 0)
                {
                    isknockingback = false;
                }
                else
                {
                    return;     //return;は以下のコードは実行されない
                }
            }

            PlayerTurning();

            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed;    //キーボードの入力を受け取る
                                                                                                                                     //velocityは速度を扱うもの Vector2はx,yの値を扱うもの .normalizedはベクトルを正規化する


            shotCooldownTime += Time.deltaTime;
            gunChangeTime -= Time.deltaTime;

            
            if(gunChangeTime <= 0)
            {
                if (fullAuto == false)
                {

                    //マウスが押されたとき 0が左クリック
                    if (Input.GetMouseButtonDown(0) && shotCooldownTime >= shotCooldown)
                    {
                        if (PlayerStatusTakeOver.currentAmmo <= 0)
                        {
                            SoundManager.instance.PlaySE(7);
                        }
                        else
                        {
                            // 弾（ゲームオブジェクト）の生成
                            GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                            // クリックした座標の取得（スクリーン座標からワールド座標に変換）
                            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            // 向きの生成（Z成分の除去と正規化）
                            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;
                            // 弾に速度を与える
                            clone.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed;

                            PlayerStatusTakeOver.currentAmmo--;

                            GameManager.instance.UpdateAmmo();

                            SoundManager.instance.PlaySE(5);

                            shotCooldownTime = 0;
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0) && shotCooldownTime >= shotCooldown)
                    {
                        if (PlayerStatusTakeOver.currentAmmo <= 0)
                        {
                            SoundManager.instance.PlaySE(7);
                        }
                        else
                        {
                            // 弾（ゲームオブジェクト）の生成
                            GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                            // クリックした座標の取得（スクリーン座標からワールド座標に変換）
                            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            // 向きの生成（Z成分の除去と正規化）
                            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;
                            // 弾に速度を与える
                            clone.GetComponent<Rigidbody2D>().velocity = shotForward * bulletSpeed;

                            PlayerStatusTakeOver.currentAmmo--;

                            GameManager.instance.UpdateAmmo();

                            SoundManager.instance.PlaySE(5);

                            shotCooldownTime = 0;
                        }
                    }
                }

                if (PlayerStatusTakeOver.currentAmmo <= maxAmmo && Input.GetKey(KeyCode.R))
                {
                    PlayerStatusTakeOver.currentAmmo = maxAmmo;
                    GameManager.instance.UpdateAmmo();

                    //shotCooldownTime -= reloadTime;

                    SoundManager.instance.PlaySE(8);
                }
            }

            if (rb.velocity != Vector2.zero)
            {
                //run時の処理
                if (Input.GetKey(KeyCode.LeftShift) && PlayerStatusTakeOver.currentStamina > runCost)
                {
                    runTimer -= Time.deltaTime;
                    if (runTimer <= 0)
                    {
                        runTimer = 0.5f;
                        PlayerStatusTakeOver.currentStamina -= runCost;
                        GameManager.instance.UpdateStaminaUI();
                    }
                    activeMoveSpeed = runSpeed;
                    recoverySpeed = runRecoverySpeed;
                    Run = true;

                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    activeMoveSpeed = moveSpeed;
                    recoverySpeed = usuallyRecoverySpeed;
                    Run = false;

                }

                //回避していないとき
                if (dashCounter <= 0)
                {
                    //Spaceををしたときかつスタミナが十分あるか
                    if (Input.GetKeyDown(KeyCode.Space) && PlayerStatusTakeOver.currentStamina > dashCost)
                    {
                        this.gameObject.layer = 12;
                        invincibilityCounter = invincibilityTime;   //無敵時間を与える

                        activeMoveSpeed = dashSpeed;    //スピードの切り替え
                        dashCounter = dashLength;   //ダッシュの時間を設定

                        PlayerStatusTakeOver.currentStamina -= dashCost; //スタミナ減らす

                        GameManager.instance.UpdateStaminaUI(); //UIの更新
                        Dash = true;
                    }
                }
                //回避していたら
                else
                {
                    dashCounter -= Time.deltaTime;  //ダッシュ時間を減らす

                    //回避が終わったか
                    if (dashCounter <= 0)
                    {
                        this.gameObject.layer = 3;
                        activeMoveSpeed = moveSpeed;    //moveSpeedに切り替え
                        Dash = false;
                    }
                }
            }

            if (!Dash && !Run)
            {
                PlayerStatusTakeOver.currentStamina = Mathf.Clamp(PlayerStatusTakeOver.currentStamina + recoverySpeed * Time.deltaTime, 0, PlayerStatusTakeOver.totalStamina); //スタミナ回復用
                GameManager.instance.UpdateStaminaUI(); //UIの更新
            }
        }
    }

    void PlayerTurning()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;

        PlayerAnimation(direction.x, direction.y);
    }

    void PlayerAnimation(float x, float y)
    {
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        playerAnim.SetFloat("X", x);
        playerAnim.SetFloat("Y", y);
        weponAnim.SetFloat("X", x);
        weponAnim.SetFloat("Y", y);
    }
        

    /// <summary>
    /// 吹き飛ばし用の関数
    /// </summary>
    /// <param name="position"></param>
    //                      ↓で呼び出すときVector3型の値を渡せる
    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        isknockingback = true;

        knockDir = transform.position - position;   //playerの現在値からEnemyの現在を引いている

        knockDir.Normalize();
    }
        /// <summary>
        /// ダメージ用の関数
        /// </summary>
        /// <param name="damage"></param>
    public void DamagePlayer(int damage)
    {

        //無敵時間が無ければ
        if(invincibilityCounter <= 0)
        {
            PlayerStatusTakeOver.currentHP = Mathf.Clamp(PlayerStatusTakeOver.currentHP - damage, 0, PlayerStatusTakeOver.maxHP);  //hpが-に行かないよう

            invincibilityCounter = invincibilityTime;   //無敵時間を与える

            SoundManager.instance.PlaySE(2);

            //hpが0になったら
            if(PlayerStatusTakeOver.currentHP == 0)
            {
                gameObject.SetActive(false);    //player非表示
                SoundManager.instance.PlaySE(0);
                GameManager.instance.ResetGame();
                GameManager.instance.Load();
            }
        }

        GameManager.instance.UpdateHealthUI();
    }

    //焚火に当たった時の関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bonfire" && PlayerStatusTakeOver.maxLT != PlayerStatusTakeOver.currentLT)
        {
            SoundManager.instance.PlaySE(6);
            PlayerStatusTakeOver.currentLTIntensity = PlayerStatusTakeOver.maxLTIntensity;
            PlayerStatusTakeOver.curretnLTInnerRadius = PlayerStatusTakeOver.maxLTInnerRadius;
            PlayerStatusTakeOver.currentLTOuterRadius = PlayerStatusTakeOver.maxLTOuterRadius;
            BonfireManager bonfire = collision.GetComponent<BonfireManager>();

            PlayerStatusTakeOver.currentLT = Mathf.Clamp(PlayerStatusTakeOver.currentLT + PlayerStatusTakeOver.maxLT, 0, PlayerStatusTakeOver.maxLT);
            GameManager.instance.UpdateLTUI();
        }
    }

    public void GunSetUp()
    {
        _NowGun = GameManager.instance.NowGun();
        shotCooldown = _NowGun.GetShotCooldown();
        fullAuto = _NowGun.GetFullAuto();
        reloadTime = _NowGun.GetReloadTime();
        maxAmmo = _NowGun.GetMaxAmmo();
        PlayerStatusTakeOver.currentAmmo = maxAmmo;
        GameManager.instance.UpdateAmmo();
    }
}
