using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Stair : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayerStatusTakeOver.floorLevel += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerStatusTakeOver.floorLevel += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
