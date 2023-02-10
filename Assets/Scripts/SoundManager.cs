using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //�V���O���g����(soundManager�͂��ꂵ���Ȃ�)
    public static SoundManager instance;

    public AudioSource[] se;    //SE�i�[�p�z��

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        //���g�͂��ꂩ
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);  //�V�[�����ړ������畁�ʂ̓Q�[���I�u�W�F�N�g�͉��邪����ŉ��Ȃ�����
    }
    

    /// <summary>
    /// SE���Ȃ炷(0:�Q�[���I�[�o�[ 1:�� 2:��e 3:�U�� 4:UI 5:�e�� 6:���� 7:�e�؂� 8:�����[�h)
    /// </summary>
    /// <param name="x"></param>
    //SE�Đ��p�֐�
    public void PlaySE(int x)
    {
        se[x].Stop();

        se[x].Play();
    }
}
