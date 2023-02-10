using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    private GameObject[] enemy;

    // Update is called once per frame
    void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("BossEnemy");

        if(enemy.Length == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
