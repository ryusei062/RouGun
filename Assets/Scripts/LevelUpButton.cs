using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpButton : MonoBehaviour
{
    public void HpUp()
    {
        PlayerStatusTakeOver.maxHP += 10;
        PlayerStatusTakeOver.currentSP -= 1;
        GameManager.instance.PlayerStatusUpdate();
        GameManager.instance.UpdateHealthUI();
    }

    public void StUp()
    {
        PlayerStatusTakeOver.totalStamina += 10;
        PlayerStatusTakeOver.currentSP -= 1;
        GameManager.instance.PlayerStatusUpdate();
        GameManager.instance.UpdateStaminaUI();
    }

    public  void PwUp()
    {
        PlayerStatusTakeOver.currentPW += 5;
        PlayerStatusTakeOver.currentSP -= 1;
        GameManager.instance.PlayerStatusUpdate();
    }
}
