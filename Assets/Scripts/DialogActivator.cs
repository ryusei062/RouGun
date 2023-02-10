using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivater : MonoBehaviour
{

    [SerializeField, Header("��b����"), Multiline(3)]  //Header("�^�C�g��"), Multiline(�\���������s��)
    private string[] lines; //��b���͂̔z��

    private bool canActivator;  //�_�C�A���O�̕\������p�̕ϐ�



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�E�N���b�N���ꂽ�Ƃ�����canActivator��ture�̂Ƃ�����dialogBox���\������Ă��Ȃ��Ƃ�
        if (Input.GetMouseButtonDown(1) && canActivator && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.HideAmmoUI();
            GameManager.instance.ShowDialog(lines); //lines�̒l��n��Shwodialog���Ăяo��
        }
    }

    //�Փ˔���̊֐�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[���G�ꂽ�Ƃ�
        if (collision.gameObject.tag == "Player")
        {
            canActivator = true;
        }
    }

    //���ꂽ�Ƃ��̊֐�
    private void OnCollisionExit2D(Collision2D collision)
    {
        //�v���C���[�����ꂽ�Ƃ�
        if (collision.gameObject.tag == "Player")
        {
            canActivator = false;

            GameManager.instance.ShowDialogChange(canActivator);    //ShowDialogChange��canactivator�̒l��n���Ăяo��
            GameManager.instance.TrueAmmoUI();
        }
    }


}
