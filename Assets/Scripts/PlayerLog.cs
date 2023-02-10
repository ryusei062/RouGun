using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLog : MonoBehaviour
{

    [SerializeField, Header("�\������"), Multiline(3)]  //Header("�^�C�g��"), Multiline(�\���������s��)
    private string message;
    private bool canPLActivator;  //�_�C�A���O�̕\������p�̕ϐ�
    private float timecount;

    private void Update()
    {
        timecount += Time.deltaTime;
    }
    //�Փ˔���̊֐�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[���G�ꂽ�Ƃ�
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.ShowPlayerLog(message);
            canPLActivator = true;
        }
    }

    //���ꂽ�Ƃ��̊֐�
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(timecount >= 5 && collision.gameObject.tag == "Player")
        {
            canPLActivator = false;

            GameManager.instance.ShowPlayerLogChange(canPLActivator);    //ShowPlayerLogChange��canactivator�̒l��n���Ăяo��
        }
    }
}
