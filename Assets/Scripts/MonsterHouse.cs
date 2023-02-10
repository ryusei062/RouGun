using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHouse : MonoBehaviour
{
    public GameObject[] enemy;
    public GameObject[] wall;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].SetActive(false);
        }

        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i].SetActive(true);
            }
        }
    }
}
