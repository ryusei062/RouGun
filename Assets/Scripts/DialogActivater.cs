using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaialogActivater : MonoBehaviour
{

    [SerializeField, Header("��b����"), Multiline(3)]
    private string[] lines; //��b���͂̔z��

    private bool canActivator;  //�_�C�A���O�̕\������p�̕ϐ�
}
