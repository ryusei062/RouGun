using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "Item/CreateItem")]
public class Item : ScriptableObject
{

    public enum KindOfItem
    {
        UseItem
    }

    //�@�A�C�e���̎��
    [SerializeField]
    private KindOfItem kindOfItem;
    //�@�A�C�e���̃A�C�R��
    [SerializeField]
    private Sprite icon;
    //�@�A�C�e���̖��O
    [SerializeField]
    private string itemName;
    //�@�A�C�e���̏��
    [SerializeField]
    private string information;
    //�@�A�C�e���̉񕜗�
    [SerializeField]
    private int recoveryHP;
    [SerializeField]
    private int recoveryST;
    [SerializeField]
    private int recoveryLT;
    [SerializeField]
    private int exMag;
    [SerializeField]
    private int exHealth;




    public KindOfItem GetKindOfItem()
    {
        return kindOfItem;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetInformation()
    {
        return information;
    }

    public int GetRecoveryHP()
    {
        return recoveryHP;
    }

    public int GetRecoveryST()
    {
        return recoveryST;
    }

    public int GetRecoveryLT()
    {
        return recoveryLT;
    }

    public int GetExMag()
    {
        return exMag;
    }

    public int GetExHealth()
    {
        return exHealth;
    }
}