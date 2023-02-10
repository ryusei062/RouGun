using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaialogActivater : MonoBehaviour
{

    [SerializeField, Header("会話文章"), Multiline(3)]
    private string[] lines; //会話文章の配列

    private bool canActivator;  //ダイアログの表示判定用の変数
}
