using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon : MonoBehaviour
{

    [SerializeField]
    private int attackDamage;   //剣の攻撃力変数

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //enemyとぶつかったか
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);  //剣のダメージと座標を渡しTakeDamageを呼び出す
            SoundManager.instance.PlaySE(3);
        }
    }
}
