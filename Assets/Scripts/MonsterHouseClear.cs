using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHouseClear : MonoBehaviour
{
    public GameObject[] wall;

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < wall.Length; i++)
            {
                wall[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < wall.Length; i++)
            {
                wall[i].SetActive(false);
            }
        }
    }

}
