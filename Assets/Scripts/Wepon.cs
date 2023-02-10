using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon : MonoBehaviour
{

    [SerializeField]
    private int attackDamage;   //���̍U���͕ϐ�

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
        //enemy�ƂԂ�������
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);  //���̃_���[�W�ƍ��W��n��TakeDamage���Ăяo��
            SoundManager.instance.PlaySE(3);
        }
    }
}
