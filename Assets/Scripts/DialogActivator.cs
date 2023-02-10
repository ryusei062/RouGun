using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivater : MonoBehaviour
{

    [SerializeField, Header("会話文章"), Multiline(3)]  //Header("タイトル"), Multiline(表示したい行数)
    private string[] lines; //会話文章の配列

    private bool canActivator;  //ダイアログの表示判定用の変数



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //右クリックされたときかつcanActivatorがtureのときかつdialogBoxが表示されていないとき
        if (Input.GetMouseButtonDown(1) && canActivator && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.HideAmmoUI();
            GameManager.instance.ShowDialog(lines); //linesの値を渡しShwodialogを呼び出す
        }
    }

    //衝突判定の関数
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーが触れたとき
        if (collision.gameObject.tag == "Player")
        {
            canActivator = true;
        }
    }

    //離れたときの関数
    private void OnCollisionExit2D(Collision2D collision)
    {
        //プレイヤーが離れたとき
        if (collision.gameObject.tag == "Player")
        {
            canActivator = false;

            GameManager.instance.ShowDialogChange(canActivator);    //ShowDialogChangeにcanactivatorの値を渡し呼び出す
            GameManager.instance.TrueAmmoUI();
        }
    }


}
