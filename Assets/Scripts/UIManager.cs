using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject StatusMenu;
    public GameObject tooltip;

    public GameObject pauseMenu;

    private bool inv = false;
    private bool paus = false;

    public int mode = 0;

    private void Start()
    {
        inventoryMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        StatusMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        InventoryControl();
        PauseControl();
    }

    private void InventoryControl()
    {
        if(Input.GetKeyDown(KeyCode.E) && !paus)
        {
            SoundManager.instance.PlaySE(4);
            if(GameManager.instance.isPaused)
            {
                inv = false;
                InvResume();
            }
            else
            {
                inv = true;
                InvPause();
            }
        }
    }

    private void PauseControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !inv)
        {
            SoundManager.instance.PlaySE(4);
            if (GameManager.instance.isPaused)
            {
                paus = false;
                EscResume();
            }
            else
            {
                paus = true;
                EscPause();
            }
        }

    }

    private void InvResume()
    {

        inventoryMenu.gameObject.SetActive(false);
        tooltip.gameObject.SetActive(false);
        StatusMenu.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
        GameManager.instance.isPaused = false;

    }

    private void InvPause()
    {

        inventoryMenu.gameObject.SetActive(true);
        StatusMenu.gameObject.SetActive(true);
        GameManager.instance.PlayerStatusUpdate();
        
        Time.timeScale = 0.0f;
        GameManager.instance.isPaused = true;
    }

    private void EscResume()
    {
        pauseMenu.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
        GameManager.instance.isPaused = false;

    }

    private void EscPause()
    {

        pauseMenu.gameObject.SetActive(true);

        Time.timeScale = 0.0f;
        GameManager.instance.isPaused = true;
    }


}
