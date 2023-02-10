using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //�V�[����J�ڂ����邽�߂̃��C�u����

public class Title : MonoBehaviour
{
    //��ʑJ�ڂ̂��߂̊֐�
    public void GameStart()
    {
        SceneManager.LoadScene("Main"); //SceneManager.LoadScene("��т����V�[��")�ŃV�[�����ړ�
        SoundManager.instance.PlaySE(5);    //4�𗬂�
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
