using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //シングルトン化(soundManagerはこれしかない)
    public static SoundManager instance;

    public AudioSource[] se;    //SE格納用配列

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        //中身はこれか
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);  //シーンを移動したら普通はゲームオブジェクトは壊れるがこれで壊れなくする
    }
    

    /// <summary>
    /// SEをならす(0:ゲームオーバー 1:回復 2:被弾 3:攻撃 4:UI 5:銃声 6:焚火 7:弾切れ 8:リロード)
    /// </summary>
    /// <param name="x"></param>
    //SE再生用関数
    public void PlaySE(int x)
    {
        se[x].Stop();

        se[x].Play();
    }
}
