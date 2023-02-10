using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int healthItemRecoveryValue; //アイテム回復量変数

    [SerializeField]
    private float lifeTime; //ロスト時間用変数

    public float waitTime;  //取得可能までの時間変数(見えないうちに取得してしまわないように)
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);  //ロストの設定
    }

    // Update is called once per frame
    void Update()
    {
        //取得時間が0より大きかったら
        if(waitTime >0 )
        {
            waitTime -= Time.deltaTime; //取得待機時間を減らす
        }
    }
}
