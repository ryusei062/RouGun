using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//���C�u�����̒ǉ�(UI���R�[�h���爵�����߂ɒǉ�����)
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static �ǂ�����ł��Ăяo����
    public static GameManager instance = null;

    //�ϐ��̐錾
    [SerializeField]
    private OverrideLight2D light2d;

    [SerializeField]
    private Slider hpSlider;    //HP�o�[�ϐ�
    [SerializeField]
    private Text hpText; //HP�\��
    [SerializeField]
    private Slider staminaSlider;   //�X�^�~�i�o�[�ϐ�
    [SerializeField]
    private Text staminaText;    //ST�\��
    [SerializeField]
    private Slider ltSlider;   //�X�^�~�i�o�[�ϐ�
    [SerializeField]
    private Text ltText;    //ST�\��
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private Text psLVtext;
    [SerializeField]
    private Text psSPtext;
    [SerializeField]
    private Text psHPtext;
    [SerializeField]
    private Text psSTtext;
    [SerializeField]
    private Text psPWtext;
    [SerializeField]
    private Text psExptext;
    [SerializeField]
    private GameObject psHPButton;
    [SerializeField]
    private GameObject psSTButton;
    [SerializeField]
    private GameObject psPWButton;

    [SerializeField]
    private PlayerController player;    //Player�͂ǂꂩ�ϐ�

    public GameObject dialogBox;    //�_�C�A���O�p�̕ϐ�
    public Text dialogText; //�\�����镶�͂̕ϐ�

    private string[] dialogLines;   //�e�L�X�g�i�[�p�̔z��
    private int currentLine;    //�����s�ڂ��̕ϐ�
    private bool justStarted;   //������������Ȃ�����p�ϐ�

    public GameObject playerLogBox;    //�v���C���[���O�p�̕ϐ�
    public Text playerLogText; //�\�����镶�͂̕ϐ�

    public bool isPaused;

    public List<Item> items = new List<Item>();
    public GameObject[] slots;

    public Item addItem_01;
    public Item removeItem_01;

    public List<GunStatus> guns = new List<GunStatus>();
    public GameObject[] gunSlots;

    public GunStatus addGun_01;
    public GunStatus removeGun_01;

    public GunStatus selectGun;

    public GameObject StatusMenu;
    public int selectSlot = 0;


    //��ԍŏ��ɌĂяo�����
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnActiveSceneChanged(Scene unloadedScene, Scene loadedScene)
    {
        light2d = GameObject.FindGameObjectWithTag("Player").GetComponent<OverrideLight2D>();
        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpText = GameObject.Find("HPText").GetComponent<Text>();
        staminaSlider = GameObject.Find("STSlider").GetComponent<Slider>();
        staminaText = GameObject.Find("STText").GetComponent<Text>();
        ltSlider = GameObject.Find("LTSlider").GetComponent<Slider>();
        ltText = GameObject.Find("LTText").GetComponent<Text>();
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        dialogBox = GameObject.Find("DialogBox");
        dialogText = GameObject.Find("DialogText").GetComponent<Text>();
        playerLogBox = GameObject.Find("PlayerLogBox");
        playerLogText = GameObject.Find("PlayerLogText").GetComponent<Text>();
        psLVtext = GameObject.Find("PSLevelText").GetComponent<Text>();
        psSPtext = GameObject.Find("PSSpText").GetComponent<Text>();
        psHPtext = GameObject.Find("PSHpText").GetComponent<Text>();
        psSTtext = GameObject.Find("PSStText").GetComponent<Text>();
        psPWtext = GameObject.Find("PSPowerText").GetComponent<Text>();
        psExptext = GameObject.Find("PSExpText").GetComponent<Text>();
        psHPButton = GameObject.Find("HpButton");
        psSTButton = GameObject.Find("StButton");
        psPWButton = GameObject.Find("PowerButton");

        for (int i = 0; i<=5;i++)
        {
            slots[i] = GameObject.Find("Content" + i.ToString());

        }

        for (int i = 0; i <= 2; i++)
        {
            gunSlots[i] = GameObject.Find("GunContent" + i.ToString());

        }

        DisplayItems();
        playerLogBox.SetActive(false);
        dialogBox.SetActive(false);

    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void Start()
    {
        DisplayItems();
        DisplayGuns();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SelectGun();
        }

        if(PlayerStatusTakeOver.floorLevel >= 21)
        {
            ClearGame();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AddItem(addItem_01);
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            RemoveItem(removeItem_01);
        }

        //dialog���\������Ă��邩
        if(dialogBox.activeInHierarchy)
        {
            //�E�N���b�N�������ꂽ��
            if(Input.GetMouseButtonUp(1))
            {
                SoundManager.instance.PlaySE(4);

                //justStarted��false��
                if(!justStarted)
                {
                    currentLine++;  //�s�̍X�V

                    //���\�����Ă���s�͍Ō�̍s��
                    if(currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    //�܂��\������̂����邩
                    else
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                }
                //true�Ȃ�
                else
                {
                    justStarted = false;
                }
            }
        }
    }

    //�v���C���[��HP��UI�ɔ��f���邽�߂̊֐�
    public void UpdateHealthUI()
    {
        hpSlider.maxValue = PlayerStatusTakeOver.maxHP;   //�X���C�_�[�̍ő�l��hp�̍ő�l
        hpSlider.value = PlayerStatusTakeOver.currentHP;  //�X���C�_�[�̌��ݒl��hp�̌��ݒl
        hpText.text = PlayerStatusTakeOver.currentHP.ToString() + "/" + PlayerStatusTakeOver.maxHP.ToString();
    }

    //�v���C���[�̃X�^�~�i��UI�ɔ��f���邽�߂̊֐�
    public void UpdateStaminaUI()
    {
        staminaSlider.maxValue = PlayerStatusTakeOver.totalStamina;   //�X���C�_�[�̍ő�l��hp�̍ő�l
        staminaSlider.value = PlayerStatusTakeOver.currentStamina;  //�X���C�_�[�̌��ݒl��hp�̌��ݒl
        staminaText.text = Mathf.FloorToInt(PlayerStatusTakeOver.currentStamina).ToString() + "/" + PlayerStatusTakeOver.totalStamina.ToString();
    }

    //�v���C���[��LT��UI�ɔ��f���邽�߂̊֐�
    public void UpdateLTUI()
    {
        ltSlider.maxValue = PlayerStatusTakeOver.maxLT;   //�X���C�_�[�̍ő�l��hp�̍ő�l
        ltSlider.value = PlayerStatusTakeOver.currentLT;  //�X���C�_�[�̌��ݒl��hp�̌��ݒl
        ltText.text = Mathf.FloorToInt(PlayerStatusTakeOver.currentLT).ToString() + "/" + PlayerStatusTakeOver.maxLT.ToString();
    }

    //�K�w��\������֐�
    public void UpdateLevel()
    {
        levelText.text = PlayerStatusTakeOver.floorLevel.ToString() + "F";
    }

    public void UpdateAmmo()
    {
        ammoText.text = PlayerStatusTakeOver.currentAmmo.ToString() + "/" + guns[selectSlot].GetMaxAmmo().ToString();
    }


    //�_�C�A���O��\�����ĕ\�����镶�͂�ݒ肷��֐�
    public void ShowDialog(string[] lines)
    {
        dialogLines = lines; //��b���e���i�[����

        currentLine = 0;    //�ŏ��̗v�f���\��

        dialogText.text = dialogLines[currentLine]; //�e�L�X�g�ɍŏ��̍s���\��
        dialogBox.SetActive(true);  //dialogbox�\��

        justStarted = true; //��������J�n
    }

    //�_�C�A���O�̕\���ؑւ̊֐�
    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
    }

    public void Load()
    {
        SceneManager.LoadScene("Title");
    }

    public void ShowPlayerLog(string message)
    {
        playerLogText.text = message;
        playerLogBox.SetActive(true);  //dialogbox�\��
    }

    public void ShowPlayerLogChange(bool x)
    {
        playerLogBox.SetActive(x);
    }

    private void DisplayItems()
    {
        for(int i = 0;i < slots.Length; i++)
        {
            if(i < items.Count)
            {

                    //�摜�\��
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetIcon();

                //remove�{�^��
                slots[i].transform.GetChild(2).gameObject.SetActive(true);
                //use�{�^��                
                slots[i].transform.GetChild(1).gameObject.SetActive(true);


            }
            else
            {
                //�摜�\��
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;

                //remove�{�^��
                slots[i].transform.GetChild(2).gameObject.SetActive(false);
                //use
                slots[i].transform.GetChild(1).gameObject.SetActive(false);



            }

        }
    }

    public void AddItem(Item _item)
    {
        items.Add(_item);
        DisplayItems();
    }

    public void RemoveItem(Item _item)
    {
        if(items.Contains(_item))
        {
                items.Remove(_item);
        }
        else
        {
            Debug.Log("���̃A�C�e���������Ă��܂���");
        }

        DisplayItems();
    }

    public void UseItem(Item _item)
    {
        if (items.Contains(_item))
        {
            if(_item.GetRecoveryHP() > 0)
            {
                SoundManager.instance.PlaySE(1);
                PlayerStatusTakeOver.currentHP = Mathf.Clamp(PlayerStatusTakeOver.currentHP + _item.GetRecoveryHP(), 0, PlayerStatusTakeOver.maxHP);
                UpdateHealthUI();
            }
            if (_item.GetRecoveryST() > 0)
            {
                SoundManager.instance.PlaySE(1);
                PlayerStatusTakeOver.currentStamina = Mathf.Clamp(PlayerStatusTakeOver.currentStamina + _item.GetRecoveryST(), 0, PlayerStatusTakeOver.totalStamina);
                UpdateStaminaUI();
            }
            if (_item.GetRecoveryLT() > 0)
            {
                SoundManager.instance.PlaySE(1);
                PlayerStatusTakeOver.currentLTIntensity = PlayerStatusTakeOver.maxLTIntensity;
                PlayerStatusTakeOver.curretnLTInnerRadius = PlayerStatusTakeOver.maxLTInnerRadius;
                PlayerStatusTakeOver.currentLTOuterRadius = PlayerStatusTakeOver.maxLTOuterRadius;

                PlayerStatusTakeOver.currentLT = Mathf.Clamp(PlayerStatusTakeOver.currentLT + _item.GetRecoveryLT(), 0, PlayerStatusTakeOver.maxLT);
                UpdateLTUI();
            }
            if (_item.GetExMag() > 0)
            {
                SoundManager.instance.PlaySE(8);
                PlayerStatusTakeOver.maxAmmo += _item.GetExMag();
                PlayerStatusTakeOver.currentAmmo = PlayerStatusTakeOver.maxAmmo;

                UpdateAmmo();
            }
            if(_item.GetExHealth() > 0)
            {
                SoundManager.instance.PlaySE(1);
                PlayerStatusTakeOver.maxHP += _item.GetExHealth();

                UpdateHealthUI();
            }
            items.Remove(_item);
        }
        else
        {
            Debug.Log("���̃A�C�e���͎g���܂���");
        }

        DisplayItems();
    }

    private void DisplayGuns()
    {

        if(selectSlot == 0)
        {
            selectGun = guns[0];
            gunSlots[0].transform.GetChild(0).GetComponent<Image>().sprite = guns[0].GetGunIcon();
            gunSlots[0].gameObject.SetActive(true);
            gunSlots[1].gameObject.SetActive(false);
            gunSlots[2].gameObject.SetActive(false);
        }
        else if(selectSlot == 1)
        {
            selectGun = guns[1];
            gunSlots[1].transform.GetChild(0).GetComponent<Image>().sprite = guns[1].GetGunIcon();
            gunSlots[1].gameObject.SetActive(true);
            gunSlots[0].gameObject.SetActive(false);
            gunSlots[2].gameObject.SetActive(false);
        }
        else if(selectSlot == 2)
        {
            selectGun = guns[2];
            gunSlots[2].transform.GetChild(0).GetComponent<Image>().sprite = guns[2].GetGunIcon();
            gunSlots[2].gameObject.SetActive(true);
            gunSlots[1].gameObject.SetActive(false);
            gunSlots[0].gameObject.SetActive(false);
        }

    }

    public void AddGun(GunStatus _gun)
    {
        guns.Add(_gun);
        DisplayGuns();
    }

    public void RemoveGun(GunStatus _gun)
    {
        if (Input.GetKey("Q"))
        {
            guns.Remove(_gun);
        }
        else
        {
            Debug.Log("���̃A�C�e���������Ă��܂���");
        }

        DisplayItems();
    }

    public void SelectGun()
    {
        if (selectSlot == 0)
        {
            selectSlot = 1;
        }
        else if (selectSlot == 1)
        {
            selectSlot = 2;
        }
        else if (selectSlot == 2)
        {
            selectSlot = 0;
        }
        UpdateAmmo();
        DisplayGuns();
    }

    public GunStatus NowGun()
    {
        return guns[selectSlot];
    }




    public void ResetGame()
    {
        PlayerStatusTakeOver.currentHP = PlayerStatusDefault.maxHP;
        PlayerStatusTakeOver.currentLT = PlayerStatusDefault.maxLT;
        PlayerStatusTakeOver.currentStamina = PlayerStatusDefault.totalStamina;
        PlayerStatusTakeOver.maxAmmo = PlayerStatusDefault.maxAmmo;
        PlayerStatusTakeOver.currentAmmo = PlayerStatusDefault.maxAmmo;
        PlayerStatusTakeOver.currentLTIntensity = PlayerStatusDefault.maxLTIntensity;
        PlayerStatusTakeOver.currentLTOuterRadius = PlayerStatusDefault.currentLTOuterRadius;
        PlayerStatusTakeOver.curretnLTInnerRadius = PlayerStatusDefault.curretnLTInnerRadius;
        PlayerStatusTakeOver.floorLevel = PlayerStatusDefault.floorLevel;

        UpdateAmmo();
        UpdateHealthUI();
        UpdateLevel();
        UpdateLTUI();
        UpdateStaminaUI();
    }

    public void ClearGame()
    {
        ResetGame();
        SceneManager.LoadScene("Clear");
    }

    public void HideAmmoUI()
    {
        ammoText.gameObject.SetActive(false);
    }

    public void TrueAmmoUI()
    {
        ammoText.gameObject.SetActive(true);
    }

    public void PlayerStatusUpdate()
    {

        if(PlayerStatusTakeOver.currentSP <= 0)
        {
            psHPButton.gameObject.SetActive(false);
            psSTButton.gameObject.SetActive(false);
            psPWButton.gameObject.SetActive(false);
        }
        else
        {
            psHPButton.gameObject.SetActive(true);
            psSTButton.gameObject.SetActive(true);
            psPWButton.gameObject.SetActive(true);




        }

        psLVtext.text = "LV:" + PlayerStatusTakeOver.currentLv.ToString();
        psSPtext.text = "SP:" + PlayerStatusTakeOver.currentSP.ToString();
        psHPtext.text = "HP:" + PlayerStatusTakeOver.maxHP.ToString();
        psSTtext.text = "ST:" + PlayerStatusTakeOver.totalStamina.ToString();
        psPWtext.text = "PW:" + PlayerStatusTakeOver.currentPW.ToString();
        psExptext.text = "Exp:" + PlayerStatusTakeOver.currenExp.ToString() + "/" + PlayerStatusTakeOver.needExp.ToString();
    }
}
