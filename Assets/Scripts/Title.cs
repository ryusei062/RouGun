using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //シーンを遷移させるためのライブラリ

public class Title : MonoBehaviour
{
    //画面遷移のための関数
    public void GameStart()
    {
        SceneManager.LoadScene("Main"); //SceneManager.LoadScene("よびたいシーン")でシーンを移動
        SoundManager.instance.PlaySE(5);    //4を流す
    }

    public void QuietGame()
    {
        UnityEngine.Application.Quit();
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlaySE(5);
    }
}
