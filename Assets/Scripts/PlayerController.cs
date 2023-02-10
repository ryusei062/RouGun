using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ��X�s�[�h")]    //����ŃG�f�B�^��Ő��l�������@Tooltip("����")�Ő�����\���ł���
    private int moveSpeed;  //�ړ��X�s�[�h�ϐ� private�͎Q�Ƃł��Ȃ��Ȃ�i���̃X�N���v�g����l��ς����Ȃ��Ȃ�j,public�͂ǂ�����ł��Q�Ƃł��ς����

    [SerializeField]
    private Animator playerAnim;    //�A�j���[�V�����p�̕ϐ�

    public Rigidbody2D rb;  //���W�b�h�{�f�B�̕ϐ�

    [SerializeField]
    private Animator weponAnim; //����A�j���[�V�����p�̕ϐ�

    [System.NonSerialized]  //����ŃG�f�B�^��ɕ\������Ȃ�


    private bool isknockingback;    //�m�b�N�o�b�N���Ă��邩�ϐ�
    private Vector2 knockDir; //������ԕ����ϐ�

    [SerializeField]
    private float knockbackTime, knockbackForce;    //������Ԏ��ԂƗ͕ϐ�
    private float knockbackCounter; //��̃^�C�}�[�p�ϐ�

    [SerializeField]
    private float invincibilityTime;    //���G���ԕϐ�
    private float invincibilityCounter; //��̃^�C�}�[�p�ϐ�

    public float usuallyRecoverySpeed;   //�X�^�~�i�ő�l�A�X�^�~�i�񕜑��x�̕ϐ�
    private float recoverySpeed, runRecoverySpeed = 0;
    [System.NonSerialized]

    [SerializeField]
    private OverrideLight2D light2d;
    [System.NonSerialized]
    private float ltTimer = 1f;

    [SerializeField]
    private float dashSpeed, dashLength, dashCost, runSpeed, runCost;    //����̑��x�A�����̕ϐ�,�X�^�~�i�̏���ʕϐ�

    private float dashCounter, activeMoveSpeed; //����̃^�C�}�[�A�ړ����ɂ�����p�̕ϐ�

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
        GameManager.instance.UpdateHealthUI();  //static����GameManager�̊֐����ȒP�ɌĂяo����

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
                PlayerStatusTakeOver.currentLT = Mathf.Clamp(PlayerStatusTakeOver.currentLT - Time.deltaTime, 0, PlayerStatusTakeOver.maxHP);  //LT�Q�[�W����
                GameManager.instance.UpdateLTUI();
            }
            else if (PlayerStatusTakeOver.currentLT <= 0 && ltTimer <= 0)
            {
                PlayerStatusTakeOver.currentHP -= 1;
                ltTimer = 1f;
                GameManager.instance.UpdateHealthUI();
            }

            //���G���Ԃ�
            if (invincibilityCounter > 0)
            {
                invincibilityCounter -= Time.deltaTime;
            }

            //�m�b�N�o�b�N����
            if (isknockingback)
            {
                knockbackCounter -= Time.deltaTime;
                rb.velocity = knockDir * knockbackForce;

                //�m�b�N�o�b�N�I�������
                if (knockbackCounter <= 0)
                {
                    isknockingback = false;
                }
                else
                {
                    return;     //return;�͈ȉ��̃R�[�h�͎��s����Ȃ�
                }
            }

            PlayerTurning();

            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed;    //�L�[�{�[�h�̓��͂��󂯎��
                                                                                                                                     //velocity�͑��x���������� Vector2��x,y�̒l���������� .normalized�̓x�N�g���𐳋K������


            shotCooldownTime += Time.deltaTime;
            gunChangeTime -= Time.deltaTime;

            
            if(gunChangeTime <= 0)
            {
                if (fullAuto == false)
                {

                    //�}�E�X�������ꂽ�Ƃ� 0�����N���b�N
                    if (Input.GetMouseButtonDown(0) && shotCooldownTime >= shotCooldown)
                    {
                        if (PlayerStatusTakeOver.currentAmmo <= 0)
                        {
                            SoundManager.instance.PlaySE(7);
                        }
                        else
                        {
                            // �e�i�Q�[���I�u�W�F�N�g�j�̐���
                            GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                            // �N���b�N�������W�̎擾�i�X�N���[�����W���烏�[���h���W�ɕϊ��j
                            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            // �����̐����iZ�����̏����Ɛ��K���j
                            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;
                            // �e�ɑ��x��^����
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
                            // �e�i�Q�[���I�u�W�F�N�g�j�̐���
                            GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);
                            // �N���b�N�������W�̎擾�i�X�N���[�����W���烏�[���h���W�ɕϊ��j
                            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            // �����̐����iZ�����̏����Ɛ��K���j
                            Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;
                            // �e�ɑ��x��^����
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
                //run���̏���
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

                //������Ă��Ȃ��Ƃ�
                if (dashCounter <= 0)
                {
                    //Space���������Ƃ����X�^�~�i���\�����邩
                    if (Input.GetKeyDown(KeyCode.Space) && PlayerStatusTakeOver.currentStamina > dashCost)
                    {
                        this.gameObject.layer = 12;
                        invincibilityCounter = invincibilityTime;   //���G���Ԃ�^����

                        activeMoveSpeed = dashSpeed;    //�X�s�[�h�̐؂�ւ�
                        dashCounter = dashLength;   //�_�b�V���̎��Ԃ�ݒ�

                        PlayerStatusTakeOver.currentStamina -= dashCost; //�X�^�~�i���炷

                        GameManager.instance.UpdateStaminaUI(); //UI�̍X�V
                        Dash = true;
                    }
                }
                //������Ă�����
                else
                {
                    dashCounter -= Time.deltaTime;  //�_�b�V�����Ԃ����炷

                    //������I�������
                    if (dashCounter <= 0)
                    {
                        this.gameObject.layer = 3;
                        activeMoveSpeed = moveSpeed;    //moveSpeed�ɐ؂�ւ�
                        Dash = false;
                    }
                }
            }

            if (!Dash && !Run)
            {
                PlayerStatusTakeOver.currentStamina = Mathf.Clamp(PlayerStatusTakeOver.currentStamina + recoverySpeed * Time.deltaTime, 0, PlayerStatusTakeOver.totalStamina); //�X�^�~�i�񕜗p
                GameManager.instance.UpdateStaminaUI(); //UI�̍X�V
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
    /// ������΂��p�̊֐�
    /// </summary>
    /// <param name="position"></param>
    //                      ���ŌĂяo���Ƃ�Vector3�^�̒l��n����
    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        isknockingback = true;

        knockDir = transform.position - position;   //player�̌��ݒl����Enemy�̌��݂������Ă���

        knockDir.Normalize();
    }
        /// <summary>
        /// �_���[�W�p�̊֐�
        /// </summary>
        /// <param name="damage"></param>
    public void DamagePlayer(int damage)
    {

        //���G���Ԃ��������
        if(invincibilityCounter <= 0)
        {
            PlayerStatusTakeOver.currentHP = Mathf.Clamp(PlayerStatusTakeOver.currentHP - damage, 0, PlayerStatusTakeOver.maxHP);  //hp��-�ɍs���Ȃ��悤

            invincibilityCounter = invincibilityTime;   //���G���Ԃ�^����

            SoundManager.instance.PlaySE(2);

            //hp��0�ɂȂ�����
            if(PlayerStatusTakeOver.currentHP == 0)
            {
                gameObject.SetActive(false);    //player��\��
                SoundManager.instance.PlaySE(0);
                GameManager.instance.ResetGame();
                GameManager.instance.Load();
            }
        }

        GameManager.instance.UpdateHealthUI();
    }

    //���΂ɓ����������̊֐�
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
