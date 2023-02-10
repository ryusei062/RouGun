using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLog : MonoBehaviour
{

    [SerializeField, Header("表示文章"), Multiline(3)]  //Header("タイトル"), Multiline(表示したい行数)
    private string message;
    private bool canPLActivator;  //ダイアログの表示判定用の変数
    private float timecount;

    private void Update()
    {
        timecount += Time.deltaTime;
    }
    //衝突判定の関数
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーが触れたとき
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.ShowPlayerLog(message);
            canPLActivator = true;
        }
    }

    //離れたときの関数
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(timecount >= 5 && collision.gameObject.tag == "Player")
        {
            canPLActivator = false;

            GameManager.instance.ShowPlayerLogChange(canPLActivator);    //ShowPlayerLogChangeにcanactivatorの値を渡し呼び出す
        }
    }
}
