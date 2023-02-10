using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int healthItemRecoveryValue; //�A�C�e���񕜗ʕϐ�

    [SerializeField]
    private float lifeTime; //���X�g���ԗp�ϐ�

    public float waitTime;  //�擾�\�܂ł̎��ԕϐ�(�����Ȃ������Ɏ擾���Ă��܂�Ȃ��悤��)
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);  //���X�g�̐ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        //�擾���Ԃ�0���傫��������
        if(waitTime >0 )
        {
            waitTime -= Time.deltaTime; //�擾�ҋ@���Ԃ����炷
        }
    }
}
